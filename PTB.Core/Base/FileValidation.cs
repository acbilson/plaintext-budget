using PTB.Core.Exceptions;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.Base
{
    public class FileValidation
    {
        private BaseResponse _response;
        private IPTBLogger _logger;

        public BaseResponse Response
        {
            get
            {
                // resets response after each retrieval to allow for singleton functionality
                var firstResponse = _response;
                _response = BaseResponse.Default;
                return firstResponse;
            }
        }

        public FileValidation(IPTBLogger logger)
        {
            _logger = logger;
            _response = BaseResponse.Default;
        }

        private enum Severity
        {
            Warning,
            Error
        }

        private FileValidation Validate(Func<bool> validateFunc, Func<string> messageFunc, Severity severity)
        {
            return this.Validate(validateFunc, messageFunc.Invoke(), severity);
        }

        // validated == true means that there was an issue with validation
        // validated == false means the validation succeeded
        private FileValidation Validate(Func<bool> validateFunc, string message, Severity severity)
        {
            if (!_response.Success) return this;
            bool validated = validateFunc.Invoke();
            if (validated)
            {
                if (severity == Severity.Error)
                {
                    var error = new ParseException(message);
                    _logger.LogError(error);
                    throw error;
                }
                _response.Success = false;
                _response.Message = message;
                _logger.LogWarning(message);
            }

            return this;
        }

        #region Line Validation

        public FileValidation LineEndsWithNewLine(string line)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return Validate(
                    () => line.IndexOf(System.Environment.NewLine) != (line.Length - System.Environment.NewLine.Length),
                    ParseMessages.LINE_NO_CR,
                    Severity.Warning);
            }
            return this;
        }

        public FileValidation LineSizeMatchesSchema(string line, int schemaSize)
        {
            return Validate(
                () => line.Length != (schemaSize + System.Environment.NewLine.Length),
                ParseMessages.LINE_LENGTH_MISMATCH_SCHEMA,
                Severity.Warning);
        }

        public FileValidation LineIndexExists(int index, int lineSize, string fileName)
        {
            return Validate(() =>
            {
                if (index == 0) return false;
                if (index > 0)
                {
                    // subtracts the first byte of the line to the start Index (e.g. a 117 byte line will start the next line on 118)
                    return (index % (lineSize + Environment.NewLine.Length)) != 0;
                }
                return false;
            }, string.Format(ParseMessages.LINE_INDEX_MISSING, index, fileName, lineSize),
            Severity.Warning);
        }

        public FileValidation LineValuesMatchColumnSize(List<PTBColumn> columns, int index)
        {
            return Validate(
                () => columns.Any(column => column.ColumnValue.Length != column.Size),
                () =>
                {
                    var mismatchedColumns = columns
                                        .Where(column => column.ColumnValue.Length != column.Size)
                                        .Select(column => $"({column.ColumnName}, {column.ColumnValue})");
                    string columnsString = string.Join(Environment.NewLine, mismatchedColumns);
                    string message = string.Format(ParseMessages.LINE_COLUMN_MISMATCH, index, Environment.NewLine, columnsString);
                    return message;
                },
                Severity.Error);
        }

        #endregion Line Validation

        #region Buffer Validation

        // the byte order mark (byteID 239) is added by some utf-8 compatible text editors. Can remove using Vim by :set nobomb; wq
        public FileValidation BufferHasByteOrderMark(byte[] buffer, string fileName)
        {
            return Validate(
                () => buffer[0] == 239,
                string.Format(ParseMessages.LINE_BOM, fileName),
                Severity.Error);
        }

        public FileValidation BufferHasNewLine(byte[] buffer, string fileName)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return Validate(
                    () => buffer.Any((b) => b == 10) == false,
                    string.Format(ParseMessages.LINE_UNIX_NEWLINE, fileName),
                    Severity.Error);
            }
            else
            {
                return this;
            }
        }

        #endregion Buffer Validation
    }
}