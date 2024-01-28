import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { InstrumentService } from 'src/app/api/services/instrument.service';
import { Instrument } from 'src/app/core/models/instrument';

@Component({
  selector: 'app-instrument-page',
  templateUrl: './instrument-page.component.html',
  styleUrls: ['./instrument-page.component.scss']
})
export class InstrumentPageComponent implements OnInit {
  protected instruments$!: Observable<Array<Instrument>>;
  protected newInstrument: Instrument = {
    name: "",
    itemsStock: undefined,
    price: undefined
  }

  constructor(private instrumentService: InstrumentService) {}

  ngOnInit(): void {
    this.instruments$ = this.instrumentService.getAllInstruments();
  }

  protected isFieldCompleted() {
    return this.newInstrument.name !== '' && this.newInstrument.itemsStock && this.newInstrument.price;
  }

  protected saveNewInstrument() {
    this.instrumentService.saveNewInstrument(this.newInstrument).subscribe(() => {
      this.instruments$ = this.instrumentService.getAllInstruments();
      this.newInstrument = {
        name: '',
        itemsStock: undefined,
        price: undefined
      };
    })
  }

  protected updateInstrumentsStatus(instrument: Instrument) {
    this.instrumentService.updateStock(instrument.id!, instrument.itemsStock!).subscribe();
  }
}
