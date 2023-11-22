using BookStoreAPI.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoStockService _photoStockService;

        public PhotosController(IPhotoStockService photoStockService)
        {
            _photoStockService = photoStockService;
        }
        [HttpPost("uploadPhotos")]
        public async Task<IActionResult> UploadPhotos([FromForm] IFormFileCollection photos, CancellationToken cancellationToken)
        {
            if (photos == null || photos.Count == 0)
            {
                return BadRequest("No photos uploaded");
            }

            var result = await _photoStockService.CreatePhotosAsync(photos, cancellationToken);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpDelete("delete/{pictureUrl}")]
        public IActionResult DeletePhoto(string pictureUrl)
        {
            var result = _photoStockService.DeletePhoto(pictureUrl);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
