using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : Controller
    {
        private readonly GoodsModel _goodsModel;

        public GoodsController(GoodsModel goodsModel)
        {
            _goodsModel = goodsModel;
        }

        [HttpGet("discounts")]
        public ActionResult<IEnumerable<DefaultGoodDTO>> GetAllDiscounts()
        {
            return Ok(_goodsModel.GetGoodsWithDiscount());
        }

        [HttpGet("{goodId}")]
        public ActionResult<DefaultGoodDTO> GetGood(Guid goodId)
        {
            return Ok(_goodsModel.GetGood(goodId));
        }

        [HttpPost("search/{pageSize}/{pageNumber}")]
        public ActionResult<SearchCollection> Search(int pageSize, int pageNumber, [FromBody]string text)
        {
            return Ok(_goodsModel.Search(pageSize, pageNumber, text));
        }

        [HttpGet("similar/{goodId}")]
        public ActionResult<IEnumerable<SearchGoodDTO>> GetSimilarGood(Guid goodId)
        {
            return Ok(_goodsModel.GetSimilar(goodId));
        }

        [HttpGet("count-in-orders/{goodId}")]
        public async Task<ActionResult<int>> GetStatistics(Guid goodId)
        {
            return await _goodsModel.GetGoodsOrdersCount(goodId, null, null);
        }

        [HttpPost("count-in-orders/{goodId}")]
        public async Task<ActionResult<int>> GetStatistics(Guid goodId, [FromBody]DateRange dateRange)
        {
            return await _goodsModel.GetGoodsOrdersCount(goodId, dateRange.DateFrom, dateRange.DateTo);
        }

        [HttpGet("goods-ratings")]
        public IEnumerable<CategoryGoodsRatingDTO> GetCategoriesStatistics()
        {
            return _goodsModel.GetGoodsRatings(null, null);
        }

        [HttpPost("goods-ratings")]
        public IEnumerable<CategoryGoodsRatingDTO> GetCategoriesStatistics([FromBody] DateRange dateRange)
        {
            return _goodsModel.GetGoodsRatings(dateRange.DateFrom, dateRange.DateTo);
        }
    }
}
