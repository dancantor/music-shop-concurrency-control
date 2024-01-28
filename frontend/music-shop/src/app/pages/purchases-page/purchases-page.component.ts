import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeeService } from 'src/app/api/services/employee.service';
import { InstrumentService } from 'src/app/api/services/instrument.service';
import { PurchaseService } from 'src/app/api/services/purchase.service';
import { Employee } from 'src/app/core/models/Employee';
import { Instrument } from 'src/app/core/models/instrument';
import { Purchase } from 'src/app/core/models/purchase';

@Component({
  selector: 'app-purchases-page',
  templateUrl: './purchases-page.component.html',
  styleUrls: ['./purchases-page.component.scss']
})
export class PurchasesPageComponent implements OnInit {
  protected purchases$!: Observable<Array<Purchase>>;
  protected employees$!: Observable<Array<Employee>>;
  protected instruments$!: Observable<Array<Instrument>>;
  protected dateSold?: Date;
  protected selectedEmployee?: Employee;
  protected selectedInstrument?: Instrument;

  constructor(private purchaseService: PurchaseService, private employeesService: EmployeeService, private instrumentService: InstrumentService) {}

  ngOnInit(): void {
    this.purchases$ = this.purchaseService.getAllPurchases();
    this.employees$ = this.employeesService.getAllEmployees();
    this.instruments$ = this.instrumentService.getAllInstruments();
  }

  protected isFieldCompleted() {
    return this.selectedEmployee && this.selectedInstrument && this.dateSold;
  }

  protected saveNewPurchase() {
    var newPurchase: Purchase = {
      instrumentId: this.selectedInstrument?.id,
      employeeId: this.selectedEmployee?.id,
      dateSold: this.dateSold
    }
    this.purchaseService.saveNewPurchase(newPurchase).subscribe(() => {
      this.purchases$ = this.purchaseService.getAllPurchases();
      this.dateSold = undefined;
      this.selectedEmployee = undefined;
      this.selectedInstrument = undefined;
    });
  }

  protected deletePurchase(purchaseId: number) {
    this.purchaseService.deletePurchase(purchaseId).subscribe(() =>
      this.purchases$ = this.purchaseService.getAllPurchases()
    )
  }
}
