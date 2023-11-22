using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Entities.Dtos.WishListsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListsController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [Authorize]
        [HttpPost("addwishlist/{bookId}")]
        public IActionResult AddWishList([FromBody] WishListAddItemDto wishListAddItemDTO)
        {
            var userId = GetUserIdFromToken();

            var result = _wishListService.AddWishList(userId, wishListAddItemDTO);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize]
        [HttpDelete("removeWishList/{bookId}")]
        public IActionResult RemoveWishList(string bookId)
        {
            var userId = GetUserIdFromToken();

            var result = _wishListService.RemoveWishList(userId, bookId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("userwishlist")]
        public IActionResult GetUserWishList()
        {
            var userId = GetUserIdFromToken();
            var result = _wishListService.GetUserWishList(userId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        private string GetUserIdFromToken()
        {
            var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(bearerToken);
            return jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
        }
    }
}
