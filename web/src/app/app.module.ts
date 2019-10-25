// angular core modules
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule, Route } from '@angular/router';

// app modules
import { LedgerModule } from 'app/ledger/ledger.module';
import { BudgetModule } from 'app/budget/budget.module';

// components
import { AppComponent } from 'app/app.component';
import { NavMenuComponent } from 'app/nav-menu/nav-menu.component';
import { HomeComponent } from 'app/home/home.component';
import { HealthComponent } from 'app/health/health.component';

// services
import { LoggingService } from 'app/services/logging/logging.service';
import { SchemaService } from 'app/services/schema/schema.service';
import { FolderService } from 'app/services/folder/folder.service';
import { ConfigService } from 'app/services/config/config.service';

// master routes
const routePaths: Route[] = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {path: 'health', component: HealthComponent, pathMatch: 'full' },
  { path: '**', component: HomeComponent }
];

@NgModule({
  declarations: [
    // master components
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HealthComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,

    // custom app modules
    LedgerModule,
    BudgetModule,

    // always set last
    RouterModule.forRoot(routePaths)
  ],
  providers: [ConfigService, LoggingService, SchemaService, FolderService],
  bootstrap: [AppComponent]
})
export class AppModule {}
