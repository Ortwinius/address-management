import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddressesPage } from './addresses-page';

describe('AddressesPage', () => {
  let component: AddressesPage;
  let fixture: ComponentFixture<AddressesPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddressesPage],
    }).compileComponents();

    fixture = TestBed.createComponent(AddressesPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
