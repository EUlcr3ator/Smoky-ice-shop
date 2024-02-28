using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.CreateGroups;
using SMOKYICESHOP_API_TEST.DTO.Filters;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;
using System.Data;
using System.Text.RegularExpressions;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PodsController : Controller
    {
        private readonly PodsModel _podsModel;

        public PodsController(PodsModel podsModel)
        {
            _podsModel = podsModel;
        }

        [HttpGet("{groupId}")]
        public ActionResult<PodGroupDTO> GetPod(Guid groupId)
        {
            return Ok(_podsModel.GetGroup(groupId));
        }

        [HttpGet()]
        public ActionResult<GroupsCollection> GetAllGoods()
        {
            return Ok(_podsModel.GetAllPods());
        }

        [HttpGet("{pageSize}/{pageNumber}")]
        public ActionResult<GroupsCollection> GetAllGoods(int pageSize, int pageNumber)
        {
            return Ok(_podsModel.GetAllPods(pageSize, pageNumber));
        }

        [HttpPost("{pageSize}/{pageNumber}")]
        public ActionResult<GoodsCollection> GetAllGoods(int pageSize, int pageNumber, [FromBody] PodFilterDTO filter)
        {
            return Ok(_podsModel.GetAllPods(pageSize, pageNumber, filter));
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] CreatePodGroupDTO group)
        {
            return Ok(_podsModel.AddGroup(group));
        }

        [HttpDelete("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(Guid groupId)
        {
            return Ok(_podsModel.RemoveGroup(groupId));
        }

        [HttpPatch("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid groupId, [FromBody] CreatePodGroupDTO group)
        {
            return Ok(_podsModel.UpdateGroup(group, groupId));
        }

        [HttpPost("goods")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGood([FromBody] CreatePodGoodDTO good)
        {
            return Ok(_podsModel.AddGood(good));
        }

        [HttpDelete("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveGood(Guid goodId)
        {
            return Ok(_podsModel.RemoveGood(goodId));
        }

        [HttpPatch("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateGood(Guid goodId, [FromBody] CreatePodGoodDTO good)
        {
            return Ok(_podsModel.UpdateGood(good, goodId));
        }

        [HttpGet("Weight")]
        public ActionResult GetWeights()
        {
            return Ok(_podsModel.GetWeights());
        }

        [HttpGet("Material")]
        public ActionResult GetMaterials()
        {
            return Ok(_podsModel.GetMaterials());
        }

        [HttpGet("EvaporatorResistance")]
        public ActionResult GetEvaporatorResistances()
        {
            return Ok(_podsModel.GetEvaporatorResistances());
        }

        [HttpGet("Power")]
        public ActionResult GetPowers()
        {
            return Ok(_podsModel.GetPowers());
        }

        [HttpGet("Battarey")]
        public ActionResult GetBattareys()
        {
            return Ok(_podsModel.GetBattareys());
        }

        [HttpGet("CartrigeCapacity")]
        public ActionResult GetCartrigeCapacities()
        {
            return Ok(_podsModel.GetCartrigeCapacities());
        }

        [HttpGet("Port")]
        public ActionResult GetPorts()
        {
            return Ok(_podsModel.GetPorts());
        }

        [HttpGet("Appearance")]
        public ActionResult GetAppearances()
        {
            return Ok(_podsModel.GetAppearances());
        }
    }
}
