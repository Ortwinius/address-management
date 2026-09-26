import type { components } from '../api/api-types'

// Generated from the API's OpenAPI document (npm run api:types), so client and server can't drift apart.
type Schemas = components['schemas']

export type AddressListItem = Schemas['AddressListDto']
export type AddressDetail = Schemas['AddressDetailDto']
export type AddressCreate = Schemas['AddressCreateDto']
export type AddressPage = Schemas['PagedResultOfAddressListDto']
export type Country = Schemas['CountryDto']

export type AddressFilter = {
  street: string;
  location: string;
  countries: string[];
}

// Note: could be auto generated too, but its hidden in openapi in the query params bc openApi flattened the [FromQuery] params
export type AddressQuery = AddressFilter & {
  sortCol: string
  desc: boolean
  page: number
  pageSize: number
}

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
