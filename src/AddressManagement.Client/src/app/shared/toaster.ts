import { inject, Injectable } from '@angular/core'
import { MatSnackBar } from '@angular/material/snack-bar'

// Single place for user feedback, backed by the Material snack bar.
@Injectable({ providedIn: 'root' })
export class Toaster {
  private readonly snackBar = inject(MatSnackBar)

  success(message: string) {
    this.snackBar.open(message, undefined, { duration: 3000 })
  }

  error(message: string) {
    this.snackBar.open(message, 'OK', { duration: 8000, panelClass: 'toast-error' })
  }
}
