using Microsoft.AspNetCore.Mvc;
using MusicShop.Business.Model;
using MusicShop.Business.Service.Interfaces;
using MusicShop.Controllers.DTO;

namespace MusicShop.Controllers;

[ApiController]
[Route("api/purchases")]
public class InstrumentPurchaseController : ControllerBase
{
    private readonly IInstrumentPurchaseService _purchaseService;

    public InstrumentPurchaseController(IInstrumentPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<ActionResult<List<InstrumentPurchaseDTO>>> GetAllPurchases()
    {
        return Ok(await _purchaseService.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<InstrumentPurchaseDTO>> BuyInstrument([FromBody] InstrumentPurchaseModel employeeModel)
    {
        return CreatedAtAction(nameof(GetAllPurchases), await _purchaseService.BuyInstrumentAsync(employeeModel));
    }

    [HttpDelete("{purchaseId}")]
    public async Task<ActionResult<InstrumentPurchaseDTO>> ReturnInstrument(int purchaseId)
    {
        return Ok(await _purchaseService.ReturnInstrumentAsync(purchaseId));
    }
}