import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Route } from '@angular/router';
import { LedgerPageComponent } from 'app/ledger/ledger-page/ledger-page.component';
import { LedgerTableComponent } from 'app/ledger/ledger-page/ledger-table/ledger-table.component';

// pipes
import { PrependZerosPipe } from 'app/custom-pipes/prepend-zeros.pipe';
import { TrimPipe } from 'app/custom-pipes/trim.pipe';
import { DebitPipe } from 'app/custom-pipes/debit.pipe';
import { ServiceConfig } from 'app/interfaces/service-config';

// services
import { LedgerService } from 'app/services/ledger/ledger.service';
import { TransformService } from 'app/services/transform/transform.service';

const routePaths: Route[] = [
  { path: 'ledger/:name', component: LedgerPageComponent },
  { path: 'ledger', component: LedgerPageComponent }
];

@NgModule({
  declarations: [
    // ledger components
    LedgerTableComponent,
    LedgerPageComponent,

    // pipes
    PrependZerosPipe,
    TrimPipe,
    DebitPipe
  ],
  imports: [CommonModule, RouterModule.forChild(routePaths)],
  providers: [TransformService, LedgerService]
})
export class LedgerModule {}
