import {Component, effect, inject, input, output, signal} from '@angular/core';
import {HttpClient, httpResource} from '@angular/common/http';
import {AddressCreate, AddressDetail} from "../addresses.models";
import { form, FormField, required, maxLength } from '@angular/forms/signals';
import {MatError, MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {MatButton} from '@angular/material/button';

const EmptyAddress: AddressCreate = {
  street: '', zipCode: '', location: '', country: '', recipient: null, addressAffix: null
}

@Component({
  imports: [
    MatFormField,
    MatLabel,
    MatInput,
    FormField,
    MatError,
    MatButton
  ],
  selector: 'app-addresses-form',
  styleUrl: './addresses-form.css',
  templateUrl: './addresses-form.html',
})
export class AddressesForm {
  private readonly http = inject(HttpClient)

  readonly selectedId = input<number | null>(null)
  readonly saved = output<void>()

  protected readonly model = signal<AddressCreate>(EmptyAddress)
  protected readonly form = form(this.model, p => {
    required(p.street);
    required(p.zipCode);
    required(p.location);
    required(p.country);
  })

  private readonly detail = httpResource<AddressDetail>(() => {
    const id = this.selectedId()
    return id ? `/api/addresses/${id}` : undefined
  })

  constructor(){
    // effect(() => {
    //
    // })
  }
  protected save(){

  }
  protected reset(){this.model.set(EmptyAddress)}
}
