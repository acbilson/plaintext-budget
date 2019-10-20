// angular core modules
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Route } from '@angular/router';

// app modules
import { LedgerModule } from './ledger/ledger.module';
import { BudgetModule } from './budget/budget.module';

// components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

// services
import { LoggingService } from './services/logging/logging.service';
import { SchemaService } from './services/schema/schema.service';
import { FolderService } from './services/folder/folder.service';
import { ServiceConfig } from './interfaces/service-config';
import { ConfigService } from './services/config/config.service';

// master routes
const routePaths: Route[] = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: '**', component: HomeComponent },
];


// master service config
const configService: ConfigService = {
  apiUrl: new URL('http://18.217.77.174')
};

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
    BudgetModule,

    // always set last
    RouterModule.forRoot(routePaths)
  ],
  providers: [
    LoggingService,
    SchemaService,
    FolderService,
    {provide: ConfigService, useValue: configService }
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
