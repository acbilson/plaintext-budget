using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.FolderAccess;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PTB.Core.Files
{
    public interface IPTBFileService
    {
        BaseReadResponse Read(BasePTBFile file, int index, int count);

        BaseUpdateResponse Update(BasePTBFile file, int index, PTBRow row);

        BaseAppendResponse Append(BasePTBFile file, PTBRow row);
    }

    public class BaseFileService : IPTBFileService
    {
        protected Encoding _encoding = Encoding.ASCII;
        protected FolderSchema _schema;
        protected BaseFileParser _parser;
        protected IPTBLogger _logger;
        protected FileValidation _validator;

        public BaseFileService(IPTBLogger logger, BaseFileParser parser, FolderSchema schema, FileValidation validator)
        {
            _schema = schema;
            _parser = parser;
            _logger = logger;
            _logger.SetContext(nameof(BaseFileService));
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
                .LineIndexExists(row.Index, _schema.LineSize, fileName)
                .Response;
        }

        protected bool IsFirstLine(int bytesRead, int bufferLength) => bytesRead == bufferLength;

        protected long GetLineNumber(long streamPosition, int lineSize) => streamPosition / (lineSize + System.Environment.NewLine.Length);

        protected byte[] GetBuffer() => new byte[_schema.LineSize + Environment.NewLine.Length];

        protected void SetBufferStartIndex(FileStream stream, int startIndex) => stream.Seek(startIndex, SeekOrigin.Begin);

        protected int GetFileLineCount(string path) => Convert.ToInt32(new System.IO.FileInfo(path).Length / (_schema.LineSize + Environment.NewLine.Length));

        public BaseReadResponse Read(BasePTBFile file, int index, int count)
        {
            var response = BaseReadResponse.Default;
            int randomLineCount = new Random().Next(1, count);

            var validationResponse = _validator
                .LineIndexExists(index, _schema.LineSize, file.FileName)
                .Response;

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

                    // log a random line for QC
                    if (count == randomLineCount)
                    {
                        _logger.LogDebug($"The {randomLineCount} line read is: {line}");
                    }

                    if (IsFirstLine(bytesRead, buffer.Length))
                    {
                        ValidateBuffer(buffer, file.FileName);
                    }

                    parseResponse = _parser.ParseLine(line, byteIndex);

                    if (!parseResponse.Success)
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.LineSize);
                        string message = $"Review file {file.FileName} for data corruption at line {lineNumber}. Message is: {parseResponse.Message}";
                        throw new ParseException(message);
                    }

                    response.ReadResult.Add(parseResponse.Row);
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

                // gets existing ledger prior to update
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string line = _encoding.GetString(buffer);
                ValidateBuffer(buffer, file.FileName);
                var stringToRowResponse = _parser.ParseLine(line, index);

                if (!stringToRowResponse.Success)
                {
                    string message = $"Unable to retrieve ledger at ${index}. Message was {response.Message}";
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
                    string message = $"Unable to reconvert ledger for update. Message was {response.Message}";
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

        public BaseAppendResponse Append(BasePTBFile file, PTBRow row)
        {
            throw new NotImplementedException();
        }
    }
}