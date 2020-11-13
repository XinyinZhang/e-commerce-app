import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  // since we want to access userInformation in multiple parts of application
  // we need a observable
  // replay: record the previous 1 value and replay to new subscriber
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }
  // generate a http request to load the currentUser from API server
  // used by appComponent to persist our login
  // tslint:disable-next-line: typedef
  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }
    // token will be sent in the header section
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get(this.baseUrl + 'account', {headers}).pipe(
      map((user: IUser) => { // map returned object to IUser type
        if (user) {
          // we will get back a new token when we do: register/login/getCurrentUser action
          // so we need to update our token in local storage
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user); // update currentUser observable
        }
      })
    );

  }

  // a login method that will take some values received from html form
  // and send an http request to login
  // tslint:disable-next-line: typedef
  login(values: any) {
    return this.http.post(this.baseUrl + 'account/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token); // store token in local storage
          this.currentUserSource.next(user);
        }
      })
    );
  }

  // tslint:disable-next-line: typedef
  register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token); // store token in local storage
          this.currentUserSource.next(user);
        }
      })
    );
  }
  // tslint:disable-next-line: typedef
  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/'); // if logging out, also redirect them to home page
  }
  // check if email exist method: will be useful when register a user
  // tslint:disable-next-line: typedef
  checkEmailExists(email: string)
  {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }
}
