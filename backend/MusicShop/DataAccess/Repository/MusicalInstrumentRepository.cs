using Microsoft.EntityFrameworkCore;
using MusicShop.DataAccess.Context;
using MusicShop.DataAccess.Entity;
using MusicShop.DataAccess.Repository.Interfaces;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace MusicShop.DataAccess.Repository;

public class MusicalInstrumentRepository : IMusicalInstrumentRepository
{
    private readonly InstrumentContext _instrumentContext;

    public MusicalInstrumentRepository(InstrumentContext instrumentContext)
    {
        _instrumentContext = instrumentContext;
    }
    
    public async Task<MusicalInstrument> GetByIdAsync(int musicalInstrumentId)
    {
        var searchedInstrument =
            await _instrumentContext.MusicalInstruments.FirstOrDefaultAsync(instrument =>
                instrument.Id == musicalInstrumentId);
        if (searchedInstrument is null)
        {
            throw new KeyNotFoundException("Couldn't find the instrument with that id key");
        }

        return searchedInstrument;
    }

    public async Task<List<MusicalInstrument>> GetAllAsync()
    {
        return await _instrumentContext.MusicalInstruments.ToListAsync();
    }

    public async Task<MusicalInstrument> InsertOneAsync(MusicalInstrument musicalInstrument)
    {
        var insertedInstrument = await _instrumentContext.MusicalInstruments.AddAsync(musicalInstrument);
        await _instrumentContext.SaveChangesAsync();
        return insertedInstrument.Entity;
    }

    public async Task<MusicalInstrument> UpdateOneAsync(MusicalInstrument musicalInstrument)
    {
        var initialInstrument = await GetByIdAsync(musicalInstrument.Id);

        initialInstrument.ItemsStock = musicalInstrument.ItemsStock ?? initialInstrument.ItemsStock;
        initialInstrument.Name = musicalInstrument.Name ?? initialInstrument.Name;
        initialInstrument.Price = musicalInstrument.Price ?? initialInstrument.Price;
        await _instrumentContext.SaveChangesAsync();
        return initialInstrument;
    }

    public async Task<MusicalInstrument> DeleteOneAsync(int instrumentId)
    {
        var deletedInstrument = await GetByIdAsync(instrumentId);
        if (deletedInstrument is null)
        {
            throw new KeyNotFoundException("Couldn't find the instrument with that id key");
        }

        _instrumentContext.MusicalInstruments.Remove(deletedInstrument);
        await _instrumentContext.SaveChangesAsync();
        return deletedInstrument;
    }
}