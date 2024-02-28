using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.Cart;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;
using System.Security.Claims;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartsModel _cartsModel;

        private readonly AuthenticationService _authenticationService;

        public CartController(CartsModel cartsModel, AuthenticationService authenticationService)
        {
            _cartsModel = cartsModel;
            _authenticationService = authenticationService;
        }

        [HttpGet()]
        [Authorize]
        public ActionResult<IEnumerable<CartGoodDTO>> GetCart()
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            return Ok(_cartsModel.GetGoods(clientID));
        }

        [HttpPut()]
        [Authorize]
        public IActionResult ChangeCartGood([FromBody] ChangeCartGoodDTO cartGood)
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            _cartsModel.ChangeCartGood(cartGood.GoodID, clientID, cartGood.Count);
            return Ok();
        }

        [HttpGet("{goodId}")]
        [Authorize]
        public ActionResult<byte> GetCart(Guid goodId)
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            return Ok(_cartsModel.GetProductCount(clientID, goodId));
        }

        [HttpPost("{goodId}")]
        [Authorize]
        public ActionResult<byte> PostCartGood(Guid goodId, [FromBody] sbyte changeCount)
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            byte count = _cartsModel.GetProductCount(clientID, goodId);

            int i = count + changeCount;

            if (i < 0)
                i = 0;

            try
            {
                count = Convert.ToByte(i);
            }
            catch (OverflowException)
            {
                return BadRequest("ChangeCount is too big for Byte type");
            }

            _cartsModel.ChangeCartGood(goodId, clientID, count);
            return Ok(count);
        }

        [HttpGet("count")]
        [Authorize]
        public ActionResult<byte> GetCartCount()
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            return Ok(_cartsModel.GetGoodsCountInCart(clientID));
        }

        [HttpHead("clear")]
        [Authorize]
        public IActionResult ClearCart()
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            _cartsModel.ClearCart(clientID);
            return Ok();
        }
    }
}
