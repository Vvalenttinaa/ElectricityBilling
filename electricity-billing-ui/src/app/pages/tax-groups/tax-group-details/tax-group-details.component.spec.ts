import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxGroupDetailsComponent } from './tax-group-details.component';

describe('TaxGroupDetailsComponent', () => {
  let component: TaxGroupDetailsComponent;
  let fixture: ComponentFixture<TaxGroupDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaxGroupDetailsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(TaxGroupDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
