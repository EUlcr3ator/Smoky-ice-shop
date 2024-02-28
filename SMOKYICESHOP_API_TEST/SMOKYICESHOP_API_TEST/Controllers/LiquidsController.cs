using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiquidsController : Controller
    {
        private readonly LiquidsModel _liquidsModel;
         
        public LiquidsController(LiquidsModel liquidsModel)
        {
            _liquidsModel = liquidsModel;
        }

        [HttpGet("{groupId}")]
        public ActionResult<LiquidGroupDTO> GetLiquid(Guid groupId)
        {
            return Ok(_liquidsModel.GetGroup(groupId));
        }

        [HttpGet()]
        public ActionResult<GroupsCollection> GetAllLiquids()
        {
            return Ok(_liquidsModel.GetAllLiquids());
        }

        [HttpGet("{pageSize}/{pageNumber}")]
        public ActionResult<GroupsCollection> GetAllLiquids(int pageSize, int pageNumber)
        {
            return Ok(_liquidsModel.GetAllLiquids(pageSize, pageNumber));
        }

        [HttpPost("{pageSize}/{pageNumber}")]
        public ActionResult<GoodsCollection> GetAllLiquids(int pageSize, int pageNumber, [FromBody] LiquidFilterDTO filter)
        {
            return Ok(_liquidsModel.GetAllLiquids(pageSize, pageNumber, filter));
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] CreateLiquidGroupDTO group)
        {
            return Ok(_liquidsModel.AddGroup(group));
        }

        [HttpDelete("{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(Guid goodId)
        {
            return Ok(_liquidsModel.RemoveGroup(goodId));
        }

        [HttpPatch("{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid goodId, [FromBody] CreateLiquidGroupDTO group)
        {
            return Ok(_liquidsModel.UpdateGroup(group, goodId));
        }

        [HttpPost("goods")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGood([FromBody] CreateLiquidGoodDTO good)
        {
            return Ok(_liquidsModel.AddGood(good));
        }

        [HttpDelete("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveGood(Guid goodId)
        {
            return Ok(_liquidsModel.RemoveGood(goodId));
        }

        [HttpPatch("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateGood(Guid goodId, [FromBody] CreateLiquidGoodDTO good)
        {
            return Ok(_liquidsModel.UpdateGood(good, goodId));
        }

        [HttpGet("NicotineStrength")]
        public ActionResult GetNicotineStrengths()
        {
            return Ok(_liquidsModel.GetNicotineStrengths());
        }

        [HttpGet("NicotineType")]
        public ActionResult GetNicotineTypes()
        {
            return Ok(_liquidsModel.GetNicotineTypes());
        }

        [HttpGet("Capacity")]
        public ActionResult GetCapacities()
        {
            return Ok(_liquidsModel.GetCapacities());
        }

        [HttpGet("TasteGroup")]
        public ActionResult GetTasteGroups()
        {
            return Ok(_liquidsModel.GetTasteGroups());
        }

        [HttpGet("Taste")]
        public ActionResult GetTastes()
        {
            return Ok(_liquidsModel.GetTastes());
        }

        [HttpPost("TasteMix")]
        public ActionResult<Guid> PostTastes([FromBody] TasteMixDTO<TasteDTO> tasteMix)
        {
            return Ok(_liquidsModel.AddTasteMix(tasteMix));
        }
    }
}
