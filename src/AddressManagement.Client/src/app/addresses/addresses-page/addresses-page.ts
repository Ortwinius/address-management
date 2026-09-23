import {Component, signal, viewChild} from '@angular/core';
import {AddressesList} from '../addresses-list/addresses-list';
import {AddressesForm} from '../addresses-form/addresses-form';

@Component({
  imports: [
    AddressesList,
    AddressesForm
  ],
  selector: 'app-addresses-page',
  styleUrl: './addresses-page.css',
  templateUrl: './addresses-page.html',
})
export class AddressesPage {
  protected readonly selectedId = signal<number | null>(null);
  private readonly list = viewChild.required(AddressesList)

  protected onSaved(){
    this.list().reload()
    this.selectedId.set(null)
  }
}
