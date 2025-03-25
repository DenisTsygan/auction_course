using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Services.Interfaces;

public interface ILotService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">if lot dont exist</exception>
    LotResponce GetById(Guid id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">if lots dont exist, length list = 0</exception>
    /// <exception cref="ArgumentException">if auctionId is not valid</exception>
    List<LotResponce> GetByAuctionId(Guid auctionId);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">if createRequest not valid</exception>
    Guid Create(CreateLotRequest request);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">if lot or auction dont exist</exception>
    /// <exception cref="ArgumentException">if auction is past or updateRequest is not valid</exception>
    void UpdateById(Guid id, UpdateLotRequest request);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <exception cref="NullReferenceException">if lot or auction dont exist</exception>
    /// <exception cref="ArgumentException">if auction is past</exception>
    void DeleteById(Guid id);
}