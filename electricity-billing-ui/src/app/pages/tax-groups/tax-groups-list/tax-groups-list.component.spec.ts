import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxGroupsListComponent } from './tax-groups-list.component';

describe('TaxGroupsListComponent', () => {
  let component: TaxGroupsListComponent;
  let fixture: ComponentFixture<TaxGroupsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaxGroupsListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(TaxGroupsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
