import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrPayrollComponent } from './hr-payroll.component';

describe('HrPayrollComponent', () => {
  let component: HrPayrollComponent;
  let fixture: ComponentFixture<HrPayrollComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HrPayrollComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HrPayrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
