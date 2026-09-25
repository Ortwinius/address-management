import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {tap} from 'rxjs';
import {Toaster} from './toaster';

// Every failed request becomes an error toast, so callers need no error handling of their own.
export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const toaster = inject(Toaster)
  return next(req).pipe(tap({ error: (err: HttpErrorResponse) => toaster.error(describe(err)) }))
}

// Prefers the first validation message, then the ProblemDetails title sent by ASP.NET Core.
function describe(err: HttpErrorResponse): string {
  if (err.status === 0) return 'Server nicht erreichbar'
  const problem = err.error as { title?: string; errors?: Record<string, string[]> } | null
  const validation = problem?.errors ? Object.values(problem.errors).flat()[0] : undefined
  return validation ?? problem?.title ?? `Fehler ${err.status}`
}
