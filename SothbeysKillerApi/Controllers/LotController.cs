using Microsoft.AspNetCore.Mvc;
using SothbeysKillerApi.Services.Interfaces;

namespace SothbeysKillerApi.Controllers;

public record CreateLotRequest(Guid AuctionId, string Title, string? Description, decimal StartPrice, decimal PriceStep);
public record UpdateLotRequest(string Title, string? Description, decimal StartPrice, decimal PriceStep);
public record LotResponce(Guid Id, Guid AuctionId, string Title, string? Description, decimal StartPrice, decimal PriceStep);


[ApiController]
[Route("api/v1/[controller]")]
public class LotController(ILotService lotService) : ControllerBase
{
    private readonly ILotService _lotService = lotService;

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var lot = _lotService.GetById(id);
        return Ok(lot);
    }
    [HttpGet("[action]/{auction_id:guid}")]
    public IActionResult Auction(Guid auction_id)
    {
        var lots = _lotService.GetByAuctionId(auction_id);
        return Ok(lots);
    }

    [HttpPost]
    public IActionResult Create(CreateLotRequest request)
    {
        var id = _lotService.Create(request);
        return Ok(new { Id = id });
    }
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, UpdateLotRequest request)
    {
        _lotService.UpdateById(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _lotService.DeleteById(id);
        return NoContent();
    }
}