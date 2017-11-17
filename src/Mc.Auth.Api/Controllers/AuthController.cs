using System.Threading.Tasks;
using Mc.Auth.Api.Model;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = Mc.Auth.Core.Interfaces.IAuthorizationService;

namespace Mc.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthorizationService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUserService _userService;

        public AuthController(IAuthorizationService authService, ITokenProvider tokenProvider, IUserService userService)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _userService = userService;
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]AuthRequest authRequest)
        {
            if (authRequest == null)
                return BadRequest("Could not create token");

            var checkPwd = await _authService.CheckPasswordAsync(authRequest.Password, authRequest.UserName);
            if (!checkPwd)
                return BadRequest("Could not create token");

            var token = _tokenProvider.GetToken();
            return Ok(new { token });
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]AuthRequest authRequest)
        {
            if (authRequest == null)
                return BadRequest("Could not register user");

            await _userService.AddUserAsync(new User
            {
                Password = authRequest.Password,
                UserName = authRequest.UserName
            });

            var token = _tokenProvider.GetToken();
            return Ok(new { token });
        }
    }
}
