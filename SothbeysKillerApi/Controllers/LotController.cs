using Microsoft.AspNetCore.Mvc;
using SothbeysKillerApi.Services.Interfaces;

namespace SothbeysKillerApi.Controllers;

public record CreateLotRequest(Guid AuctionId, string Title, string? Description, decimal StartPrice, decimal PriceStep);
public record UpdateLotRequest(string Title, string? Description, decimal StartPrice, decimal PriceStep);
public record LotResponce(Guid Id, Guid AuctionId, string Title, string? Description, decimal StartPrice, decimal PriceStep);

public class Lot
{
    public Guid Id { get; set; }
    public Guid AuctionId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal StartPrice { get; set; }
    public decimal PriceStep { get; set; }
}

[ApiController]
[Route("api/v1/[controller]")]
public class LotController(ILotService lotService) : ControllerBase
{
    private readonly ILotService _lotService = lotService;

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var lot = _lotService.GetById(id);
            return Ok(lot);
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Houston, we have a problem...");
        }
    }
    [HttpGet("[action]/{auction_id:guid}")]
    public IActionResult Auction(Guid auction_id)
    {
        try
        {
            var lots = _lotService.GetByAuctionId(auction_id);
            return Ok(lots);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Houston, we have a problem...");
        }
    }

    [HttpPost]
    public IActionResult Create(CreateLotRequest request)
    {
        try
        {
            var id = _lotService.Create(request);
            return Ok(new { Id = id });
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            return StatusCode(500, "Houston, we have a problem...");
        }

    }
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, UpdateLotRequest request)
    {
        try
        {
            _lotService.UpdateById(id, request);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Houston, we have a problem...");
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _lotService.DeleteById(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Houston, we have a problem...");
        }
    }
}