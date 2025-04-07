using SothbeysKillerApi.Entity;

namespace SothbeysKillerApi.Repository.Interface;

public interface ILotRepository
{
    Lot? GetById(Guid id);
    IEnumerable<Lot> GetByAuctionId(Guid auctionId);
    Lot Create(Lot entity);
    void Update(Lot entity);
    void Delete(Guid id);
}
