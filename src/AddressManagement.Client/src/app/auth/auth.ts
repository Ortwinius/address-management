import {computed, inject, Injectable, signal} from '@angular/core';
import {CanActivateFn, Router} from '@angular/router';

// OAuth client ID from the Google Cloud Console. Public, not a secret; must match Google:ClientId of the API.
export const GoogleClientId = '1045809102500-sselr5h9ota3mkb0bshvl1h7putr57v4.apps.googleusercontent.com'

const TokenKey = 'auth_id_token'

// Holds the Google ID token that authenticates every API request.
@Injectable({ providedIn: 'root' })
export class Auth {
  private readonly router = inject(Router)

  readonly token = signal(sessionStorage.getItem(TokenKey))
  readonly isLoggedIn = computed(() => this.token() !== null)

  login(idToken: string) {
    sessionStorage.setItem(TokenKey, idToken)
    this.token.set(idToken)
    this.router.navigateByUrl('/')
  }

  logout() {
    sessionStorage.removeItem(TokenKey)
    this.token.set(null)
    google.accounts.id.disableAutoSelect()
    this.router.navigateByUrl('/login')
  }
}

export const authGuard: CanActivateFn = () => inject(Auth).isLoggedIn() || inject(Router).parseUrl('/login')
