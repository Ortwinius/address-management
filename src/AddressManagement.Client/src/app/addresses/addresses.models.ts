export type AddressListItem = {
  id: number;
  street: string;
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

export type PagedResult<T> = {
  items: T[];
  total: number;
  pageSize: number;
  page: number;
}

export type AddressQuery = {
  street: string;
  page: number;
  pageSize: number;
  sort: string;
}
