using MusicShop.DataAccess.Entity;

namespace MusicShop.Business.Model;

public class InstrumentPurchaseModel
{
    public InstrumentPurchaseModel()
    {
    }

    public InstrumentPurchaseModel(InstrumentPurchase instrumentPurchase)
    {
        Id = instrumentPurchase.Id;
        InstrumentId = instrumentPurchase.InstrumentId;
        DateSold = instrumentPurchase.DateSold;
        EmployeeId = instrumentPurchase.EmployeeId;
    }
    
    public int? Id { get; set; }

    public int? InstrumentId { get; set; }

    public DateOnly? DateSold { get; set; }

    public int? EmployeeId { get; set; }
}