import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeeService } from 'src/app/api/services/employee.service';
import { Employee } from 'src/app/core/models/Employee';

@Component({
  selector: 'app-employee-page',
  templateUrl: './employee-page.component.html',
  styleUrls: ['./employee-page.component.scss']
})
export class EmployeePageComponent implements OnInit {
  protected employees$!: Observable<Array<Employee>>;
  protected newEmployee: Employee = {
    name: '',
    dateOfBirth: undefined,
    position: undefined
  }

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.employees$ = this.employeeService.getAllEmployees();
  }

  protected isFieldCompleted() {
    return this.newEmployee.name !== '' && this.newEmployee.dateOfBirth && this.newEmployee.position;
  }

  protected saveNewEmployee() {
    this.employeeService.saveNewEmployee(this.newEmployee).subscribe(() => {
      this.employees$ = this.employeeService.getAllEmployees();
      this.newEmployee = {
        name: '',
        dateOfBirth: undefined,
        position: undefined
      };
    })
  }
}
