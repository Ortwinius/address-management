import {Component, inject, input, linkedSignal} from '@angular/core';
import {form, FormField, required} from '@angular/forms/signals';
import {MatError, MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import {AddressCreate, AddressDetail, AddressField, AddressFields, AddressLabels} from '../addresses.models';
import {AddressStore} from '../address-store';

// Inputs work with strings, the API uses null for an empty address affix.
type AddressFormValue = Record<AddressField, string>

@Component({
  imports: [
    MatFormField,
    MatLabel,
    MatInput,
    FormField,
    MatError,
    MatButton,
    MatProgressSpinner
  ],
  selector: 'app-addresses-form',
  templateUrl: './addresses-form.html',
})
export class AddressesForm {
  protected readonly store = inject(AddressStore)

  // Edit mode gets the address, create mode starts empty.
  readonly address = input<AddressDetail>()

  protected readonly fields = AddressFields
  protected readonly labels = AddressLabels
  protected readonly model = linkedSignal(() => toFormValue(this.address()))
  protected readonly form = form(this.model, p => {
    required(p.street);
    required(p.zipCode);
    required(p.location);
    required(p.country);
    required(p.recipient);
  })

  protected save() {
    this.store.save(toAddressCreate(this.model()))
  }
}

function toFormValue(a?: AddressDetail): AddressFormValue {
  return {
    street: a?.street ?? '',
    zipCode: a?.zipCode ?? '',
    location: a?.location ?? '',
    country: a?.country ?? '',
    recipient: a?.recipient ?? '',
    addressAffix: a?.addressAffix ?? '',
  }
}

function toAddressCreate(v: AddressFormValue): AddressCreate {
  return { ...v, addressAffix: v.addressAffix || null }
}
