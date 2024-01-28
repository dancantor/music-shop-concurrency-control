import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { Employee } from 'src/app/core/models/Employee';
import { format } from 'date-fns';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = `${environment.apiUrl}/employees`;

  constructor(private httpClient: HttpClient) { }

  public getAllEmployees(): Observable<Array<Employee>> {
    return this.httpClient.get<Array<Employee>>(this.apiUrl);
  }

  public saveNewEmployee(employee: Employee): Observable<Employee> {
    employee.dateOfBirth = format(employee.dateOfBirth!, "yyyy-MM-dd")
    return this.httpClient.post<Employee>(this.apiUrl, employee);
  }
}
