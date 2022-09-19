using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturants.DTO.Requests;
using Resturants.Repositories.Interfaces;
using Resturants.Repositories.other;

namespace Resturants.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository _cartRepository)
        {
            this._cartRepository = _cartRepository;
        }



        [HttpGet("{UserId}")]
        public IActionResult GetCart(int UserId, [FromHeader] string Token)
        {
            var respone = _cartRepository.GetCart(UserId, Token);
            if (respone.Code == 401)
                return Unauthorized(respone);
            if (respone.Code == 400)
                return BadRequest(respone);
            return Ok(respone);
        }

        [HttpPost("AddToCart/{UserId}")]
        public IActionResult AddToCart(int UserId, [FromHeader] string Token, [FromForm] CartRequest CartRequest)
        {
            var respone = _cartRepository.AddToCart(UserId, Token, CartRequest);
            if (respone.Code == 401)
                return Unauthorized(respone);
            if (respone.Code == 400)
                return BadRequest(respone);
            return Ok(respone);
        }
    }
}
