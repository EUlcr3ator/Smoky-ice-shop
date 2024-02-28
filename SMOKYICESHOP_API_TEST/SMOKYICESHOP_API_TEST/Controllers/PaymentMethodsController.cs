using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly PaymentMethodsModel _paymentMethodsModel;

        public PaymentMethodsController(PaymentMethodsModel paymentMethodsModel)
        {
            _paymentMethodsModel = paymentMethodsModel;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<PaymentMethodDTO>> GetAllDeliveryMethods()
        {
            return Ok(_paymentMethodsModel.GetPaymentMethods());
        }
    }
}
