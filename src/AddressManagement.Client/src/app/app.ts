import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbar } from '@angular/material/toolbar';
import { MatIcon } from '@angular/material/icon';

@Component({
  imports: [RouterOutlet, MatToolbar, MatIcon],
  selector: 'app-root',
  host: { class: 'flex h-dvh flex-col' },
  templateUrl: './app.html',
})
export class App {}
