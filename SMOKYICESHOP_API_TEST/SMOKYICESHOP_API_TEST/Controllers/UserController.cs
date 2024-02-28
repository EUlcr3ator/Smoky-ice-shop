using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMOKYICESHOP_API_TEST.DTO.Authentication;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Services;
using System.Security.Claims;

namespace SMOKYICESHOP_API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AuthenticationService _authenticationService;
        private readonly UserModel _userModel;

        public UserController(AuthenticationService authenticationService, UserModel userModel)
        {
            _authenticationService = authenticationService;
            _userModel = userModel;
        }

        [HttpPost("login")]
        public ActionResult<AuthenticatedDTO> Login([FromBody] LoginDTO login)
        {
            try
            {
                return Ok(_authenticationService.AuthenticateAndGenrateToken(login));
            }
            catch (InvalidOperationException)
            {
                return NotFound("User not Found");
            }
        }

        [HttpPost("telegram-login")]
        public ActionResult<AuthenticatedDTO> Login([FromBody] TelegramLoginDTO login)
        {
            try
            {
                return Ok(_authenticationService.AuthenticateAndGenrateToken(login));
            }
            catch (InvalidOperationException)
            {
                return NotFound("User not Found");
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginDTO login)
        {
            bool registred = _authenticationService.RegisterClient(login);

            if (registred)
            {
                return Ok(_authenticationService.AuthenticateAndGenrateToken(login));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("telegram-register")]
        public IActionResult Register([FromBody] TelegramRegistrationDTO register)
        {
            bool registred = _authenticationService.RegisterClient(register);

            if (registred)
            {
                return Ok("User registred");
            }
            else
            {
                return BadRequest("User not registred, already using number");
            }
        }

        [HttpPost("refresh")]
        public ActionResult<AuthenticatedDTO> Refresh([FromBody] AuthenticatedDTO authenticated)
        {
            try
            {
                return Ok(_authenticationService.RefreshToken(authenticated));
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Invalid client request");
            }
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public ActionResult<bool> Revoke()
        {
            Guid clientId = _authenticationService.GetClientsIdFromToken(HttpContext.User.Identity as ClaimsIdentity);
            return Ok(_authenticationService.RevokeToken(clientId));
        }

        [HttpGet("has-phone-number/{number}")]
        public ActionResult<bool> HasPhoneNumber(string number)
        {
            return  Ok(_userModel.HasPhoneNumber(number));
        }
    }
}
