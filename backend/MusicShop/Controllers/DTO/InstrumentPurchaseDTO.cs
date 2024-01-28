namespace MusicShop.Controllers.DTO;

public class InstrumentPurchaseDTO
{
    public int? Id { get; set; }

    public int? InstrumentId { get; set; }
    
    public string? InstrumentName { get; set; }

    public DateOnly? DateSold { get; set; }

    public int? EmployeeId { get; set; }
    
    public string? EmployeeName { get; set; }
}