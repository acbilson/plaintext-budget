using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using PTB.Core.Exceptions;
using PTB.Core.Logging;

namespace PTB.Core.Categories
{
    public class CategoriesRepository : BaseFileRepository
    {
        private string _folder = "Categories";
        private CategoriesParser _parser;
        private IPTBLogger _logger;
        private Encoding _encoding = Encoding.ASCII;

        public CategoriesRepository(FileManager fileManager, IPTBLogger logger) : base(fileManager)
        {
            _parser = new CategoriesParser(_schema.Categories);
            _logger = logger;
            _logger.SetContext("CategoriesRepo");
        }

        public CategoriesReadResponse ReadAllDefaultCategories()
        {
            var response = CategoriesReadResponse.Default;

            string categoriesPath = _fileManager.GetDefaultCategoriesFilePath();

            using (var stream = new FileStream(categoriesPath, FileMode.Open, FileAccess.Read))
            {
                int bufferLength = _schema.Categories.LineSize + System.Environment.NewLine.Length;
                int lineIndex = _schema.Categories.LineSize - 1;
                var buffer = new byte[bufferLength];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string line = _encoding.GetString(buffer);

                    // the byte order mark (byteID 239) is added by some utf-8 compatible text editors. Can remove using Vim by :set nobomb; wq
                    if (HasByteOrderMark(buffer))
                    {
                        _logger.LogError($"Found a byte order mark in string: {line}");
                        throw new ParseException("The categories file has a byte order mark added by a utf-8 compatible editor. Please remove from the text file to continue.");
                    }

                    // editing text files on a Unix-based can convert the line endings. Fix by running the unixtodos command
                    if (HasUnixNewLine(buffer))
                    {
                        _logger.LogError($"Classified string as missing byte 13 (windows-only newline): {line}");
                        throw new ParseException("The categories file has Unix new lines instead of Windows new lines. Please convert to windows new lines to continue");
                    }

                    StringToCategoriesResponse current = _parser.ParseLine(line);

                    if (current.Success)
                    {
                        response.Categories.Add(current.Result);
                    }
                    else
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.Categories.LineSize);
                        string message = $"Skipped categories at line {lineNumber}. Message was {current.Message}";
                        response.SkippedMessages.Add(message);
                        _logger.LogWarning($"Skipped category from string: {line}. Reason: {current.Message}");
                    }
                }
            }
            return response;
        }
    }
}