import { Routes } from '@angular/router'
import { authGuard } from './auth/auth'

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./auth/login/login').then((m) => m.Login) },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./addresses/addresses-page/addresses-page').then((m) => m.AddressesPage),
  },
  { path: '**', redirectTo: '' },
]
