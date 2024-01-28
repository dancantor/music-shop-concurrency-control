using MusicShop.DataAccess.Entity;

namespace MusicShop.DataAccess.Repository.Interfaces;

public interface IMusicalInstrumentRepository
{
    public Task<MusicalInstrument> GetByIdAsync(int instrumentId);
    public Task<List<MusicalInstrument>> GetAllAsync();
    public Task<MusicalInstrument> InsertOneAsync(MusicalInstrument musicalInstrument);
    public Task<MusicalInstrument> UpdateOneAsync(MusicalInstrument musicalInstrument);
    public Task<MusicalInstrument> DeleteOneAsync(int instrumentId);
}