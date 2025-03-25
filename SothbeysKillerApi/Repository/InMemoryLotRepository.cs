using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Repository.Interface;

namespace SothbeysKillerApi.Repository;

public class InMemoryLotRepository : ILotRepository
{
    private static List<Lot> _storage = [];
    public Lot Create(Lot entity)
    {
        _storage.Add(entity);
        return entity;
    }

    public void Delete(Guid id)
    {
        _storage = _storage.Where(l => l.Id != id).ToList();
    }

    public IEnumerable<Lot> GetByAuctionId(Guid auctionId)
    {
        return _storage.Where(l => l.AuctionId == auctionId).OrderByDescending(l => l.Title);
    }

    public Lot? GetById(Guid id)
    {
        return _storage.FirstOrDefault(l => l.Id == id);
    }

    public void Update(Lot entity)
    {
    }
}