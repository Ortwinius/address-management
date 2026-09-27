import { Component, inject } from '@angular/core'
import { MatButton } from '@angular/material/button'
import { MatIcon } from '@angular/material/icon'
import { AddressFields, AddressLabels } from '../addresses.models'
import { AddressStore } from '../address-store'
import { Skeleton } from '../../shared/skeleton'

@Component({
  imports: [MatButton, MatIcon, Skeleton],
  selector: 'app-addresses-detail',
  templateUrl: './addresses-detail.html',
})
export class AddressesDetail {
  protected readonly store = inject(AddressStore)
  protected readonly fields = AddressFields
  protected readonly labels = AddressLabels

  protected remove() {
    if (confirm('Delete this address?')) this.store.remove()
  }
}
