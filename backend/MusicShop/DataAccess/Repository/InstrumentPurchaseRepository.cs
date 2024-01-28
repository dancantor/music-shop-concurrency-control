using Microsoft.EntityFrameworkCore;
using MusicShop.DataAccess.Context;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;

namespace MusicShop.DataAccess.Repository;

public class InstrumentPurchaseRepository : IInstrumentPurchaseRepository
{
    private readonly InstrumentContext _instrumentContext;

    public InstrumentPurchaseRepository(InstrumentContext instrumentContext)
    {
        _instrumentContext = instrumentContext;
    }
    
    public async Task<InstrumentPurchase> GetByIdAsync(int purchaseId)
    {
        var searchedPurchase =
            await _instrumentContext.InstrumentPurchases.Include(purchase => purchase.Instrument).FirstOrDefaultAsync(instrument =>
                instrument.Id == purchaseId);
        if (searchedPurchase is null)
        {
            throw new KeyNotFoundException("Couldn't find the purchase with that id key");
        }

        return searchedPurchase;
    }

    public async Task<List<InstrumentPurchase>> GetAllAsync()
    {
        return await _instrumentContext.InstrumentPurchases.ToListAsync();
    }

    public async Task<InstrumentPurchase> InsertOneAsync(InstrumentPurchase instrumentPurchase)
    {
        var insertedPurchase = await _instrumentContext.InstrumentPurchases.AddAsync(instrumentPurchase);
        await _instrumentContext.SaveChangesAsync();
        return insertedPurchase.Entity;
    }

    public async Task<InstrumentPurchase> DeleteOneAsync(int purchaseId)
    {
        var deletedPurchase = await GetByIdAsync(purchaseId);
        if (deletedPurchase is null)
        {
            throw new KeyNotFoundException("Couldn't find the purchase with that id key");
        }

        _instrumentContext.InstrumentPurchases.Remove(deletedPurchase);
        await _instrumentContext.SaveChangesAsync();
        return deletedPurchase;
    }
}