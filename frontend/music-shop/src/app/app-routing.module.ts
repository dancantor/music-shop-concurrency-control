import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeePageComponent } from './pages/employee-page/employee-page.component';
import { InstrumentPageComponent } from './pages/instrument-page/instrument-page.component';
import { PurchasesPageComponent } from './pages/purchases-page/purchases-page.component';

const routes: Routes = [
  { path: 'employees', component: EmployeePageComponent },
  { path: 'instruments', component: InstrumentPageComponent },
  { path: 'purchases', component: PurchasesPageComponent },
  { path: '', redirectTo: '/employees', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
