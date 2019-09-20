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

        BaseAppendResponse Append(BasePTBFile file, PTBRow row);
    }

    public class BaseReportService : IPTBReportService
    {
        protected Encoding _encoding = Encoding.ASCII;
        protected IPTBLogger _logger;
        protected BaseReportParser _parser;
        protected FolderSchema _schema;

        public BaseReportService(IPTBLogger logger, BaseReportParser parser, FolderSchema schema)
        {
            _logger = logger;
            _schema = schema;
            _parser = parser;
        }

        protected void ValidateBuffer(byte[] buffer, string fileName)
        {
            if (HasByteOrderMark(buffer))
            {
                string message = $"The {fileName} ledger has a byte order mark added by a utf-8 compatible editor. Please remove from the text file to continue.";
                _logger.LogError(message);
                throw new ParseException(message);
            }

            if (HasUnixNewLine(buffer))
            {
                string message = $"The {fileName} ledger has Unix new lines instead of Windows new lines. Please convert to Windows new lines to continue.";
                _logger.LogError(message);
                throw new ParseException(message);
            }
        }

        protected void ValidateUpdateRow(PTBRow row, string fileName)
        {
            if (!IndexStartsAtCorrectByte(row.Index))
            {
                string message = $"The start index {row.Index} to update file {fileName} does not match the index of any line. It should be divisible by {_schema.LineSize}";
                _logger.LogError(message);
                throw new FileException(message);
            }

            if (!EveryValueMatchesColumnSize(row.Columns))
            {
                var mismatchedColumns = row.Columns
                    .Where(column => column.ColumnValue.Length != column.Size)
                    .Select(column => $"({column.ColumnName}, {column.ColumnValue})");
                string columnsString = string.Join(Environment.NewLine, mismatchedColumns);
                string message = $"The row at index {row.Index} has the following column size mismatches: {Environment.NewLine}{columnsString}";
                _logger.LogError(message);
                throw new FileException(message);
            }
        }

        protected bool EveryValueMatchesColumnSize(List<PTBColumn> columns)
        {
            return columns.TrueForAll(column => column.ColumnValue.Length == column.Size);
        }

        protected bool IsFirstLine(int bytesRead, int bufferLength) => bytesRead == bufferLength;

        // the byte order mark (byteID 239) is added by some utf-8 compatible text editors. Can remove using Vim by :set nobomb; wq
        private bool HasByteOrderMark(byte[] buffer) => buffer[0] == 239;

        // Windows new line has both byte 13 (\n) and byte 10 (\r). Unix only has byte 13 (\n), so it will not contain byte 10 (\r)
        // editing text files on a Unix-based can convert the line endings. Fix by running the unixtodos command
        private bool HasUnixNewLine(byte[] buffer) => buffer.Any((b) => b == 10) == false;

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

        public BaseAppendResponse Append(BasePTBFile file, PTBRow row)
        {
            throw new NotImplementedException();
        }

        public ReportReadResponse Read(BasePTBFile file, int index, int count)
        {
            var response = ReportReadResponse.Default;

            if (!IndexStartsAtCorrectByte(index))
            {
                string message = $"The start index {index} to read from file {file.FileName} does not match the index of any line. It should be divisible by {_schema.LineSize}";
                _logger.LogError(message);
                throw new FileException(message);
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
                        string message = $"Review file {file.FileName} for data corruption at line {lineNumber}. Message is: {parseResponse.Message}";
                        throw new ParseException(message);
                    }

                    if (parseResponse.Message != "Empty Line")
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
            throw new NotImplementedException();
        }
    }
}