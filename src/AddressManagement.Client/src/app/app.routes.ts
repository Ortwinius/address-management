import { Routes } from '@angular/router';
export const routes: Routes = [
  { path: '', loadComponent: () => import('./addresses/addresses-page/addresses-page').then(m => m.AddressesPage) },
];
