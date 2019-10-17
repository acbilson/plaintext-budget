import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Route } from '@angular/router';
import { BudgetPageComponent } from './budget-page/budget-page.component';
import { BudgetFormComponent } from './budget-form/budget-form.component';

const routePaths: Route[] = [
  { path: 'budget/:name', component: BudgetPageComponent },
  { path: 'budget', component: BudgetPageComponent },
];

@NgModule({
  declarations: [

    // ledger components
    BudgetFormComponent,
    BudgetPageComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routePaths)
  ],
  providers: [
  ]
})

export class BudgetModule { }