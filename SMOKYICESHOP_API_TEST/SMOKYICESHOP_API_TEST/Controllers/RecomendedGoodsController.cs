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
    public class RecomendedGoodsController : Controller
    {
        private readonly RecomendedGoodsModel _recomendedGoodsModel;

        public RecomendedGoodsController(RecomendedGoodsModel recomendedGoodsModel)
        {
            _recomendedGoodsModel = recomendedGoodsModel;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<DefaultGoodDTO>> Get()
        {
            return Ok(_recomendedGoodsModel.GetAllRecomendedGoods());
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult AddByID([FromBody] Guid goodID)
        {
            _recomendedGoodsModel.AddToRecomendedGood(goodID);
            return Ok();
        }

        [HttpDelete("{goodID}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveByID(Guid goodID)
        {
            _recomendedGoodsModel.RemoveFromRecomended(goodID);
            return Ok();
        }
    }
}
