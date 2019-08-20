import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OpenIdConnectService } from '../open-id-connect.service';
import { environment } from 'src/environments/environment';



@Component({
  selector: 'app-signin-oidc',
  templateUrl: './signin-oidc.component.html',
  styleUrls: ['./signin-oidc.component.scss']
})
export class SigninOidcComponent implements OnInit {

  constructor(private openIdConnectService: OpenIdConnectService,
    private router: Router) { }

  ngOnInit() {
    this.openIdConnectService.userLoaded$.subscribe((userLoaded) => {
      if (userLoaded) {
        // Route to home page if user is loggin, otherwise log the exception
        this.router.navigate(['./']);
      } else {
        if (!environment.production) {
          console.log('An error happened: user wasn\'t loaded.');
        }
      }
    });

    this.openIdConnectService.handleCallback();
  }
}
