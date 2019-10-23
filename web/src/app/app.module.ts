// angular core modules
import { APP_INITIALIZER } from '@angular/core';
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

// services
import { LoggingService } from 'app/services/logging/logging.service';
import { SchemaService } from 'app/services/schema/schema.service';
import { FolderService } from 'app/services/folder/folder.service';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { map, catchError } from 'rxjs/operators';
import { Observable, ObservableInput } from 'rxjs/Observable';

// master routes
const routePaths: Route[] = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: '**', component: HomeComponent }
];

/* works in theory, but not for me
function load(http: HttpClient, config: ConfigService): (() => Promise<boolean>) {
  return (): Promise<boolean> => {
    return new Promise<boolean>((resolve: (a: boolean) => void): void => {
      http.get<ServiceConfig>('assets/app-config.json').toPromise()
        .then( (cfg: ServiceConfig) => {
          config.config = cfg;
          resolve(true);
         } );
    });
  };
}
*/

@NgModule({
  declarations: [
    // master components
    AppComponent,
    NavMenuComponent,
    HomeComponent
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
  providers: [
    ConfigService,
    /* matches commented code block above
    {provide: APP_INITIALIZER,
    useFactory: load,
  deps: [HttpClient, ConfigService], multi: true},
    */
    LoggingService,
    SchemaService,
    FolderService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
