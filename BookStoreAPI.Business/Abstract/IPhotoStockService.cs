using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Dtos.PhotoDtos;
using Microsoft.AspNetCore.Http;
using IResult = BookStoreAPI.Core.Utilities.Result.Abstract.IResult;

namespace BookStoreAPI.Business.Abstract
{
    public interface IPhotoStockService
    {
        Task<IDataResult<List<PhotoDto>>> CreatePhotosAsync(IFormFileCollection photos, CancellationToken cancellationToken);
        IResult DeletePhoto(string pictureUrl);
    }
}
