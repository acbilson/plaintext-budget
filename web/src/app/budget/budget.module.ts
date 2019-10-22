import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Route } from '@angular/router';
import { BudgetPageComponent } from 'app/budget/budget-page/budget-page.component';
import { BudgetFormComponent } from 'app/budget/budget-page/budget-form/budget-form.component';

import { BudgetService } from 'app/services/budget/budget.service';

const routePaths: Route[] = [
  { path: 'budget/:name', component: BudgetPageComponent },
  { path: 'budget', component: BudgetPageComponent }
];

@NgModule({
  declarations: [
    // ledger components
    BudgetFormComponent,
    BudgetPageComponent
  ],
  imports: [CommonModule, RouterModule.forChild(routePaths)],
  providers: [BudgetService]
})
export class BudgetModule {}
