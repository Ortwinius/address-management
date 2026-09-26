// Follows the source signal once it has been quiet for `ms`. Must be called in an injection context.
import { inject, Signal, WritableSignal } from '@angular/core'
import { toObservable, toSignal } from '@angular/core/rxjs-interop'
import { debounceTime, finalize, Observable, tap } from 'rxjs'
import { Toaster } from './toaster'
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http'

export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const toaster = inject(Toaster)
  return next(req).pipe(
    tap({
      error: (err: HttpErrorResponse) => {
        if (err.status !== 401) toaster.error(err.error?.title ?? err.message)
      },
    }),
  )
}

export function send<T>(
  request: Observable<T>,
  busy: WritableSignal<boolean>,
  onSuccess: (response: T) => void,
) {
  busy.set(true)
  request.pipe(finalize(() => busy.set(false))).subscribe({ next: onSuccess, error: () => {} })
}

export function debouncedSignal<T>(source: Signal<T>, ms: number): Signal<T> {
  return toSignal(toObservable(source).pipe(debounceTime(ms)), { initialValue: source() })
}

export function withoutEmpty(
  params: Record<string, string | number | boolean | readonly (string | number)[]>,
) {
  return Object.fromEntries(
    Object.entries(params).filter(([_, v]) => v !== '' && !(Array.isArray(v) && v.length === 0)),
  )
}
