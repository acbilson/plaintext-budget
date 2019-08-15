using PTB.File.Exceptions;
using PTB.File.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace PTB.File.Categories
{
    public class CategoriesRepository : BaseFileRepository
    {
        private string _Folder = "Categories";
        private CategoriesParser _parser;
        private Encoding _encoding = Encoding.ASCII;

        public CategoriesRepository(PTBSettings settings, PTBSchema schema) : base(settings, schema)
        {
            _parser = new CategoriesParser(_schema.Categories);
        }
    }
}