using MusicShop.Business.Model;

namespace MusicShop.Business.Service.Interfaces;

public interface IMusicalInstrumentService
{
    public Task<List<MusicalInstrumentModel>> GetAllAsync();
    public Task<MusicalInstrumentModel> InsertOneAsync(MusicalInstrumentModel musicalInstrumentModel);
    public Task<MusicalInstrumentModel> UpdateItemStock(int instrumentId, int newStock);
}