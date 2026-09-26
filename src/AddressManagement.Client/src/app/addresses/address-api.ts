import { HttpClient, httpResource } from '@angular/common/http'
import { inject, Injectable } from '@angular/core'
import { AddressCreate, AddressDetail, AddressPage, AddressQuery, Country } from './addresses.models'

const Url = '/api/addresses'

@Injectable({ providedIn: 'root' })
export class AddressApi {
  private readonly http = inject(HttpClient)

  list(query: () => AddressQuery) {
    return httpResource<AddressPage>(() => ({ url: Url, params: withoutEmpty(query()) }))
  }

  detail(id: () => number | null) {
    return httpResource<AddressDetail>(() => {
      const current = id()
      return current === null ? undefined : `${Url}/${current}`
    })
  }

  countries() {
    return httpResource<Country[]>(() => '/api/countries')
  }

  create(address: AddressCreate) {
    return this.http.post<AddressDetail>(Url, address)
  }

  update(id: number, address: AddressCreate) {
    return this.http.put<AddressDetail>(`${Url}/${id}`, address)
  }

  delete(id: number) {
    return this.http.delete<void>(`${Url}/${id}`)
  }

  deleteMany(ids: number[]) {
    return this.http.delete<void>(Url, { params: { ids } })
  }
}

function withoutEmpty(query: AddressQuery) {
  return Object.fromEntries(
    Object.entries(query).filter(([, v]) => v !== '' && !(Array.isArray(v) && v.length === 0)),
  )
}
