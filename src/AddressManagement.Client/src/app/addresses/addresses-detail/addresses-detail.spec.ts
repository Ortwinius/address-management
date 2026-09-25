import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { AddressStore } from '../address-store';
import { AddressesDetail } from './addresses-detail';

describe('AddressesDetail', () => {
  let component: AddressesDetail;
  let fixture: ComponentFixture<AddressesDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [AddressStore, provideHttpClient(), provideHttpClientTesting()],
      imports: [AddressesDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(AddressesDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
