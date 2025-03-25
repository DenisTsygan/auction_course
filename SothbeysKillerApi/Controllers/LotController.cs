using Microsoft.AspNetCore.Mvc;

namespace SothbeysKillerApi.Controllers;

public record CreateLotRequest(Guid AuctionId, string Title, string? Description, decimal StartPrice, decimal PriceStep);
public record UpdateLotRequest(string Title, string? Description, decimal StartPrice, decimal PriceStep);

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
public class LotController : ControllerBase
{
    private static List<Lot> _storageLots = [];
    private static List<Auction> _storageAuctions = [];
    private static Lot? GetLotById(Guid id)
    {
        return _storageLots.FirstOrDefault(l => l.Id == id);
    }
    private static Auction? GetAuctionById(Guid id)
    {
        var list = _storageAuctions;
        return list.FirstOrDefault(a => a.Id == id);
    }
    /*[HttpGet("[action]")]
    public IActionResult InitAuctionStorage()
    {
        _storageAuctions.Add(new Auction()
        {
            Id = Guid.Parse("27d7bd1b-79fd-41ba-8e11-eee17d5d1c32"),
            Start = DateTime.Now.AddDays(5),
            Title = "123",
            Finish = DateTime.Now.AddDays(35)
        });
        _storageAuctions.Add(new Auction()
        {
            Id = Guid.Parse("ec73883e-6f10-4f7b-8bd4-3fc60868a7ea"),
            Start = DateTime.Now.AddDays(1),
            Title = "456",
            Finish = DateTime.Now.AddDays(20)
        });
        return NoContent();
    }*/ //test

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var lot = GetLotById(id);
        if (lot is null) return NotFound();
        return Ok(lot);
    }
    [HttpGet("[action]/{auction_id:guid}")]
    public IActionResult Auction(Guid auction_id)
    {
        var isAuctionId = _storageAuctions.Any(a => a.Id == auction_id);
        if (!isAuctionId) return BadRequest();
        var lots = _storageLots.Where(l => l.AuctionId == auction_id).ToList();
        if (lots.Count == 0) return NotFound();
        return Ok(lots);
    }

    [HttpPost]
    public IActionResult Create(CreateLotRequest request)
    {
        var auction = GetAuctionById(request.AuctionId);
        if (auction is null) return BadRequest();
        if (auction.Start <= DateTime.Now) return BadRequest();

        if (request.Title.Length < 3 || request.Title.Length > 255) return BadRequest();
        if (request.StartPrice <= 0 || request.PriceStep <= 0) return BadRequest();

        var newLot = new Lot
        {
            Id = Guid.NewGuid(),
            AuctionId = auction.Id,
            Title = request.Title,
            Description = request.Description,
            StartPrice = request.StartPrice,
            PriceStep = request.PriceStep
        };
        _storageLots.Add(newLot);
        return Ok(new { Id = newLot.Id });
    }
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, UpdateLotRequest request)
    {
        var lot = GetLotById(id);
        if (lot is null) return NotFound();

        var auction = GetAuctionById(lot.AuctionId);
        if (auction is null) return NotFound();// cannot be null, because when creating is checking in db and in controller/service 
        if (auction.Start <= DateTime.Now) return BadRequest();

        if (request.Title.Length < 3 || request.Title.Length > 255) return BadRequest();
        if (request.StartPrice <= 0 || request.PriceStep <= 0) return BadRequest();

        lot.Title = request.Title;
        lot.Description = request.Description;
        lot.StartPrice = request.StartPrice;
        lot.PriceStep = request.PriceStep;

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var lot = GetLotById(id);
        if (lot is null) return NotFound();

        var auction = GetAuctionById(lot.AuctionId);
        if (auction is null) return NotFound();
        if (auction.Start <= DateTime.Now) return BadRequest();

        _storageLots.Remove(lot);

        return NoContent();
    }
}