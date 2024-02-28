using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PopularGoodsController : Controller
    {
        private readonly PopularGoodsModel _popularGoodsModel;

        public PopularGoodsController(PopularGoodsModel popularGoodsModel)
        {
            _popularGoodsModel = popularGoodsModel;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<DefaultGoodDTO>> Get()
        {
            return Ok(_popularGoodsModel.GetAllPopularGoods());
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult AddByID([FromBody] Guid goodID)
        {
            _popularGoodsModel.AddToPopularGood(goodID);
            return Ok();
        }

        [HttpDelete("{goodID}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveByID(Guid goodID)
        {
            _popularGoodsModel.RemoveFromPopular(goodID);
            return Ok();
        }
    }
}
