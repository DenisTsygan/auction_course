using System.Data;
using Dapper;
using SothbeysKillerApi.Entity;
using SothbeysKillerApi.Repository.Interface;

namespace SothbeysKillerApi.Repository;

public class DbLotRepository : ILotRepository
{
    private readonly IDbConnection _dbConnection;

    public DbLotRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Lot? GetById(Guid id)
    {
        var query = "select * from lots where id = @Id;";
        return _dbConnection.QuerySingleOrDefault<Lot>(query, new { Id = id });
    }

    public IEnumerable<Lot> GetByAuctionId(Guid auctionId)
    {
        var query = "select * from lots where auction_id = @AuctionId order by title desc;";
        return _dbConnection.Query<Lot>(query, new { AuctionId = auctionId });
    }

    public Lot Create(Lot entity)
    {
        var query = @"insert into lots (id, auction_id, title, description, start_price, price_step) 
                      values (@Id, @AuctionId, @Title, @Description, @StartPrice, @PriceStep) 
                      returning *;";
        return _dbConnection.QueryFirst<Lot>(query, entity);
    }

    public void Update(Lot entity)
    {
        var updateCommand = @"update lots set title = @Title, description = @Description,
                            start_price = @StartPrice, price_step = @PriceStep 
                            where id = @Id;";
        _dbConnection.ExecuteScalar(updateCommand, entity);
    }

    public void Delete(Guid id)
    {
        var deleteCommand = "delete from lots where id = @Id;";
        _dbConnection.ExecuteScalar(deleteCommand, new { Id = id });
    }
}