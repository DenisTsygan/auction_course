using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Contexts;
using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Entity;
using SothbeysKillerApi.Exceptions;
using SothbeysKillerApi.Services.Interfaces;

namespace SothbeysKillerApi.Services;

public class DefaultLotService(IAuctionService auctionService, LotDbContext lotDbContext) : ILotService
{
    private readonly IAuctionService _auctionService = auctionService;
    private readonly LotDbContext _lotDbContext = lotDbContext;

    public LotResponce GetById(Guid id)
    {
        var lot = _lotDbContext.Lots
            .AsNoTracking()
            .FirstOrDefault(l => l.Id == id)
                ?? throw new EntityExceptionNullreference(nameof(Lot));
        return MapResponce(lot);
    }

    public List<LotResponce> GetByAuctionId(Guid auctionId)
    {
        var auction = _auctionService.GetAuctionById(auctionId)
            ?? throw new LotExceptionValidation(nameof(auctionId), "auction is not found");

        var lots = _lotDbContext.Lots
            .AsNoTracking()
            .Where(l => l.AuctionId == auctionId)
            .ToList();
        if (lots.Count == 0) throw new EntityExceptionNullreference(nameof(List<object>) + " of " + nameof(Lot));
        return lots.Select(MapResponce).ToList();
    }

    public Guid Create(CreateLotRequest request)
    {
        var auction = _auctionService.GetAuctionById(request.AuctionId)
            ?? throw new LotExceptionValidation(nameof(request.AuctionId), "auction is not found");

        ValidateAuctionIsFutere(auction);

        ValidateLotRequest(request.Title, request.Description, request.StartPrice, request.PriceStep);

        var newLot = new Lot
        {
            Id = Guid.NewGuid(),
            AuctionId = auction.Id,
            Title = request.Title,
            Description = request.Description,
            StartPrice = request.StartPrice,
            PriceStep = request.PriceStep
        };
        _lotDbContext.Lots.Add(newLot);
        _lotDbContext.SaveChanges();

        return newLot.Id;
    }

    public void UpdateById(Guid id, UpdateLotRequest request)
    {
        var lot = _lotDbContext.Lots
            .FirstOrDefault(l => l.Id == id)
                ?? throw new EntityExceptionNullreference(nameof(Lot));

        var auction = _auctionService.GetAuctionById(lot.AuctionId)
             ?? throw new EntityExceptionNullreference(nameof(Auction));

        ValidateAuctionIsFutere(auction);
        ValidateLotRequest(request.Title, request.Description, request.StartPrice, request.PriceStep);

        lot.Title = request.Title;
        lot.Description = request.Description;
        lot.StartPrice = request.StartPrice;
        lot.PriceStep = request.PriceStep;

        _lotDbContext.SaveChanges();
    }

    public void DeleteById(Guid id)
    {
        var lot = _lotDbContext.Lots
             .FirstOrDefault(l => l.Id == id)
                 ?? throw new EntityExceptionNullreference(nameof(Lot));

        var auction = _auctionService.GetAuctionById(lot.AuctionId)
            ?? throw new EntityExceptionNullreference(nameof(Auction));

        ValidateAuctionIsFutere(auction);

        _lotDbContext.Lots.Remove(lot);
        _lotDbContext.SaveChanges();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="StartPrice"></param>
    /// <param name="PriceStep"></param>
    /// <exception cref="LotExceptionValidation">if not valid data</exception>
    private void ValidateLotRequest(string title, string? description, decimal StartPrice, decimal PriceStep)
    {
        if (title.Length < 3 || title.Length > 255) throw new LotExceptionValidation(nameof(title), "length should be > 3 and < 255 symbols");
        if (description?.Length > 255) throw new LotExceptionValidation(nameof(description), "length should < 255 symbols");
        if (StartPrice <= 0) throw new LotExceptionValidation(nameof(StartPrice), "should be more than 0");
        if (PriceStep <= 0) throw new LotExceptionValidation(nameof(PriceStep), "should be more than 0");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="auction"></param>
    /// <exception cref="LotExceptionValidation">if auction is not future</exception>
    private void ValidateAuctionIsFutere(AuctionResponse auction)
    {
        if (auction.Start <= DateTime.Now) throw new LotExceptionValidation(nameof(auction), "auction is past");
    }

    private LotResponce MapResponce(Lot lot)
    {
        return new LotResponce(lot.Id, lot.AuctionId, lot.Title, lot.Description, lot.StartPrice, lot.PriceStep);
    }
}