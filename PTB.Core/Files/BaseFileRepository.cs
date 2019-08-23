using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PTB.Core.Files
{
    public class BaseFileRepository : IPTBRepository
    {
        protected Encoding _encoding = Encoding.ASCII;
        protected FolderSchema _schema;
        protected BaseFileParser _parser;
        protected PTB.Core.Files.PTBFile _file;
        protected IPTBLogger _logger;

        public BaseFileRepository(IPTBLogger logger, BaseFileParser parser, FolderSchema schema, PTBFile file)
        {
            _logger = logger;
            _schema = schema;
            _parser = parser;
            _file = file;
        }

        protected bool HasByteOrderMark(byte[] buffer) => buffer[0] == 239;

        // Windows new line has both byte 13 (\n) and byte 10 (\r). Unix only has byte 13 (\n), so it will not contain byte 10 (\r)
        protected bool HasUnixNewLine(byte[] buffer) => buffer.Any((b) => b == 10) == false;

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

        public BaseReadResponse Read(int index, int count)
        {
            var response = BaseReadResponse.Default;

            if (!IndexStartsAtCorrectByte(index))
            {
                throw new Exception($"The start index {index} does not match the index of any line. It should be divisible by {_schema.LineSize}");
            }

            using (var stream = new FileStream(_file.FullPath, FileMode.Open, System.IO.FileAccess.Read))
            {
                int byteIndex = index;
                int bytesRead = 0;
                byte[] buffer = GetBuffer();
                SetBufferStartIndex(stream, index);

                var parseResponse = StringToRowResponse.Default;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0 && count > 0)
                {
                    string line = _encoding.GetString(buffer);
                    parseResponse = _parser.ParseLine(line, byteIndex);

                    if (!parseResponse.Success)
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.LineSize);
                        throw new ParseException($"Review the file for data corruption at line {lineNumber}. Message is: {parseResponse.Message}");
                    }

                    response.ReadResult.Add(parseResponse.Row);
                    count--;
                    byteIndex += bytesRead;
                }
            }

            return response;
        }

        public BaseUpdateResponse Update(int index, PTBRow row)
        {
            var response = BaseUpdateResponse.Default;

            if (!IndexStartsAtCorrectByte(index))
            {
                response.Success = false;
                response.Message = $"Could not update with these values. The start index {index} does not match the index of any line. It should be divisible by {_schema.LineSize}";
                return response;
            }

            var parseResponse = _parser.ParseRow(row);

            if (!parseResponse.Success)
            {
                response.Success = parseResponse.Success;
                response.Message = parseResponse.Message;
                return response;
            }

            using (var stream = new FileStream(_file.FullPath, FileMode.Open, System.IO.FileAccess.Write))
            {
                byte[] buffer = _encoding.GetBytes(parseResponse.Line);
                SetBufferStartIndex(stream, index);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }

            return response;
        }

        public BaseAppendResponse Append(PTBRow row)
        {
            throw new NotImplementedException();
        }
    }
}