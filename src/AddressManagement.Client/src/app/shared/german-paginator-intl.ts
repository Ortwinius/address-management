import {Injectable} from '@angular/core';
import {MatPaginatorIntl} from '@angular/material/paginator';

// German texts for every mat-paginator in the app.
@Injectable()
export class GermanPaginatorIntl extends MatPaginatorIntl {
  override itemsPerPageLabel = 'Einträge pro Seite'
  override nextPageLabel = 'Nächste Seite'
  override previousPageLabel = 'Vorherige Seite'
  override firstPageLabel = 'Erste Seite'
  override lastPageLabel = 'Letzte Seite'

  override getRangeLabel = (page: number, pageSize: number, length: number) => {
    const start = Math.min(page * pageSize + 1, length)
    const end = Math.min((page + 1) * pageSize, length)
    return `${start} - ${end} von ${length.toLocaleString('de')}`
  }
}
