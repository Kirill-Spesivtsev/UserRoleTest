using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserRoleTest.Interfaces;
using UserRoleTest.Middleware;
using UserRoleTest.Models;

namespace UserRoleTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public AuthController(IConfiguration configuration,IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        /// <summary>
        /// Аутентификация с генерацией JWT-токена
        /// </summary>
        /// <remarks>
        /// Для успешной аутентификации необходима предварительная регистрация.
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Account data)
        {
            bool isValid = await _authService.ValidateUserCredentials(data);
            if (isValid)
            {
                var tokenString = GenerateJwtToken(data.UserName);
                return Ok(new { Token = tokenString, Message = "Success" });
            }
            return BadRequest("Account credentials are not valid");
        }


        /// <summary>
        ///Регистрация аккаунта
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Account data)
        {
            
            var alreadyExists = await _authService.CheckAccountExistsence(data.UserName);
            if (!alreadyExists)
            {
                var result = await _authService.AddAccount(data);
                if (result != 0)
                {
                    return Ok(new { Message = "Success" });
                }

            }
            return BadRequest("Unable to register account");
        }

        /// <summary>
        /// Метод для проверки работы авторизации
        /// </summary>
        /// <remarks>
        /// Предполагается, что любой пользовать с существующим аккаунтом обладает необходимыми правами.
        /// Среди всех методов в приложении, аутентификация необходима только для этого.
        /// </remarks>
        /// <returns></returns>
        [AuthorizeJWT]
        [HttpGet]
        [Route("TestAuthorization")]
        public async Task<IActionResult> TestAuthorization()
        {
            return Ok("User is Authorised for the API");
        }

        /// <summary>
        /// Выдача JWT-токена при успешной аутентификации
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private async Task<string> GenerateJwtToken(string userName)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtOrigin:Key"]);
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userName) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JwtOrigin:Issuer"],
                Audience = _configuration["JwtOrigin:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

    }
}
