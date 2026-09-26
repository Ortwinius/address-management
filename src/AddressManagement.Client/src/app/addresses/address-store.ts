import { computed, inject, Injectable, linkedSignal, signal } from '@angular/core'
import { debounce, form } from '@angular/forms/signals'
import { Sort } from '@angular/material/sort'
import { finalize, Observable } from 'rxjs'
import { Toaster } from '../shared/toaster'
import { AddressApi } from './address-api'
import { AddressCreate } from './addresses.models'

export type PanelMode = 'closed' | 'view' | 'edit' | 'new'

type Filter = { street: string; location: string; countries: string[] }
type Query = Filter & { sort: Sort }
type Page = { index: number; size: number }

const DebounceTimeInMs = 300

@Injectable()
export class AddressStore {
  private readonly api = inject(AddressApi)
  private readonly toaster = inject(Toaster)

  // Bound to the filter inputs with [formField]. Free text is debounced, so typing does not fire a request per key.
  private readonly filter = signal<Filter>({ street: '', location: '', countries: [] })
  readonly filterForm = form(this.filter, (p) => {
    debounce(p.street, DebounceTimeInMs)
    debounce(p.location, DebounceTimeInMs)
  })
  readonly sort = signal<Sort>({ active: 'street', direction: 'asc' })
  private readonly query = computed<Query>(() => ({ ...this.filter(), sort: this.sort() }))

  // Any filter or sort change jumps back to the first page, the page size is kept.
  readonly page = linkedSignal<Query, Page>({
    source: this.query,
    computation: (_, previous) => ({ index: 0, size: previous?.value.size ?? 10 }),
  })

  readonly addresses = this.api.list(() => {
    const { street, location, countries, sort } = this.query()
    const { index, size } = this.page()
    return {
      street: street,
      location: location,
      countries: countries,
      sortCol: sort.active,
      desc: sort.direction === 'desc',
      page: index + 1,
      pageSize: size,
    }
  })

  readonly items = computed(() => (this.addresses.hasValue() ? this.addresses.value().items : []))
  readonly total = computed(() => (this.addresses.hasValue() ? this.addresses.value().total : 0))
  readonly totalCapped = computed(
    () => this.addresses.hasValue() && this.addresses.value().totalCapped,
  )

  private readonly countries = this.api.countries()
  readonly countryOptions = computed(() =>
    this.countries.hasValue() ? this.countries.value() : [],
  )

  readonly mode = signal<PanelMode>('closed')
  readonly selectedId = signal<number | null>(null)
  readonly detail = this.api.detail(this.selectedId)
  readonly selected = computed(() => (this.detail.hasValue() ? this.detail.value() : undefined))
  readonly busy = signal(false)

  select(id: number) {
    this.selectedId.set(id)
    this.mode.set('view')
  }

  create() {
    this.selectedId.set(null)
    this.mode.set('new')
  }

  edit() {
    this.mode.set('edit')
  }

  cancelEdit() {
    this.mode.set(this.selectedId() === null ? 'closed' : 'view')
  }

  close() {
    this.selectedId.set(null)
    this.mode.set('closed')
  }

  save(address: AddressCreate) {
    const id = this.selectedId()
    const request = id === null ? this.api.create(address) : this.api.update(id, address)

    this.run(request, (saved) => {
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

    this.run(this.api.delete(id), () => {
      this.toaster.success('Address deleted')
      this.addresses.reload()
      this.close()
    })
  }

  removeMany(ids: number[]) {
    this.run(this.api.deleteMany(ids), () => {
      this.toaster.success(`${ids.length} addresses deleted`)
      this.addresses.reload()
      const open = this.selectedId()
      if (open !== null && ids.includes(open)) this.close()
    })
  }

  private run<T>(request: Observable<T>, onSuccess: (response: T) => void) {
    this.busy.set(true)
    request
      .pipe(finalize(() => this.busy.set(false)))
      .subscribe({ next: onSuccess, error: () => {} })
  }
}
