using PTB.Core.Categories;
using PTB.Core.Ledger;
using PTB.Core.TitleRegex;

namespace PTB.Core
{
    public class PTBSchema
    {
        public LedgerSchema Ledger;
        public TitleRegexSchema TitleRegex;
        public CategoriesSchema Categories;
    }
}