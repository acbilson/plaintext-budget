using PTB.File.Categories;
using PTB.File.Ledger;
using PTB.File.TitleRegex;

namespace PTB.File
{
    public class PTBSchema
    {
        public LedgerSchema Ledger;
        public TitleRegexSchema TitleRegex;
        public CategoriesSchema Categories;
    }
}