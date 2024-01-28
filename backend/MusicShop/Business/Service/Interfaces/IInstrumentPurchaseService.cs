using MusicShop.Business.Model;
using MusicShop.Controllers.DTO;

namespace MusicShop.Business.Service.Interfaces;

public interface IInstrumentPurchaseService
{
    public Task<List<InstrumentPurchaseDTO>> GetAllAsync();
    public Task<InstrumentPurchaseDTO> BuyInstrumentAsync(InstrumentPurchaseModel purchaseModel);
    public Task<InstrumentPurchaseDTO> ReturnInstrumentAsync(int purchaseId);
}