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
        public IActionResult GetCart([FromHeader] string Token, int UserId)
        {
            var respone = _cartRepository.GetCart(Token, UserId);
            if (respone.Code == 401)
                return Unauthorized(respone);
            if (respone.Code == 400)
                return BadRequest(respone);
            return Ok(respone);
        }


        [HttpPost("AddToCart/{UserId}")]
        public IActionResult AddToCart([FromHeader] string Token, int UserId, [FromForm] int cartId, [FromForm] CartRequest CartRequest)
        {
            var respone = _cartRepository.AddToCart(Token, UserId, cartId, CartRequest);
            if (respone.Code == 401)
                return Unauthorized(respone);
            if (respone.Code == 400)
                return BadRequest(respone);
            return Ok(respone);
        }




        [HttpDelete("RemoveProductFromCart/{UserId}")]
        public IActionResult RemoveProductFromCart([FromHeader] string Token, int UserId, [FromForm] int CartId, [FromForm] int ProductId)
        {
            var respone = _cartRepository.RemoveProductFromCart(Token, UserId, CartId, ProductId);
            if (respone.Code == 401)
                return Unauthorized(respone);
            if (respone.Code == 400)
                return BadRequest(respone);
            return Ok(respone);
        }

        /*  
       [HttpPut("UpdateProductInCart/{UserId}")]
       public IActionResult UpdateProductInCart([FromHeader] string Token, int UserId, [FromQuery] int CartId, [FromQuery] int Quantity)
       {
           var respone = _cartRepository.UpdateProductInCart(Token, UserId, CartId, Quantity);
           if (respone.Code == 401)
               return Unauthorized(respone);
           if (respone.Code == 400)
               return BadRequest(respone);
           return Ok(respone);
       }
*/

        [HttpDelete("ClearAllCart")]
        public IActionResult ClearAllCart()
        {
            var respone = _cartRepository.ClearAllCart();
            return Ok(respone);
        }

        [HttpDelete("DeleteCartById/{CartId}")]
        public IActionResult DeleteCartById(int CartId)
        {
            var respone = _cartRepository.DeleteCartById(CartId);
            if (respone.Code == 401)
                return Unauthorized(respone);
            if (respone.Code == 400)
                return BadRequest(respone);
            return Ok(respone);
        }

    }
}
