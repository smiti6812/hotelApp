import { Component } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private jwtHelper: JwtHelperService, private router:Router, public translate: TranslateService){
    translate.addLangs(['en', 'hu']);
    translate.setDefaultLang('en');
  }
  switchLang(lang: string) {
    this.translate.use(lang);
 }
 isUserAuthenticated = (): boolean => {
  const token = localStorage.getItem("jwt");
  if (token && !this.jwtHelper.isTokenExpired(token)){
    return true;
  }
  return false;
}

logOut = () => {
  localStorage.removeItem("jwt");
  this.router.navigate(["login"]);
}


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
