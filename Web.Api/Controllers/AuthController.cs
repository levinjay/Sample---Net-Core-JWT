using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Web.Api.Data;
using Web.Api.Model;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration _config { get; }
        private readonly UserContext _context;

        public AuthController(UserContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("token")]
        public IActionResult Token()
        {
            var header = Request.Headers["Authorization"];
            if(header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic ".Length).Trim();
                var credEnc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));
                //"email:password"
                var credArray = credEnc.Split(":");

                string tokenExpireMinute = _config["TokenSettings:ExpireInMinute"];

                User users = _context.Users.FromSql("EXECUTE spGetUser @email = {0}", credArray[0]).SingleOrDefault();

                if(users != null)
                {
                    if (credArray[0] == users.Email && credArray[1] == users.Password)
                    {
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdlfasjdfl13123asdf"));
                        var claimsData = new[] { new Claim(ClaimTypes.Name, "username") };
                        var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                        var token = new JwtSecurityToken(
                                issuer: "website.com",
                                audience: "website.com",
                                expires: DateTime.UtcNow.AddMinutes(double.Parse(tokenExpireMinute)),
                                notBefore: DateTime.UtcNow,
                                claims: claimsData,
                                signingCredentials: signInCred
                            );
                        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(tokenStr);

                    }
                }
            }


            return BadRequest("Oops! something went wrong!");
        }



    }
}