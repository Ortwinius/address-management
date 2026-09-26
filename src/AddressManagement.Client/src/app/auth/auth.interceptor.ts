import { HttpInterceptorFn } from '@angular/common/http'
import { inject } from '@angular/core'
import { Auth } from './auth'

// Sends the Google ID token with every request. An expired token (401) is handled by errorInterceptor.
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(Auth).token()
  return next(token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req)
}
