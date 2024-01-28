using MusicShop.DataAccess.Entity;

namespace MusicShop.DataAccess.Repository.Interfaces;

public interface IInstrumentPurchaseRepository
{
    public Task<InstrumentPurchase> GetByIdAsync(int purchaseId);
    public Task<List<InstrumentPurchase>> GetAllAsync();
    public Task<InstrumentPurchase> InsertOneAsync(InstrumentPurchase instrumentPurchase);
    public Task<InstrumentPurchase> DeleteOneAsync(int purchaseId);
}