using AuthenticationRepository;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuthentication.Controller
{
    [Produces("application/json")]
    [Route("api/facebook")]
    public class FacebookController : ControllerBase
    {
        private readonly IFacebookRepository _facebookRepository;
        public FacebookController(IFacebookRepository facebookRepository)
        {
            _facebookRepository = facebookRepository;
        }

        [HttpPost]
        [Authorize]
        [Route("GetAllFacebook")]
        public IActionResult GetAllFacebook()
        {
            var lstFacebook = _facebookRepository.GetAllFacebook();
            return Ok(lstFacebook);
        }


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login(User user)
        {
            if(user.Password == "123456" && user.UserName == "manhdt")
            {
                //gọi đến đoạn lấy access token
            }
            return Ok();
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userName", user.UserName.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
