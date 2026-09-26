import {computed, inject, Injectable, linkedSignal, signal} from '@angular/core';
import {HttpClient, httpResource} from '@angular/common/http';
import {Sort} from '@angular/material/sort';
import {Toaster} from '../shared/toaster';
import {AddressCreate, AddressDetail, AddressListItem, Country, PagedResult} from './addresses.models';
import {debouncedSignal, send, withoutEmpty} from '../shared/helpers';

export type PanelMode = 'closed' | 'view' | 'edit' | 'new'

type Filter = { street: string; location: string; countries: string[]; sort: Sort }
type Page = { index: number; size: number }

const Url = '/api/addresses'
const DebounceTimeInMs = 300

@Injectable()
export class AddressStore {
  private readonly http = inject(HttpClient)
  private readonly toaster = inject(Toaster)

  readonly streetInput = signal('')
  readonly locationInput = signal('')
  readonly countriesInput = signal<string[]>([])
  readonly sort = signal<Sort>({ active: 'street', direction: 'asc' })

  // Only free text is debounced, so typing does not fire a request per key.
  private readonly street = debouncedSignal(this.streetInput, DebounceTimeInMs)
  private readonly location = debouncedSignal(this.locationInput, DebounceTimeInMs)

  private readonly filter = computed<Filter>(() => ({
    street: this.street(),
    location: this.location(),
    countries: this.countriesInput(),
    sort: this.sort(),
  }))

  // Any filter or sort change jumps back to the first page, the page size is kept.
  readonly page = linkedSignal<Filter, Page>({
    source: this.filter,
    computation: (_, previous) => ({ index: 0, size: previous?.value.size ?? 10 }),
  })

  readonly addresses = httpResource<PagedResult<AddressListItem>>(() => {
    const { street, location, countries, sort } = this.filter()
    const { index, size } = this.page()
    return {
      url: Url,
      params: withoutEmpty({
        street, location, countries,
        sortCol: sort.active, desc: sort.direction === 'desc',
        page: index + 1, pageSize: size,
      }),
    }
  })
  // value() throws while a resource is in error state, hence the hasValue() checks.
  readonly items = computed(() => this.addresses.hasValue() ? this.addresses.value().items : [])
  readonly total = computed(() => this.addresses.hasValue() ? this.addresses.value().total : 0)
  readonly totalCapped = computed(() => this.addresses.hasValue() && this.addresses.value().totalCapped)

  private readonly countries = httpResource<Country[]>(() => `${Url}/countries`)
  readonly countryOptions = computed(() => this.countries.hasValue() ? this.countries.value() : [])

  readonly mode = signal<PanelMode>('closed')
  readonly selectedId = signal<number | null>(null)
  readonly detail = httpResource<AddressDetail>(() => {
    const id = this.selectedId()
    return id === null ? undefined : `${Url}/${id}`
  })
  readonly selected = computed(() => this.detail.hasValue() ? this.detail.value() : undefined)
  readonly busy = signal(false)

  select(id: number) {
    this.selectedId.set(id)
    this.mode.set('view')
  }

  create() {
    this.selectedId.set(null)
    this.mode.set('new')
  }

  edit() { this.mode.set('edit') }

  cancelEdit() { this.mode.set(this.selectedId() === null ? 'closed' : 'view') }

  close() {
    this.selectedId.set(null)
    this.mode.set('closed')
  }

  save(address: AddressCreate) {
    const id = this.selectedId()
    const request = id === null
      ? this.http.post<AddressDetail>(Url, address)
      : this.http.put<AddressDetail>(`${Url}/${id}`, address)

    send(request, this.busy, saved => {
      this.toaster.success(id === null ? 'Address created' : 'Address saved')
      this.addresses.reload()
      this.countries.reload() // a new country may have been created
      this.selectedId.set(saved.id)
      this.detail.reload()
      this.mode.set('view')
    })
  }

  remove() {
    const id = this.selectedId()
    if (id === null) return

    send(this.http.delete(`${Url}/${id}`), this.busy, () => {
      this.toaster.success('Address deleted')
      this.addresses.reload()
      this.close()
    })
  }

  removeMany(ids: number[]) {
    send(this.http.delete(Url, { params: { ids } }), this.busy, () => {
      this.toaster.success(`${ids.length} addresses deleted`)
      this.addresses.reload()
      const open = this.selectedId()
      if (open !== null && ids.includes(open)) this.close()
    })
  }
}
