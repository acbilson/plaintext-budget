import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LedgerComponent } from './ledger/ledger.component';

// pipes
import { PrependZerosPipe } from './custom-pipes/prepend-zeros.pipe';
import { TrimPipe } from './custom-pipes/trim.pipe';
import { DebitPipe } from './custom-pipes/debit.pipe';

// services
import { PtbService } from './ptb.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LedgerComponent,
    PrependZerosPipe,
    TrimPipe,
    DebitPipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'ledger', component: LedgerComponent },
    ])
  ],
  providers: [PtbService],
  bootstrap: [AppComponent]
})
export class AppModule { }
