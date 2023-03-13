import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../models/user';
import { UserAccount } from '../models/account/user-account.model';
import { RegisterUser } from '../models/account/register-user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  private refreshTokenTimeout: any;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser') || '{}')
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(userName: string, password: string) {
    return this.http.post<any>(`api/useraccount/authenticate`, { userName, password }, { withCredentials: true })
        .pipe(map(response => {
          if (response?.status){
            localStorage.setItem('currentUser', JSON.stringify(response?.entity));
            this.currentUserSubject.next(response?.entity);
            this.startRefreshTokenTimer();
            return response;
          } else {
            return throwError(()=> new Error(response?.message));
          }
        }));
}

logout() {
  this.http.post<any>(`api/useraccount/revoke-token`, {}, { withCredentials: true }).subscribe();
  localStorage.removeItem('currentUser');
  this.stopRefreshTokenTimer();
  this.currentUserSubject.next(JSON.parse(localStorage.getItem('currentUser') || '{}'));
  return of({ success: false });
}

refreshToken() {
  const userAccount = JSON.parse(localStorage.getItem('currentUser') || '{}');
  if (userAccount){
    return this.http.post<any>(`api/useraccount/refresh-token`, {}, { withCredentials: true })
    .pipe(map((account) => {
        this.currentUserSubject.next(account);
        this.startRefreshTokenTimer();
        return account;
    }));
  } else{ return userAccount; }

}

register(account: RegisterUser) {
    return this.http.post<any>(`api/useraccount/register`, account)
    .pipe(map(response => {
      if (response?.status){
        localStorage.setItem('currentUser', JSON.stringify(response?.entity));
        this.currentUserSubject.next(response?.entity);
        this.startRefreshTokenTimer();
      }
      return response;
    }));
}

verifyEmail(token: string) {
    return this.http.post<any>(`api/useraccount/verify-email`, { token }).pipe(map(response => {
      if (response?.status){
        return response;
      } else {
        return throwError(()=> new Error(response?.message));
      }
    }));
}

forgotPassword(email: string) {
    return this.http.post<any>(`api/useraccount/forgot-password`, { email }).pipe(map(response => {
      if (response?.status){
        return response;
      } else {
        return throwError(()=> new Error(response?.message));
      }
    }));
}

validateResetToken(token: string) {
    return this.http.post<any>(`api/useraccount/validate-reset-token`, { token });
}

resetPassword(token: string, password: string, confirmPassword: string) {
    return this.http.post<any>(`api/useraccount/reset-password`, { token, password, confirmPassword }).pipe(map(response => {
      if (response?.status){
        return response;
      } else {
        return throwError(()=> new Error(response?.message));
      }
    }));
}

getAll() {
    return this.http.get<UserAccount[]>(`api/useraccount/`);
}

getById(id: string) {
    return this.http.get<UserAccount>(`api/useraccount/${id}`);
}

private startRefreshTokenTimer() {
  // parse json object from base64 encoded jwt token
  const jwtToken = JSON.parse(atob(this.currentUserValue.token.split('.')[1]));

  // set a timeout to refresh the token a minute before it expires
  const expires = new Date(jwtToken.exp * 1000);
  const timeout = expires.getTime() - Date.now() - (60 * 1000);
  this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
}

private stopRefreshTokenTimer() {
  clearTimeout(this.refreshTokenTimeout);
}

}
