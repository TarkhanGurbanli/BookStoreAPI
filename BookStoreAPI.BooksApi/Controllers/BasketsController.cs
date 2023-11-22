using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Entities.Dtos.BasketDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("getBasket/{userId}")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBasket(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("Invalid userId");

                var result = await _basketService.GetBasket(userId);

                if (result.Success)
                    return Ok(result.Data);

                return NotFound(result.Message);
            }
            catch (Exception ex)
            {
                // Daha fazla hata yönetimi ekleyebilirsiniz
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("addBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBasket([FromBody] BasketDto basketDto)
        {
            var result = await _basketService.AddOrUpdateBasket(basketDto);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("deleteBasket/{userId}/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBasket(string userId, string bookId)
        {
            var result = await _basketService.DeleteAsync(userId, bookId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

    }
}
