import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { AddressesForm } from './addresses-form';

describe('AddressesForm', () => {
  let component: AddressesForm;
  let fixture: ComponentFixture<AddressesForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
      imports: [AddressesForm],
    }).compileComponents();

    fixture = TestBed.createComponent(AddressesForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
