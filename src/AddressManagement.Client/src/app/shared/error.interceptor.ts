import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http'
import { inject } from '@angular/core'
import { tap } from 'rxjs'
import { Auth } from '../auth/auth'
import { Toaster } from './toaster'

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(Auth)
  const toaster = inject(Toaster)

  return next(req).pipe(
    tap({
      error: (err: HttpErrorResponse) => {
        if (err.status === 401) {
          // The Google ID token expires after 1h, so the user signs in again.
          toaster.error('Please sign in again')
          auth.logout()
        } else {
          toaster.error(errorMessage(err))
        }
      },
    }),
  )
}

function errorMessage(err: HttpErrorResponse): string {
  if (err.status === 0) return 'The server is not reachable.'
  const problem = err.error
  if (problem?.errors) return Object.values<string[]>(problem.errors).flat().join(' ')
  return problem?.detail ?? problem?.title ?? err.message
}
