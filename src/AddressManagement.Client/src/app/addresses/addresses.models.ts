export type AddressListItem = {
  id: number
  street: string
  zipCode: string
  location: string
  country: string
  recipient: string
}

export type AddressCreate = {
  street: string
  zipCode: string
  location: string
  country: string
  recipient: string
  addressAffix: string | null
}

export type AddressDetail = {
  id: number
} & AddressCreate

export type Country = {
  name: string
}

export type PagedResult<T> = {
  items: T[]
  total: number
  totalCapped: boolean // true when there are more matches than the API counts
  pageSize: number
  page: number
}

// One place for field labels, shared by table, detail view and form.
export const AddressLabels = {
  street: 'Street',
  zipCode: 'Zip code',
  location: 'Location',
  country: 'Country',
  recipient: 'Recipient',
  addressAffix: 'Address affix',
} satisfies Record<keyof AddressCreate, string>

export type AddressField = keyof typeof AddressLabels
export const AddressFields = Object.keys(AddressLabels) as AddressField[]
