using AuthenticationRepository;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    [Route("api/facebook")]
    [ApiController]
    //[Produces("application/json")]

    public class FacebookController : ControllerBase
    {
        private readonly IFacebookRepository _facebookRepository;
        private readonly IConfiguration _configuration;
        public FacebookController(IFacebookRepository facebookRepository, IConfiguration configuration)
        {
            _facebookRepository = facebookRepository;
            _configuration = configuration;
        }

        [HttpGet]
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
        public IActionResult Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = generateJwtToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }


        private User AuthenticateUser(User login)
        {
            User user = null;
            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.UserName == "manhdt")
            {
                user = new User { UserName = "Jignesh Trivedi", Password = "123456" };
            }
            return user;
        }
        private string generateJwtToken(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
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
