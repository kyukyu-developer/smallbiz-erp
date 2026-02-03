import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HrPayrollRoutingModule } from './hr-payroll-routing.module';
import { HrPayrollComponent } from './hr-payroll.component';


@NgModule({
  declarations: [
    HrPayrollComponent
  ],
  imports: [
    CommonModule,
    HrPayrollRoutingModule
  ]
})
export class HrPayrollModule { }
