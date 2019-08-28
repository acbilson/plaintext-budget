import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LedgerTableComponent } from './ledger/ledger-table/ledger-table.component';

// pipes
import { PrependZerosPipe } from './custom-pipes/prepend-zeros.pipe';
import { TrimPipe } from './custom-pipes/trim.pipe';
import { DebitPipe } from './custom-pipes/debit.pipe';

// services
import { PtbService } from './services/ptb.service';
import { PtbTransformService } from './services/ptb-transform.service';
import { LoggingService } from './services/logging.service';
import { LedgerPageComponent } from './ledger/ledger-page.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LedgerPageComponent,
    LedgerTableComponent,
    PrependZerosPipe,
    TrimPipe,
    DebitPipe,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'ledger/:name', component: LedgerPageComponent },
      { path: 'ledger', component: LedgerPageComponent },
    ])
  ],
  providers: [PtbTransformService, LoggingService, PtbService],
  bootstrap: [AppComponent]
})
export class AppModule { }
