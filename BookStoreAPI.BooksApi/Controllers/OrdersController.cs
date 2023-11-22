using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Entities.Dtos.OrdersDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("createOrder")]
        [ProducesResponseType(typeof(List<OrderCreateDTO>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<OrderCreateDTO>), StatusCodes.Status401Unauthorized)]
        public IActionResult CreateOrder([FromBody] List<OrderCreateDTO> orderCreateDTOs)
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(_bearer_token);
            var userId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value;

            var result = _orderService.CreateOrder(userId, orderCreateDTOs);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("cancelOrder/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CancelOrder(string orderId)
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(_bearer_token);
            var userId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value;

            var result = _orderService.CancelOrder(userId, orderId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("getUserOrders/{userId}")]
        public IActionResult GetUserOrders(string userId)
        {
            var result = _orderService.GetOrdersByUser(userId);

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }
    }
}
