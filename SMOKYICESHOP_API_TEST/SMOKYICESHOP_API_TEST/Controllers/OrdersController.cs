using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.Orders;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly OrdersModel _ordersModel;
        private readonly AuthenticationService _authenticationService;
        private readonly SendToBotService _sendToBotService;

        public OrdersController(OrdersModel ordersModel, AuthenticationService authenticationService, SendToBotService sendToBotService)
        {
            _ordersModel = ordersModel;
            _authenticationService = authenticationService;
            _sendToBotService = sendToBotService;
        }

        [HttpGet("by-status/{statusId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<OrderDTO>> GetOrdersByStatusName(Guid statusId)
        {
            return Ok(_ordersModel.GetOrdersByStatusId(statusId));
        }

        [HttpPatch("{orderID}")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeOrderStatus(Guid orderID, [FromBody] Guid statusId)
        {
            try
            {
                _ordersModel.UpdateOrderStatus(orderID, statusId);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public ActionResult<OrderDTO> GetOrderById(Guid orderId)
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            OrderDTO order = _ordersModel.GetOrder(orderId);

            if (order.ClientID == clientID || _authenticationService.IsAdmin(clientID))
            {
                return Ok(order);
            }

            return Forbid();
        }

        [HttpGet("history")]
        [Authorize]
        public ActionResult<IEnumerable<OrderDTO>> GetOrderHistory()
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            return Ok(_ordersModel.GetOrderHistory(clientID));
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult CreateOrder([FromBody] CreateOrderDTO orderCE)
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);

            try
            {
                Guid orderID = _ordersModel.CreateOrder(orderCE, clientID);
                OrderDTO order = _ordersModel.GetOrder(orderID);
                _sendToBotService.SendOrderToApi(order);
                return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }

        [HttpPost("create-withoutaccount")]
        public IActionResult CreateOrder([FromBody] CreateOrderWithoutClientDTO orderCE)
        {
            try
            {
                Guid orderID = _ordersModel.CreateOrder(orderCE);
                OrderDTO order = _ordersModel.GetOrder(orderID);
                _sendToBotService.SendOrderToApi(order);
                return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }
    }
}
