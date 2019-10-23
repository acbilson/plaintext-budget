import { BudgetColumn } from 'app/interfaces/budget-column';
import { BudgetEntryType } from 'app/interfaces/budget-entry-type';

export interface BudgetEntry {
  id: number;
  type: BudgetEntryType;
  index: BudgetColumn;
  category: BudgetColumn;
  subcategory: BudgetColumn;
  amount: BudgetColumn;
}
