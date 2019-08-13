using PTB.File.Ledger;
using PTB.File.TitleRegex;
using PTB.File.Categories;

namespace PTB.File
{
    public class PTBSchema
    {
        public LedgerSchema Ledger;
        public TitleRegexSchema TitleRegex;
        public CategoriesSchema Categories;
    }
}