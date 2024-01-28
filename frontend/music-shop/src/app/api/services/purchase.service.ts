import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { format } from 'date-fns';
import { Observable } from 'rxjs';
import { Purchase } from 'src/app/core/models/purchase';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
  private apiUrl = `${environment.apiUrl}/purchases`;

  constructor(private httpClient: HttpClient) { }

  public getAllPurchases(): Observable<Array<Purchase>> {
    return this.httpClient.get<Array<Purchase>>(this.apiUrl);
  }

  public saveNewPurchase(purchase: Purchase): Observable<Purchase> {
    purchase.dateSold = format(purchase.dateSold!, "yyyy-MM-dd")
    return this.httpClient.post<Purchase>(this.apiUrl, purchase);
  }

  public deletePurchase(purchaseId: number): Observable<Purchase> {
    return this.httpClient.delete<Purchase>(`${this.apiUrl}/${purchaseId}`);
  };
}
