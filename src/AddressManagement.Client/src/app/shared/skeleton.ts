import { Component, computed, input } from '@angular/core'

// Pulsing placeholder lines while data is loading.
@Component({
  selector: 'app-skeleton',
  template: `
    @for (line of lines(); track line) {
      <div class="my-4 h-4 rounded bg-(--mat-sys-surface-container-high) animate-pulse"></div>
    }
  `,
})
export class Skeleton {
  readonly count = input(5)
  protected readonly lines = computed(() => Array.from({ length: this.count() }, (_, i) => i))
}
