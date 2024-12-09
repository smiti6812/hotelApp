import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { LoginModel } from '../interfaces/LoginModel'
import {AuthenticatedResponse} from '../interfaces/AuthenticatedResponse'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
 invalidLogin: boolean;
 credentials: LoginModel = {userName:'', password:''};

 constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
  }

  login = ( form: NgForm) => {
      if(form.valid){
          this.http.post<AuthenticatedResponse>("https://localhost:7246/auth/login", this.credentials, {
            headers: new HttpHeaders({"Content-Type": "application/json"})
          }).subscribe({
            next: (response: AuthenticatedResponse) => {
              const token = response.token;
              const refreshToken = response.refreshToken;
              localStorage.setItem("jwt", token);
              localStorage.setItem("refreshtoken", refreshToken)
              this.invalidLogin = false;
              this.router.navigate(["/"]);
          },
          error: (err: HttpErrorResponse) => this.invalidLogin = true
      })
      }

  }

}
