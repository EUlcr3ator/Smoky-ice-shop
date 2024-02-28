using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodsController : Controller
    {
        private readonly DeliveryMethodsModel _deliveryMethodsModel;

        public DeliveryMethodsController(DeliveryMethodsModel deliveryMethodsModel)
        {
            _deliveryMethodsModel = deliveryMethodsModel;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<DeliveryMethodDTO>> GetAllDeliveryMethods()
        {
            return Ok(_deliveryMethodsModel.GetDeliveryMethods());
        }
    }
}
