using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Repository.Interface;
using SothbeysKillerApi.Services.Interfaces;

namespace SothbeysKillerApi.Services;

public class DefaultLotService(IAuctionService auctionService, ILotRepository lotRepository) : ILotService
{
    private readonly ILotRepository _lotRepository = lotRepository;
    private readonly IAuctionService _auctionService = auctionService;
    public LotResponce GetById(Guid id)
    {
        var lot = _lotRepository.GetById(id)
            ?? throw new NullReferenceException();
        return MapResponce(lot);
    }

    public List<LotResponce> GetByAuctionId(Guid auctionId)
    {
        var auction = _auctionService.GetAuctionById(auctionId)
            ?? throw new ArgumentException();

        var lots = _lotRepository.GetByAuctionId(auction.Id).ToList();
        if (lots.Count == 0) throw new NullReferenceException();
        return lots.Select(MapResponce).ToList();
    }

    public Guid Create(CreateLotRequest request)
    {
        var auction = _auctionService.GetAuctionById(request.AuctionId)
            ?? throw new ArgumentException();

        ValidateAuctionIsFutere(auction);

        ValidateRequest(request.Title, request.Description, request.StartPrice, request.PriceStep);

        var newLot = new Lot
        {
            Id = Guid.NewGuid(),
            AuctionId = auction.Id,
            Title = request.Title,
            Description = request.Description,
            StartPrice = request.StartPrice,
            PriceStep = request.PriceStep
        };
        var lot = _lotRepository.Create(newLot);
        return lot.Id;
    }

    public void UpdateById(Guid id, UpdateLotRequest request)
    {
        var lot = _lotRepository.GetById(id)
            ?? throw new NullReferenceException();

        var auction = _auctionService.GetAuctionById(lot.AuctionId)
             ?? throw new NullReferenceException();

        ValidateAuctionIsFutere(auction);
        ValidateRequest(request.Title, request.Description, request.StartPrice, request.PriceStep);

        lot.Title = request.Title;
        lot.Description = request.Description;
        lot.StartPrice = request.StartPrice;
        lot.PriceStep = request.PriceStep;
        _lotRepository.Update(lot);
    }

    public void DeleteById(Guid id)
    {
        var lot = _lotRepository.GetById(id)
            ?? throw new NullReferenceException();

        var auction = _auctionService.GetAuctionById(lot.AuctionId)
            ?? throw new NullReferenceException();

        ValidateAuctionIsFutere(auction);

        _lotRepository.Delete(id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="StartPrice"></param>
    /// <param name="PriceStep"></param>
    /// <exception cref="ArgumentException">if not valid data</exception>
    private void ValidateRequest(string title, string? description, decimal StartPrice, decimal PriceStep)
    {
        if (title.Length < 3 || title.Length > 255) throw new ArgumentException();
        if (description?.Length > 255) throw new ArgumentException();
        if (StartPrice <= 0 || PriceStep <= 0) throw new ArgumentException();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="auction"></param>
    /// <exception cref="ArgumentException">if auction is not future</exception>
    private void ValidateAuctionIsFutere(AuctionResponse auction)
    {
        if (auction.Start <= DateTime.Now) throw new ArgumentException();
    }

    private LotResponce MapResponce(Lot lot)
    {
        return new LotResponce(lot.Id, lot.AuctionId, lot.Title, lot.Description, lot.StartPrice, lot.PriceStep);
    }
}