using System;
using System.Collections.Generic;

namespace MusicShop.DataAccess.Entity;

public partial class MusicalInstrument
{
    public int Id { get; set; }

    public int? ItemsStock { get; set; }

    public int? Price { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<InstrumentPurchase> InstrumentPurchases { get; set; } = new List<InstrumentPurchase>();
}
