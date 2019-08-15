using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using PTB.Core.Ledger;
using PTB.Core.TitleRegex;

namespace PTB.Core.Tests
{
    [TestClass]
    public class GlobalSetup
    {
        public PTBSchema Schema;

        [TestInitialize]
        public void Initialize()
        {
            Schema = GetDefaultSchema();
        }

        public PTBSchema GetDefaultSchema()
        {
            var schema = new PTBSchema
            {
                Ledger = new LedgerSchema
                {
                    Columns = new LedgerColumns
                    {
                        Date = new ColumnDate { Index = 1, Size = 10, Offset = 0 },
                        Type = new ColumnType { Index = 2, Size = 1, Offset = 10 },
                        Amount = new ColumnAmount { Index = 3, Size = 12, Offset = 11 },
                        Subcategory = new Ledger.ColumnSubcategory { Index = 4, Size = 20, Offset = 23 },
                        Title = new ColumnTitle { Index = 5, Size = 50, Offset = 43 },
                        Location = new ColumnLocation { Index = 6, Size = 15, Offset = 93 },
                        Locked = new ColumnLocked { Index = 7, Size = 1, Offset = 108 }
                    },
                    Delimiter = " ",
                    Files = new PTBFiles[] {
                        new PTBFiles { IsDefault = true, Name = "ledger_checking_19-01-01_19-12-31" }
                    },
                    Size = 115
                },
                TitleRegex = new TitleRegexSchema
                {
                    Columns = new TitleRegexColumns
                    {
                        Priority = new ColumnPriority { Index = 1, Size = 1, Offset = 0 },
                        Subcategory = new TitleRegex.ColumnSubcategory { Index = 2, Size = 30, Offset = 1 },
                        Regex = new ColumnRegex { Index = 3, Size = 30, Offset = 31 }
                    },
                    Delimiter = " ",
                    Files = new PTBFiles[] {
                        new PTBFiles { IsDefault = true, Name = "regex-title" }
                    },
                    Size = 63
                }
            };

            return schema;
        }
    }
}