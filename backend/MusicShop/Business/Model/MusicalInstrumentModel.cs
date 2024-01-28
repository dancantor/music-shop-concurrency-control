using MusicShop.DataAccess.Entity;

namespace MusicShop.Business.Model;

public class MusicalInstrumentModel
{
    public MusicalInstrumentModel()
    {
    }
    
    public MusicalInstrumentModel(MusicalInstrument musicalInstrument)
    {
        Id = musicalInstrument.Id;
        ItemsStock = musicalInstrument.ItemsStock;
        Price = musicalInstrument.Price;
        Name = musicalInstrument.Name;
    }

    public int? Id { get; set; }

    public int? ItemsStock { get; set; }

    public int? Price { get; set; }

    public string? Name { get; set; }
}