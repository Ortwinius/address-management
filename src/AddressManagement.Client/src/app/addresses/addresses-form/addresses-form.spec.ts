import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddressesForm } from './addresses-form';

describe('AddressesForm', () => {
  let component: AddressesForm;
  let fixture: ComponentFixture<AddressesForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
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
