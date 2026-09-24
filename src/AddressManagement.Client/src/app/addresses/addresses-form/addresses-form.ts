import {Component, effect, inject, model, output, signal} from '@angular/core';
import {HttpClient, httpResource} from '@angular/common/http';
import {AddressCreate, AddressDetail} from "../addresses.models";
import { form, FormField, required, maxLength } from '@angular/forms/signals';
import {MatError, MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import {finalize} from 'rxjs';

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
    MatButton,
    MatProgressSpinner
  ],
  selector: 'app-addresses-form',
  styleUrl: './addresses-form.css',
  templateUrl: './addresses-form.html',
})
export class AddressesForm {
  private readonly http = inject(HttpClient)

  readonly selectedId = model<number | null>(null)
  readonly saved = output<void>()

  protected readonly saving = signal<boolean>(false)
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
    /* When id is null reset the form, else fill it with the detail data */
    effect(() => {
      if (this.selectedId() === null) {
        this.model.set(EmptyAddress)
        return
      }
      const detail = this.detail.value()
      if (detail) {
        const { id, ...address } = detail
        this.model.set(address)
      }
    })
  }

  /* Update saving state,
  if the current form page has an id
  it means its an existing address
  -> update, else create a new one
  -> signal parent with emit and set saving-state finally to false */
  protected save(){
    this.saving.set(true)
    const id = this.selectedId()
    const request = id
      ? this.http.put(`/api/addresses/${id}`, this.model())
      : this.http.post('/api/addresses', this.model())
    request.pipe(finalize(() => this.saving.set(false))).subscribe(() => this.saved.emit())
  }
  protected reset(){this.selectedId.set(null)}
}
