using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Mc.Auth.Api.Model;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using IAuthorizationService = Mc.Auth.Core.Interfaces.IAuthorizationService;

namespace Mc.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _signService;
        private readonly AuthorizationSettings _authSettings;

        public AuthController(AuthorizationSettings authSettings, IUserService userService, IAuthorizationService signService)
        {
            _userService = userService;
            _signService = signService;
            _authSettings = authSettings;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            var user = await _userService.FindByEmailAsync(authRequest.UserName);
            if (user == null) return BadRequest("Could not create token");

            var checkPwd = _signService.CheckPassword(authRequest.Password, authRequest.UserName);
            if (!checkPwd) return BadRequest("Could not create token");

            var jwsToken = new JwtSecurityToken(_authSettings.Issuer,
                _authSettings.Audience,
                expires: DateTime.Now.AddMinutes(_authSettings.Expires),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey)), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwsToken);
            return Ok(new { token });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
