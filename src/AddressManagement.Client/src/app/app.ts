import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbar } from '@angular/material/toolbar';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { Auth } from './auth/auth';

@Component({
  imports: [RouterOutlet, MatToolbar, MatIconButton, MatIcon],
  selector: 'app-root',
  host: { class: 'flex h-dvh flex-col' },
  templateUrl: './app.html',
})
export class App {
  protected readonly auth = inject(Auth)
}
