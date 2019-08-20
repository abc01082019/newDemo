import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { Routes, RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { RequireAuthenticatedUserRouteGuard } from './shared/oidc/require-authenticated-user-route.guard';
import { RedirectSilentRenewComponent } from './shared/oidc/redirect-silent-renew/redirect-silent-renew.component';
import { SigninOidcComponent } from './shared/oidc/signin-oidc/signin-oidc.component';
import { OpenIdConnectService } from './shared/oidc/open-id-connect.service';

const routes: Routes = [
  // routing to the blog/blog.module.ts file's BlogModule module
  { path: "blog", loadChildren: './blog/blog.module#BlogModule'},
  // authentication check
  { path: 'signin-oidc', component: SigninOidcComponent },
  { path: 'redirect-silentrenew', component: RedirectSilentRenewComponent },
  // otherwi goes to blog router
  { path: "**", redirectTo: 'blog'}
];

@NgModule({
  declarations: [
    AppComponent,
    SigninOidcComponent,
    RedirectSilentRenewComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes),
    HttpClientModule
  ],
  providers: [
    OpenIdConnectService,
    RequireAuthenticatedUserRouteGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
