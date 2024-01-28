using System;
using System.Collections.Generic;

namespace MusicShop.DataAccess.Entity;

public partial class InstrumentPurchase
{
    public int Id { get; set; }

    public int? InstrumentId { get; set; }

    public DateOnly? DateSold { get; set; }

    public int? EmployeeId { get; set; }

    public virtual MusicalInstrument? Instrument { get; set; }
}
