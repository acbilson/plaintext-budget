using PTB.File.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PTB.File.TitleRegex
{
    public class TitleRegexRepository : BaseFileRepository
    {
        private string _folder = "Regex";
        private TitleRegexParser _parser;
        private Encoding _encoding = Encoding.ASCII;

        public TitleRegexRepository(PTBSettings settings, PTBSchema schema) : base(settings, schema)
        {
            _parser = new TitleRegexParser(_schema.TitleRegex);
        }

        public TitleRegexReadResponse ReadAllTitleRegex()
        {
            var response = TitleRegexReadResponse.Default;
            string path = base.GetDefaultPath(_folder, _schema.TitleRegex.GetDefaultName());

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                int bufferLength = _schema.TitleRegex.Size + Environment.NewLine.Length;
                int lineIndex = _schema.TitleRegex.Size - 1;
                var buffer = new byte[bufferLength];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string line = _encoding.GetString(buffer);

                    // the byte order mark (byteID 239) is added by some utf-8 compatible text editors. Can remove using Vim by :set nobomb; wq
                    if (HasByteOrderMark(buffer))
                    {
                        throw new ParseException("The title regex file has a byte order mark added by a utf-8 compatible editor. Please remove from the text file to continue.");
                    }

                    // editing text files on a Unix-based can convert the line endings. Fix by running the unixtodos command
                    if (HasUnixNewLine(buffer))
                    {
                        throw new ParseException("The title regex file has Unix new lines instead of Windows new lines. Please convert to windows new lines to continue");
                    }

                    StringToTitleRegexResponse current = _parser.ParseLine(line);

                    if (current.Success)
                    {
                        response.TitleRegices.Add(current.Result);
                    }
                    else
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.TitleRegex.Size);
                        string message = $"Skipped title regex at line {lineNumber}. Message was {current.Message}";
                        response.SkippedMessages.Add(message);
                    }
                }
            }
            return response;
        }
    }
}