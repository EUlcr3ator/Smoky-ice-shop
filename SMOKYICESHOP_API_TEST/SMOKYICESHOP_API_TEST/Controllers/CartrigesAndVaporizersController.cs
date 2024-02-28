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
using System.Text.RegularExpressions;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartrigesAndVaporizersController : Controller
    {
        private readonly CartrigesAndVaporizersModel _cartrigesAndVaporizersModel;

        public CartrigesAndVaporizersController(CartrigesAndVaporizersModel cartrigesAndVaporizersModel)
        {
            _cartrigesAndVaporizersModel = cartrigesAndVaporizersModel;
        }

        [HttpGet("{groupId}")]
        public ActionResult<CartrigeAndVaporizerGroupDTO> GetCartrigeAndVaporizer(Guid groupId)
        {
            try
            {
                return Ok(_cartrigesAndVaporizersModel.GetGroup(groupId));
            }
            catch(InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpGet()]
        public ActionResult<GroupsCollection> GetAllGroups()
        {
            return Ok(_cartrigesAndVaporizersModel.GetAllCartriges());
        }

        [HttpGet("{pageSize}/{pageNumber}")]
        public ActionResult<GroupsCollection> GetCartrigeAndVaporizer(int pageSize, int pageNumber)
        {
            return Ok(_cartrigesAndVaporizersModel.GetAllCartriges(pageSize, pageNumber));
        }

        [HttpPost("{pageSize}/{pageNumber}")]
        public ActionResult<GoodsCollection> GetAllGoods(int pageSize, int pageNumber, [FromBody] CartrigeFilterDTO filter)
        {
            return Ok(_cartrigesAndVaporizersModel.GetAllCartriges(pageSize, pageNumber, filter));
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public ActionResult<Guid> Add([FromBody] CreateCartrigeAndVaporizerGroupDTO group)
        {
            return Ok(_cartrigesAndVaporizersModel.AddGroup(group));
        }

        [HttpDelete("{groupId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Guid> Remove(Guid groupId)
        {
            return Ok(_cartrigesAndVaporizersModel.RemoveGroup(groupId));
        }

        [HttpPatch("{groupId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<bool> Update(Guid groupId, [FromBody] CreateCartrigeAndVaporizerGroupDTO group)
        {
            return Ok(_cartrigesAndVaporizersModel.UpdateGroup(group, groupId));
        }

        [HttpPost("goods")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Guid> AddGood(CreateCartrigeAndVaporizerGoodDTO good)
        {
            return Ok(_cartrigesAndVaporizersModel.AddGood(good));
        }

        [HttpDelete("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<bool> RemoveGood(Guid goodId)
        {
            return Ok(_cartrigesAndVaporizersModel.RemoveGood(goodId));
        }

        [HttpPatch("goods/{goodId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<bool> UpdateGood(Guid goodId, [FromBody] CreateCartrigeAndVaporizerGoodDTO good)
        {
            return Ok(_cartrigesAndVaporizersModel.UpdateGood(good, goodId));
        }

        [HttpGet("SpiralType")]
        public ActionResult<string> GetSpiralTypes()
        {
            return Ok(_cartrigesAndVaporizersModel.GetSpiralTypes());
        }

        [HttpGet("CartrigeCapacity")]
        public ActionResult<double> GetCartrigeCapacities()
        {
            return Ok(_cartrigesAndVaporizersModel.GetCartrigeCapacities());
        }

        [HttpGet("Resistance")]
        public ActionResult<double> GetResistances()
        {
            return Ok(_cartrigesAndVaporizersModel.GetResistances());
        }
    }
}
