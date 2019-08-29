import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Route } from '@angular/router';
import { LedgerPageComponent } from './ledger-page/ledger-page.component';

// ledger components
import { LedgerTableComponent } from './ledger-table/ledger-table.component';

// pipes
import { PrependZerosPipe } from '../custom-pipes/prepend-zeros.pipe';
import { TrimPipe } from '../custom-pipes/trim.pipe';
import { DebitPipe } from '../custom-pipes/debit.pipe';

const routePaths: Route[] = [
      { path: 'ledger/:name', component: LedgerPageComponent },
      { path: 'ledger', component: LedgerPageComponent },
];

@NgModule({
  declarations: [

    // ledger components
    LedgerPageComponent,
    LedgerTableComponent,

    // pipes
    PrependZerosPipe,
    TrimPipe,
    DebitPipe,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routePaths)
  ],
})

export class LedgerModule { }
