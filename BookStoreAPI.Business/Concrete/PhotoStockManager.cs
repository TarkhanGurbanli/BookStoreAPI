using AutoMapper;
using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult;
using BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Dtos.PhotoDtos;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using IResult = BookStoreAPI.Core.Utilities.Result.Abstract.IResult;


namespace BookStoreAPI.Business.Concrete
{
    public class PhotoStockManager : IPhotoStockService
    {
        //MongoDb ile elaqeni qururuq
        private readonly IMongoCollection<PhotoDto> _photoCollection;

        public PhotoStockManager(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            //MongoDb ile ConnectionString elaqesini qururuq
            var client = new MongoClient(databaseSettings.ConnectionString);
            //MongoDb ile DatabaseName elaqesini qururuq
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _photoCollection = database.GetCollection<PhotoDto>(databaseSettings.PhotoCollectionName);
        }

        public async Task<IDataResult<List<PhotoDto>>> CreatePhotosAsync(IFormFileCollection photos, CancellationToken cancellationToken)
        {
            try
            {
                if (photos == null || photos.Count == 0)
                    return new ErrorDataResult<List<PhotoDto>>("No photos uploaded");

                var uploadedPhotos = new List<PhotoDto>();

                foreach (var photo in photos)
                {
                    if (photo.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", photo.FileName);

                        using var stream = new FileStream(path, FileMode.Create);
                        await photo.CopyToAsync(stream, cancellationToken);

                        var returnPath = "Photos/" + photo.FileName;

                        PhotoDto photoDto = new PhotoDto { Url = returnPath };
                        uploadedPhotos.Add(photoDto);

                        await _photoCollection.InsertOneAsync(photoDto, cancellationToken: cancellationToken);
                    }
                }

                return new SuccessDataResult<List<PhotoDto>>(uploadedPhotos, $"{uploadedPhotos.Count} photos uploaded successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<PhotoDto>>($"An error occurred while uploading the photos: {ex.Message}");
            }
        }


        public IResult DeletePhoto(string pictureUrl)
        {
            try
            {
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", pictureUrl);

                if (File.Exists(wwwrootPath))
                {
                    File.Delete(wwwrootPath);
                }
                else
                {
                    return new ErrorResult("File not found");
                }

                var filter = Builders<PhotoDto>.Filter.Eq(x => x.Url, pictureUrl) |
                             Builders<PhotoDto>.Filter.Eq(x => x.Url, "Photos/" + pictureUrl);
                var result = _photoCollection.DeleteOne(filter);

                if (result.DeletedCount > 0)
                {
                    return new SuccessResult("File deleted successfully");
                }
                else
                {
                    return new ErrorResult("File not found");
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while deleting the file: {ex.Message}");
            }
        }


    }
}
