// angular core modules
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Route } from '@angular/router';

// app modules
import { LedgerModule } from './ledger/ledger.module';

// components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

// services
import { PtbService } from './services/ptb.service';
import { PtbTransformService } from './services/ptb-transform.service';
import { LoggingService } from './services/logging.service';

// master routes
const routePaths: Route[] = [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: '**', component: HomeComponent },
];

@NgModule({
  declarations: [

    // master components
    AppComponent,
    NavMenuComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,

    // custom app modules
    LedgerModule,

    // always set last
    RouterModule.forRoot(routePaths)
  ],
  providers: [
    PtbTransformService,
    LoggingService,
    PtbService
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
