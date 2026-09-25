import {Component, computed, linkedSignal, output, signal} from '@angular/core';
import {toObservable, toSignal} from '@angular/core/rxjs-interop';
import {httpResource} from '@angular/common/http';
import {debounceTime} from 'rxjs';
import {AddressListItem, Country, PagedResult} from '../addresses.models';
import {MatPaginator} from '@angular/material/paginator';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {MatOption, MatSelect} from '@angular/material/select';
import {MatSort, MatSortHeader, Sort} from '@angular/material/sort';
import {MatProgressBar} from '@angular/material/progress-bar';
import {
  MatCell, MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef,
  MatNoDataRow,
  MatRow, MatRowDef,
  MatTable
} from '@angular/material/table';
import {SelectionModel} from '@angular/cdk/collections';
import {MatCheckbox} from '@angular/material/checkbox';

type TextFilter = { street: string; location: string }
type Filter = TextFilter & { countries: string[]; sort: Sort }
type Page = { index: number; size: number }

const DebounceTimeInMs = 300;
@Component({
  imports: [
    MatFormField,
    MatLabel,
    MatInput,
    MatSelect,
    MatOption,
    MatSort,
    MatSortHeader,
    MatProgressBar,
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
    MatCheckbox
  ],
  selector: 'app-addresses-list',
  templateUrl: './addresses-list.html',
})
export class AddressesList {
  readonly selected = output<number>();

  protected readonly columnDefs = [
    { id: 'street', label: 'Straße' },
    { id: 'zipCode', label: 'PLZ' },
    { id: 'location', label: 'Ort' },
    { id: 'country', label: 'Land' },
  ] as const
  protected readonly columns = ['select', ...this.columnDefs.map(c => c.id)]

  protected readonly streetInput = signal('')
  protected readonly locationInput = signal('')
  protected readonly countriesInput = signal<string[]>([])
  protected readonly sort = signal<Sort>({ active: 'street', direction: 'asc' })

  // Only free text is debounced, so typing does not fire a request per key.
  private readonly text = toSignal(
    toObservable(computed<TextFilter>(() => ({ street: this.streetInput(), location: this.locationInput() })))
      .pipe(debounceTime(DebounceTimeInMs)),
    { initialValue: { street: '', location: '' }, equal: (a, b) => a.street === b.street && a.location === b.location })

  private readonly filter = computed<Filter>(() => ({ ...this.text(), countries: this.countriesInput(), sort: this.sort()}))

  // Any filter or sort change jumps back to the first page, the page size is kept.
  protected readonly page = linkedSignal<Filter, Page>({
    source: this.filter,
    computation: (_, previous) => ({ index: 0, size: previous?.value.size ?? 10 }),
  })

  protected readonly addresses = httpResource<PagedResult<AddressListItem>>(() => {
    const { street, location, countries, sort } = this.filter()
    const { index, size } = this.page()
    return {
      url: '/api/addresses',
      params: withoutEmpty({
        street, location, countries,
        sortCol: sort.active, desc: sort.direction === 'desc',
        page: index + 1, pageSize: size,
      }),
    }
  })
  protected readonly items = computed(() => this.addresses.hasValue() ? this.addresses.value().items : [])
  protected readonly total = computed(() => this.addresses.hasValue() ? this.addresses.value().total : 0)

  private readonly countries = httpResource<Country[]>(() => '/api/addresses/countries')
  protected readonly uniqueCountryOptions = computed(() => this.countries.hasValue() ? this.countries.value() : [])

  reload() { this.addresses.reload() }

  readonly initialRowSelection = [];
  readonly allowMultiSelect = true;
  readonly selection = linkedSignal<Filter,SelectionModel<AddressListItem>>({
    source:this.filter,
    computation: (_, previous) => (new SelectionModel<AddressListItem>(this.allowMultiSelect, this.initialRowSelection)),
  })

  isAllSelected() {
    const numSelected = this.selection().selected.length;
    const numRows = this.items().length;
    return numRows > 0 && numSelected == numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  toggleAllRows() {
    this.isAllSelected() ?
      this.selection().clear() :
      this.addresses.value()?.items?.forEach(row => this.selection().select(row));
  }
}

// Drops unset filters so the URL only carries what is actually filtered.
function withoutEmpty(params: Record<string, string | number | boolean | string[]>) {
  return Object.fromEntries(
    Object.entries(params).filter(([_, v]) => v !== '' && !(Array.isArray(v) && v.length === 0)))
}

