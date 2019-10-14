using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.FolderAccess;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PTB.Core.Reports
{
    public interface IPTBReportService
    {
        ReportReadResponse Read(BasePTBFile file, int index, int count);

        BaseUpdateResponse Update(BasePTBFile file, int index, PTBRow row);
    }

    public class BaseReportService : IPTBReportService
    {
        protected Encoding _encoding = Encoding.ASCII;
        protected IPTBLogger _logger;
        protected BaseReportParser _parser;
        protected FolderSchema _schema;
        protected FileValidation _validator;

        public BaseReportService(IPTBLogger logger, BaseReportParser parser, FolderSchema schema, FileValidation validator)
        {
            _logger = logger;
            _schema = schema;
            _parser = parser;
            _validator = validator;
        }

        protected void ValidateBuffer(byte[] buffer, string fileName)
        {
            var response = _validator
                .BufferHasByteOrderMark(buffer, fileName)
                .BufferHasNewLine(buffer, fileName)
                .Response;
        }

        protected void ValidateUpdateRow(PTBRow row, string fileName)
        {
            var response = _validator
                .LineValuesMatchColumnSize(row.Columns, row.Index)
                .Response;
        }

        protected bool EveryValueMatchesColumnSize(List<PTBColumn> columns)
        {
            return columns.TrueForAll(column => column.ColumnValue.Length == column.Size);
        }

        protected bool IsFirstLine(int bytesRead, int bufferLength) => bytesRead == bufferLength;

        protected long GetLineNumber(long streamPosition, int lineSize) => streamPosition / (lineSize + System.Environment.NewLine.Length);

        protected byte[] GetBuffer() => new byte[_schema.LineSize + Environment.NewLine.Length];

        protected bool IndexStartsAtCorrectByte(int startIndex)
        {
            if (startIndex == 0) { return true; }

            if (startIndex > 0)
            {
                // subtracts the first byte of the line to the start Index (e.g. a 117 byte line will start the next line on 118)
                return (startIndex % (_schema.LineSize + Environment.NewLine.Length)) == 0;
            }
            return false;
        }

        protected void SetBufferStartIndex(FileStream stream, int startIndex) => stream.Seek(startIndex, SeekOrigin.Begin);

        protected int GetFileLineCount(string path) => Convert.ToInt32(new System.IO.FileInfo(path).Length / (_schema.LineSize + Environment.NewLine.Length));

        public ReportReadResponse Read(BasePTBFile file, int index, int count)
        {
            var response = ReportReadResponse.Default;

            var validationResponse = _validator
                .LineIndexExists(index, _schema.LineSize, file.FileName)
                .Response;

            if (!validationResponse.Success)
            {
                response.Success = validationResponse.Success;
                response.Message = validationResponse.Message;
                return response;
            }

            using (var stream = new FileStream(file.FullPath, FileMode.Open, System.IO.FileAccess.Read))
            {
                int byteIndex = index;
                int bytesRead = 0;
                byte[] buffer = GetBuffer();
                SetBufferStartIndex(stream, index);

                var parseResponse = StringToRowResponse.Default;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0 && count > 0)
                {
                    string line = _encoding.GetString(buffer);

                    if (IsFirstLine(bytesRead, buffer.Length))
                    {
                        ValidateBuffer(buffer, file.FileName);
                    }

                    parseResponse = _parser.ParseLine(line, byteIndex);

                    if (!parseResponse.Success)
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.LineSize);
                        string message = string.Format(ParseMessages.LINE_DATA_CORRUPTION, file.FileName, lineNumber, parseResponse.Message);
                        throw new ParseException(message);
                    }

                    if (parseResponse.Message != ParseMessages.EMPTY_LINE)
                    {
                        response.ReadResult.Add(parseResponse.Row);
                    }

                    count--;
                    byteIndex += bytesRead;
                }
            }

            return response;
        }

        public BaseUpdateResponse Update(BasePTBFile file, int index, PTBRow row)
        {
            var response = BaseUpdateResponse.Default;

            ValidateUpdateRow(row, file.FileName);
            
            var parseResponse = _parser.ParseRow(row);

            if (!parseResponse.Success)
            {
                response.Success = parseResponse.Success;
                response.Message = parseResponse.Message;
                return response;
            }

            using (var stream = new FileStream(file.FullPath, FileMode.Open, System.IO.FileAccess.ReadWrite))
            {
                byte[] buffer = GetBuffer();
                SetBufferStartIndex(stream, index);

                // gets existing record prior to update
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string line = _encoding.GetString(buffer);
                ValidateBuffer(buffer, file.FileName);
                var stringToRowResponse = _parser.ParseLine(line, index);

                if (!stringToRowResponse.Success)
                {
                    string message = $"Unable to retrieve budget record at ${index}. Message was {response.Message}";
                    _logger.LogError(message);
                    throw new ParseException(message);
                }

                // generates new row where only the editable columns are taken
                List<PTBColumn> uneditableColumns = stringToRowResponse.Row.Columns.Where(column => column.Editable == false).ToList();
                List<PTBColumn> editableColumns = row.Columns.Where(column => column.Editable == true).ToList();
                uneditableColumns.AddRange(editableColumns);
                PTBRow rowToUpdate = new PTBRow
                {
                    Index = index,
                    Columns = uneditableColumns
                };

                // Writes the new row with only editable columns changed to the file
                var rowToStringResponse = _parser.ParseRow(rowToUpdate);

                if (!rowToStringResponse.Success)
                {
                    string message = $"Unable to reconvert budget record for update. Message was {response.Message}";
                    _logger.LogError(message);
                    throw new ParseException(message);
                }

                byte[] bufferToUpdate = _encoding.GetBytes(rowToStringResponse.Line + Environment.NewLine);
                SetBufferStartIndex(stream, index);
                stream.Write(bufferToUpdate, 0, buffer.Length);
                stream.Flush();
            }

            return response;

        }
    }
}