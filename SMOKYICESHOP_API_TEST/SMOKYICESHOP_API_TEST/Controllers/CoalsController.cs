using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class CoalsController : Controller
    {
        private readonly CoalsModel _coalsModel;

        public CoalsController(CoalsModel coalsModel)
        {
            _coalsModel = coalsModel;
        }

        [HttpGet("{groupId}")]
        public ActionResult<CoalGroupDTO> GetGood(Guid groupId)
        {
            return Ok(_coalsModel.GetGroup(groupId));
        }

        [HttpGet()]
        public ActionResult<GroupsCollection> GetAllGoods()
        {
            return Ok(_coalsModel.GetAllCoals());
        }

        [HttpGet("{pageSize}/{pageNumber}")]
        public ActionResult<GroupsCollection> GetAllGoods(int pageSize, int pageNumber)
        {
            return Ok(_coalsModel.GetAllCoals(pageSize, pageNumber));
        }

        [HttpPost("{pageSize}/{pageNumber}")]
        public ActionResult<GoodsCollection> GetAllGoods(int pageSize, int pageNumber, [FromBody] CoalFilterDTO filter)
        {
            return Ok(_coalsModel.GetAllCoals(pageSize, pageNumber, filter));
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] CreateCoalGroupDTO group)
        {
            return Ok(_coalsModel.AddGroup(group));
        }

        [HttpDelete("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(Guid groupId)
        {
            return Ok(_coalsModel.RemoveGroup(groupId));
        }

        [HttpPatch("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid groupId, [FromBody] CreateCoalGroupDTO group)
        {
            return Ok(_coalsModel.UpdateGroup(group, groupId));
        }

        [HttpPost("goods")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGood([FromBody] CreateCoalGoodDTO good)
        {
            return Ok(_coalsModel.AddGood(good));
        }

        [HttpDelete("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveGood(Guid goodId)
        {
            return Ok(_coalsModel.RemoveGood(goodId));
        }

        [HttpPatch("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateGood(Guid goodId, [FromBody] CreateCoalGoodDTO good)
        {
            return Ok(_coalsModel.UpdateGood(good, goodId));
        }

        [HttpGet("Type")]
        public ActionResult<string> GetTypes()
        {
            return Ok(_coalsModel.GetTypes());
        }

        [HttpGet("Weight")]
        public ActionResult<double> GetWeights()
        {
            return Ok(_coalsModel.GetWeights());
        }
    }
}
