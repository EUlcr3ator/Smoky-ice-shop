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

    [ApiController]
    [Route("api/[controller]")]
    public class ECigarettesController : Controller
    {
        private readonly ECigarettesModel _eCigarettesModel;

        public ECigarettesController(ECigarettesModel eCigarettesModel)
        {
            _eCigarettesModel = eCigarettesModel;
        }

        [HttpGet("{groupId}")]
        public ActionResult<EcigaretteGroupDTO> GetGroup(Guid groupId)
        {
            return Ok(_eCigarettesModel.GetGroup(groupId));
        }

        [HttpGet()]
        public ActionResult<GroupsCollection> GetAllGoods()
        {
            return Ok(_eCigarettesModel.GetAllCartriges());
        }

        [HttpGet("{pageSize}/{pageNumber}")]
        public ActionResult<GroupsCollection> GetAllGoods(int pageSize, int pageNumber)
        {
            return Ok(_eCigarettesModel.GetAllCartriges(pageSize, pageNumber));
        }

        [HttpPost("{pageSize}/{pageNumber}")]
        public ActionResult<GoodsCollection> GetAllGoods(int pageSize, int pageNumber, [FromBody]ECigaretteFilterDTO filter)
        {
            return Ok(_eCigarettesModel.GetAllCartriges(pageSize, pageNumber, filter));
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] CreateEcigaretteGroupDTO group)
        {
            return Ok(_eCigarettesModel.AddGroup(group));
        }

        [HttpDelete("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(Guid groupId)
        {
            return Ok(_eCigarettesModel.RemoveGroup(groupId));
        }

        [HttpPatch("{groupId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid groupId, [FromBody] CreateEcigaretteGroupDTO group)
        {
            return Ok(_eCigarettesModel.UpdateGroup(group, groupId));
        }

        [HttpPost("goods")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGood([FromBody] CreateEcigaretteGoodDTO good)
        {
            return Ok(_eCigarettesModel.AddGood(good));
        }

        [HttpDelete("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveGood(Guid goodId)
        {
            return Ok(_eCigarettesModel.RemoveGood(goodId));
        }

        [HttpPatch("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateGood(Guid goodId, [FromBody] CreateEcigaretteGoodDTO good)
        {
            return Ok(_eCigarettesModel.UpdateGood(good, goodId));
        }

        [HttpGet("BattareyCapacity")]
        public ActionResult<short> GetBattareyCapacities()
        {
            return Ok(_eCigarettesModel.GetBattareyCapacities());
        }

        [HttpGet("Taste")]
        public ActionResult<string> GetTastes()
        {
            return Ok(_eCigarettesModel.GetTastes());
        }

        [HttpGet("EvaporatorVolume")]
        public ActionResult<byte> GetEvaporatorVolumes()
        {
            return Ok(_eCigarettesModel.GetEvaporatorVolumes());
        }

        [HttpGet("PuffsCount")]
        public ActionResult<int> GetPuffsCounts()
        {
            return Ok(_eCigarettesModel.GetPuffsCounts());
        }

        [HttpPost("TasteMix")]
        public ActionResult<Guid> PostTastes([FromBody] TasteMixDTO<string> tasteMix)
        {
            return Ok(_eCigarettesModel.AddTasteMix(tasteMix));
        }
    }
}
