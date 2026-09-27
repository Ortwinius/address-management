import { ComponentFixture, TestBed } from '@angular/core/testing'
import { provideHttpClient } from '@angular/common/http'
import { provideHttpClientTesting } from '@angular/common/http/testing'
import { AddressStore } from '../address-store'
import { AddressesForm } from './addresses-form'

describe('AddressesForm', () => {
  let component: AddressesForm
  let fixture: ComponentFixture<AddressesForm>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [AddressStore, provideHttpClient(), provideHttpClientTesting()],
      imports: [AddressesForm],
    }).compileComponents()

    fixture = TestBed.createComponent(AddressesForm)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
