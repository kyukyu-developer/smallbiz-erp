import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HrPayrollComponent } from './hr-payroll.component';

const routes: Routes = [{ path: '', component: HrPayrollComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HrPayrollRoutingModule { }
