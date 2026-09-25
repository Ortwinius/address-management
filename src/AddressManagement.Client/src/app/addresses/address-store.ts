import {computed, inject, Injectable, linkedSignal, signal} from '@angular/core';
import {toObservable, toSignal} from '@angular/core/rxjs-interop';
import {HttpClient, httpResource} from '@angular/common/http';
import {debounceTime, finalize, Observable} from 'rxjs';
import {Sort} from '@angular/material/sort';
import {Toaster} from '../shared/toaster';
import {AddressCreate, AddressDetail, AddressListItem, Country, PagedResult} from './addresses.models';

export type PanelMode = 'closed' | 'view' | 'edit' | 'new'

type TextFilter = { street: string; location: string }
export type Filter = TextFilter & { countries: string[]; sort: Sort }
type Page = { index: number; size: number }

const Url = '/api/addresses'
const DebounceTimeInMs = 300

// State and requests of the address feature. Components render it and call its methods.
@Injectable()
export class AddressStore {
  private readonly http = inject(HttpClient)
  private readonly toaster = inject(Toaster)

  // --- List: filters, sorting and paging end up in one request ---

  readonly streetInput = signal('')
  readonly locationInput = signal('')
  readonly countriesInput = signal<string[]>([])
  readonly sort = signal<Sort>({ active: 'street', direction: 'asc' })

  // Only free text is debounced, so typing does not fire a request per key.
  private readonly text = toSignal(
    toObservable(computed<TextFilter>(() => ({ street: this.streetInput(), location: this.locationInput() })))
      .pipe(debounceTime(DebounceTimeInMs)),
    { initialValue: { street: '', location: '' }, equal: (a, b) => a.street === b.street && a.location === b.location })

  readonly filter = computed<Filter>(() => ({ ...this.text(), countries: this.countriesInput(), sort: this.sort() }))

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
  readonly items = computed(() => this.addresses.value()?.items ?? [])
  readonly total = computed(() => this.addresses.value()?.total ?? 0)

  private readonly countries = httpResource<Country[]>(() => `${Url}/countries`)
  readonly countryOptions = computed(() => this.countries.value() ?? [])

  // --- Side panel: the selected address and what the panel shows ---

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

    this.send(request, saved => {
      this.toaster.success(id === null ? 'Adresse angelegt' : 'Adresse gespeichert')
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

    this.send(this.http.delete(`${Url}/${id}`), () => {
      this.toaster.success('Adresse gelöscht')
      this.addresses.reload()
      this.close()
    })
  }

  // Failed requests are already toasted by httpErrorInterceptor, so the error is only swallowed here.
  private send<T>(request: Observable<T>, onSuccess: (response: T) => void) {
    this.busy.set(true)
    request
      .pipe(finalize(() => this.busy.set(false)))
      .subscribe({ next: onSuccess, error: () => {} })
  }
}

// Drops unset filters so the URL only carries what is actually filtered.
function withoutEmpty(params: Record<string, string | number | boolean | string[]>) {
  return Object.fromEntries(
    Object.entries(params).filter(([_, v]) => v !== '' && !(Array.isArray(v) && v.length === 0)))
}
