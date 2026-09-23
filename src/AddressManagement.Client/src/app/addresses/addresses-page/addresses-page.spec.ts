import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { AddressesPage } from './addresses-page';

describe('AddressesPage', () => {
  let component: AddressesPage;
  let fixture: ComponentFixture<AddressesPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
      imports: [AddressesPage],
    }).compileComponents();

    fixture = TestBed.createComponent(AddressesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
