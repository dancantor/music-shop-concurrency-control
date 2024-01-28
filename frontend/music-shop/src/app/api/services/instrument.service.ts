import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Instrument } from 'src/app/core/models/instrument';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class InstrumentService {
  private apiUrl = `${environment.apiUrl}/instruments`;

  constructor(private httpClient: HttpClient) { }

  public getAllInstruments(): Observable<Array<Instrument>> {
    return this.httpClient.get<Array<Instrument>>(this.apiUrl);
  }

  public saveNewInstrument(instrument: Instrument): Observable<Instrument> {
    return this.httpClient.post<Instrument>(this.apiUrl, instrument);
  }

  public updateStock(instrumentId: number, newStockValue: number): Observable<Instrument> {
    return this.httpClient.put<Instrument>(this.apiUrl, {
      id: instrumentId,
      itemsStock: newStockValue
    })
  };
}
