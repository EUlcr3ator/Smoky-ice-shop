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
using System.Text.RegularExpressions;

namespace SMOKYICESHOP_API_TEST.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HookahTobaccoController : Controller
    {
        private readonly HookahTobaccosModel _hookahTobaccosModel;

        public HookahTobaccoController(HookahTobaccosModel hookahTobaccosModel)
        {
            _hookahTobaccosModel = hookahTobaccosModel;
        }

        [HttpGet()]
        public ActionResult<GroupsCollection> GetAllGoods()
        {
            return Ok(_hookahTobaccosModel.GetAllTobaccos());
        }

        [HttpGet("{pageSize}/{pageNumber}")]
        public ActionResult<GroupsCollection> GetAllGoods(int pageSize, int pageNumber)
        {
            return Ok(_hookahTobaccosModel.GetAllTobaccos(pageSize, pageNumber));
        }

        [HttpPost("{pageSize}/{pageNumber}")]
        public ActionResult<GoodsCollection> GetAllGoods(int pageSize, int pageNumber, [FromBody]TobaccoFilterDTO filter)
        {
            return Ok(_hookahTobaccosModel.GetAllTobaccos(pageSize, pageNumber, filter));
        }

        [HttpGet("{groupId}")]
        public ActionResult<HookahTobaccoGroupDTO> GetGood(Guid groupId)
        {
            return Ok(_hookahTobaccosModel.GetGroup(groupId));
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] CreateHookahTobaccoGroupDTO group)
        {
            return Ok(_hookahTobaccosModel.AddGroup(group));
        }

        [HttpDelete("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(Guid groupId)
        {
            return Ok(_hookahTobaccosModel.RemoveGroup(groupId));
        }

        [HttpPatch("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid groupId, [FromBody] CreateHookahTobaccoGroupDTO group)
        {
            return Ok(_hookahTobaccosModel.UpdateGroup(group, groupId));
        }

        [HttpPost("goods")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGood([FromBody] CreateHookahTobaccoGoodDTO good)
        {
            return Ok(_hookahTobaccosModel.AddGood(good));
        }

        [HttpDelete("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveGood(Guid goodId)
        {
            return Ok(_hookahTobaccosModel.RemoveGroup(goodId));
        }

        [HttpPatch("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateGood(Guid goodId, [FromBody] CreateHookahTobaccoGoodDTO good)
        {
            return Ok(_hookahTobaccosModel.UpdateGood(good, goodId));
        }

        [HttpGet("Strength")]
        public ActionResult<string> GetStrengths()
        {
            return Ok(_hookahTobaccosModel.GetStrengths());
        }

        [HttpGet("Weight")]
        public ActionResult<double> GetWeights()
        {
            return Ok(_hookahTobaccosModel.GetWeights());
        }

        [HttpGet("Taste")]
        public ActionResult<string> GetTastes()
        {
            return Ok(_hookahTobaccosModel.GetTastes());
        }

        [HttpPost("TasteMix")]
        public ActionResult<Guid> PostTastes([FromBody] TasteMixDTO<string> tasteMix)
        {
            return Ok(_hookahTobaccosModel.AddTasteMix(tasteMix));
        }
    }
}
