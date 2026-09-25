export type AddressListItem = {
  id: number;
  street: string;
  zipCode: string;
  location: string;
  country: string;
}

export type AddressCreate = {
  street: string;
  zipCode: string;
  location: string;
  country: string;
  recipient: string | null;
  addressAffix: string | null;
}

export type AddressDetail = {
  id: number;
} & AddressCreate

export type Country = {
  name: string;
}

export type PagedResult<T> = {
  items: T[];
  total: number;
  pageSize: number;
  page: number;
}

// One place for field labels, shared by table, detail view and form.
export const AddressLabels = {
  street: 'Straße',
  zipCode: 'PLZ',
  location: 'Ort',
  country: 'Land',
  recipient: 'Empfänger',
  addressAffix: 'Adresszusatz',
} satisfies Record<keyof AddressCreate, string>

export type AddressField = keyof typeof AddressLabels
export const AddressFields = Object.keys(AddressLabels) as AddressField[]
