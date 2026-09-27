import { Component, computed, inject, input, linkedSignal } from '@angular/core'
import { DecimalPipe } from '@angular/common'
import { AddressField, AddressLabels, AddressListItem } from '../addresses.models'
import { AddressStore } from '../address-store'
import { Skeleton } from '../../shared/skeleton'
import { MatPaginator } from '@angular/material/paginator'
import { MatFormField, MatInput, MatLabel } from '@angular/material/input'
import { MatOption, MatSelect } from '@angular/material/select'
import { MatSort, MatSortHeader } from '@angular/material/sort'
import { MatProgressBar } from '@angular/material/progress-bar'
import { MatButton } from '@angular/material/button'
import { MatIcon } from '@angular/material/icon'
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef,
  MatNoDataRow,
  MatRow,
  MatRowDef,
  MatTable,
} from '@angular/material/table'
import { SelectionModel } from '@angular/cdk/collections'
import { MatCheckbox } from '@angular/material/checkbox'
import { FormField } from '@angular/forms/signals'

const AllColumns: AddressField[] = ['street', 'zipCode', 'location', 'country', 'recipient']
const CompactColumns: AddressField[] = ['street', 'location', 'country']
const UnsortableColumns: AddressField[] = [] // could be added later

@Component({
  imports: [
    DecimalPipe,
    Skeleton,
    MatFormField,
    MatLabel,
    MatInput,
    MatSelect,
    MatOption,
    MatSort,
    MatSortHeader,
    MatProgressBar,
    MatButton,
    MatIcon,
    MatTable,
    MatColumnDef,
    MatHeaderRow,
    MatRow,
    MatCell,
    MatHeaderCell,
    MatNoDataRow,
    MatPaginator,
    MatHeaderRowDef,
    MatHeaderCellDef,
    MatCellDef,
    MatRowDef,
    MatCheckbox,
    FormField,
  ],
  selector: 'app-addresses-list',
  styleUrl: './addresses-list.css',
  templateUrl: './addresses-list.html',
})
export class AddressesList {
  protected readonly store = inject(AddressStore)

  // Narrow layout (open side panel or phone): fewer columns, filters and sorting keep working.
  readonly compact = input(false)

  protected readonly labels = AddressLabels
  protected readonly dataColumns = AllColumns
  protected readonly unsortableColumns = UnsortableColumns
  protected readonly columns = computed(() => [
    'select',
    ...(this.compact() ? CompactColumns : AllColumns),
  ])

  readonly initialRowSelection = []
  readonly allowMultiSelect = true
  // Tied to the rows on the current page: paging, filtering and reloading clear it.
  readonly selection = linkedSignal<AddressListItem[], SelectionModel<AddressListItem>>({
    source: this.store.items,
    computation: (_, previous) =>
      new SelectionModel<AddressListItem>(this.allowMultiSelect, this.initialRowSelection),
  })

  isAllSelected() {
    const numSelected = this.selection().selected.length
    const numRows = this.store.items().length
    return numRows > 0 && numSelected == numRows
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  toggleAllRows() {
    this.isAllSelected()
      ? this.selection().clear()
      : this.store.items().forEach((row) => this.selection().select(row))
  }

  protected removeSelected() {
    const ids = this.selection().selected.map((a) => a.id)
    if (confirm(`Delete ${ids.length} addresses?`)) this.store.removeMany(ids)
  }
}
