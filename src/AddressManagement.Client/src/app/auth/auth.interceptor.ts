import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {tap} from 'rxjs';
import {Auth} from './auth';

// Sends the ID token with every request. A 401 means it expired (after 1h), so the user signs in again.
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(Auth)
  const token = auth.token()
  const request = token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req

  return next(request).pipe(tap({
    error: (err: HttpErrorResponse) => {
      if (err.status === 401) auth.logout()
    },
  }))
}
