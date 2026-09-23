import { HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { tap } from 'rxjs';

// Single place for request feedback. Swap console for a toast service later.
export const httpLogInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    tap({
      next: event => {
        if (event instanceof HttpResponse) {
          console.info(`${req.method} ${req.urlWithParams} -> ${event.status}`, event.body);
        }
      },
      error: (err: HttpErrorResponse) =>
        console.error(`${req.method} ${req.urlWithParams} -> ${err.status} ${err.message}`, err.error),
    }),
  );
