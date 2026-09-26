import {afterNextRender, Component, ElementRef, inject, viewChild} from '@angular/core';
import {Auth, GoogleClientId} from '../auth';

@Component({
  selector: 'app-login',
  host: { class: 'flex flex-1 items-center justify-center p-4' },
  template: `
    <div class="flex flex-col items-center gap-6 rounded-2xl bg-(--mat-sys-surface-container) p-10">
      <h1 class="text-2xl font-medium tracking-tight">Sign in to Addresso</h1>
      <div #googleButton></div>
    </div>
  `,
})
export class Login {
  private readonly auth = inject(Auth)
  private readonly googleButton = viewChild.required<ElementRef<HTMLElement>>('googleButton')

  constructor() {
    // Google renders its own button into the element once the view exists.
    afterNextRender(() => {
      google.accounts.id.initialize({
        client_id: GoogleClientId,
        callback: response => this.auth.login(response.credential),
      })
      google.accounts.id.renderButton(this.googleButton().nativeElement, { type: 'standard', theme: 'outline', size: 'large', locale: 'en' })
    })
  }
}
