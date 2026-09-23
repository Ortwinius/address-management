import {Component, output, signal} from '@angular/core';
import {AddressListItem, AddressQuery, PagedResult} from '../addresses.models';
import { httpResource } from '@angular/common/http';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {
  MatCell, MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef,
  MatRow, MatRowDef,
  MatTable
} from '@angular/material/table';
import {MatButton} from '@angular/material/button';
@Component({
  imports: [
    MatFormField,
    MatLabel,
    MatTable,
    MatColumnDef,
    MatInput,
    MatButton,
    MatHeaderRow,
    MatRow,
    MatCell,
    MatHeaderCell,
    MatPaginator,
    MatHeaderRowDef,
    MatHeaderCellDef,
    MatCellDef,
    MatRowDef
  ],
  selector: 'app-addresses-list',
  styleUrl: './addresses-list.css',
  templateUrl: './addresses-list.html',
})
export class AddressesList {
  readonly selected = output<number>();
  protected readonly columns = ['street']

  protected readonly streetInput = signal('')

  protected readonly query = signal<AddressQuery>(
    {street: '' , page: 1, pageSize: 10, sort: 'street' })

  // TODO: Add <Pagedresult> as wrapper
  protected readonly addresses = httpResource<PagedResult<AddressListItem>>(() => ({
    url: '/api/addresses',
    params: { page: this.query().page, pageSize: this.query().pageSize },
  }))

  reload() {this.addresses.reload()}
  toParams(q: AddressQuery) {
    // TODO: filter duplicates
  }
  protected search(){

  }
  onPage(e: PageEvent){
    this.query.update(q => ({ ...q, page: e.pageIndex + 1, pageSize: e.pageSize }))
  }

}
