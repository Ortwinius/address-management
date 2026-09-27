import { Component, computed, inject } from '@angular/core'
import { toSignal } from '@angular/core/rxjs-interop'
import { BreakpointObserver } from '@angular/cdk/layout'
import { map } from 'rxjs'
import { MatSidenav, MatSidenavContainer, MatSidenavContent } from '@angular/material/sidenav'
import { MatIconButton } from '@angular/material/button'
import { MatIcon } from '@angular/material/icon'
import { AddressStore, PanelMode } from '../address-store'
import { AddressesList } from '../addresses-list/addresses-list'
import { AddressesDetail } from '../addresses-detail/addresses-detail'
import { AddressesForm } from '../addresses-form/addresses-form'

const PhoneQuery = '(max-width: 767px)'

@Component({
  imports: [
    MatSidenavContainer,
    MatSidenav,
    MatSidenavContent,
    MatIconButton,
    MatIcon,
    AddressesList,
    AddressesDetail,
    AddressesForm,
  ],
  providers: [AddressStore],
  selector: 'app-addresses-page',
  host: { class: 'block flex-1 min-h-0' },
  templateUrl: './addresses-page.html',
})
export class AddressesPage {
  protected readonly store = inject(AddressStore)
  protected readonly panelOpen = computed(() => this.store.mode() !== 'closed')

  // Phones get the panel as an overlay instead of next to the table.
  protected readonly phone = toSignal(
    inject(BreakpointObserver)
      .observe(PhoneQuery)
      .pipe(map((state) => state.matches)),
    { requireSync: true },
  )

  protected readonly titles: Record<PanelMode, string> = {
    closed: '',
    view: 'Address',
    edit: 'Edit address',
    new: 'New address',
  }
}
