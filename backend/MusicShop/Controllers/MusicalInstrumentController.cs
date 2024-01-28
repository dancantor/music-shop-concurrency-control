using Microsoft.AspNetCore.Mvc;
using MusicShop.Business.Model;
using MusicShop.Business.Service.Interfaces;
using MusicShop.Controllers.DTO;

namespace MusicShop.Controllers;

[ApiController]
[Route("api/instruments")]
public class MusicalInstrumentController : ControllerBase
{
    private readonly IMusicalInstrumentService _musicalInstrumentService;

    public MusicalInstrumentController(IMusicalInstrumentService musicalInstrumentService)
    {
        _musicalInstrumentService = musicalInstrumentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MusicalInstrumentModel>>> GetAllInstruments()
    {
        return Ok(await _musicalInstrumentService.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<MusicalInstrumentModel>> InsertOneInstrument([FromBody] MusicalInstrumentModel musicalInstrumentModel)
    {
        return CreatedAtAction(nameof(GetAllInstruments), await _musicalInstrumentService.InsertOneAsync(musicalInstrumentModel));
    }

    [HttpPut]
    public async Task<ActionResult<MusicalInstrumentModel>> UpdateInstrumentItemsStock(
        [FromBody] UpdateInstrumentItemsStockDTO updateInstrumentItemsStockDto)
    {
        return Ok(await _musicalInstrumentService.UpdateItemStock(updateInstrumentItemsStockDto.Id,
            updateInstrumentItemsStockDto.ItemsStock));
    }
}