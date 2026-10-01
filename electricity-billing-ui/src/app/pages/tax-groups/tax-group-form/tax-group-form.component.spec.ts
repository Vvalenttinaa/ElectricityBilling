import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxGroupFormComponent } from './tax-group-form.component';

describe('TaxGroupFormComponent', () => {
  let component: TaxGroupFormComponent;
  let fixture: ComponentFixture<TaxGroupFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaxGroupFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(TaxGroupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
