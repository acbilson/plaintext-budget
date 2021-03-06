﻿using PTB.Core.Base;
using PTB.Core.Logging;
using System.Linq;
using System.Text;

namespace PTB.Core.Files
{
    public class BaseFileParser
    {
        private FolderSchema _schema;
        private IPTBLogger _logger;
        private FileValidation _validator;

        public BaseFileParser(FolderSchema schema, IPTBLogger logger, FileValidation validator)
        {
            _schema = schema;
            _logger = logger;
            logger.SetContext(nameof(BaseFileParser));
            _validator = validator;
        }

        protected string CalculateByteIndex(int delimiterLength, string line, ColumnSchema column)
        {
            int start = column.Offset + (delimiterLength * (column.Index - 1));
            return line.Substring(start, column.Size);
        }

        public RowToStringResponse ParseRow(PTBRow row)
        {
            var response = RowToStringResponse.Default;

            var builder = new StringBuilder();
            var columnValues = row.Columns
                .OrderBy(column => column.Index)
                .Select(column => column.ColumnValue);
            builder.AppendJoin(_schema.Delimiter, columnValues);

            response.Line = builder.ToString();
            return response;
        }

        public StringToRowResponse ParseLine(string line, int index)
        {
            var response = StringToRowResponse.Default;

            var validationResponse = _validator
                .LineEndsWithNewLine(line)
                .LineSizeMatchesSchema(line, _schema.LineSize)
                .Response;

            if (!validationResponse.Success)
            {
                response.Success = validationResponse.Success;
                response.Message = validationResponse.Message;
                return response;
            }

            foreach (ColumnSchema columnSchema in _schema.Columns)
            {
                var column = new PTBColumn(columnSchema);
                column.ColumnValue = CalculateByteIndex(_schema.Delimiter.Length, line, column);
                response.Row.Columns.Add(column);
            }

            response.Row.Index = index;

            return response;
        }
    }
}