using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using accomondationApp.AuthModel;
using accomondationApp.Interfaces;
using accomondationApp.Utilities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

namespace accomondationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext userContext;
        private readonly ITokenService tokenService;
        public AuthController(UserContext _userContext, ITokenService _tokenService)
        {
            userContext = _userContext ?? throw new ArgumentNullException(nameof(userContext));
            tokenService = _tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }       

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Invalid client request");
            }
            var user = userContext.LoginModels.FirstOrDefault(u => u.UserName == loginModel.UserName &&
            u.Password == EncryptDecrypt.EncodePasswordToBase64(loginModel.Password));
            if(user is not LoginModel)
            {
                return Unauthorized();
            }
            var userInRoles = userContext.UserInRoles.Where(ur => ur.User.UserName == loginModel.UserName);
            var roles = (from ur in userInRoles
                         join r in userContext.Roles on ur.RoleId equals r.RoleId
                        into usersRole
                         from ru in usersRole.DefaultIfEmpty()
                         select ru.Role1).ToList();
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, loginModel.UserName) };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            userContext.SaveChanges();
            return Ok(new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });           
        }
    }
}
