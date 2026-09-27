import { ComponentFixture, TestBed } from '@angular/core/testing'
import { provideHttpClient } from '@angular/common/http'
import { provideHttpClientTesting } from '@angular/common/http/testing'
import { AddressStore } from '../address-store'
import { AddressesList } from './addresses-list'

describe('AddressesList', () => {
  let component: AddressesList
  let fixture: ComponentFixture<AddressesList>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [AddressStore, provideHttpClient(), provideHttpClientTesting()],
      imports: [AddressesList],
    }).compileComponents()

    fixture = TestBed.createComponent(AddressesList)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
