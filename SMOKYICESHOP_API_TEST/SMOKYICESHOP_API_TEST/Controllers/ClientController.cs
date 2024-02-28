using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using SMOKYICESHOP_API_TEST.DTO.Cart;
using SMOKYICESHOP_API_TEST.DTO.Orders;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;
using System.Security.Claims;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientsModel _clientsModel;

        private readonly AuthenticationService _authenticationService;

        public ClientController(ClientsModel clientsModel, AuthenticationService authenticationService)
        {
            _clientsModel = clientsModel;
            _authenticationService = authenticationService;
        }

        [HttpGet("data")]
        public ActionResult<ClientDataDTO> GetClientData()
        {
            Guid clientID = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            return Ok(_clientsModel.GetClientData(clientID));
        }
    }
}
