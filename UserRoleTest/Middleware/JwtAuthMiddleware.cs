using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserRoleTest.Interfaces;

namespace UserRoleTest.Middleware
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtAuthMiddleware> _logger;

        public JwtAuthMiddleware(
            RequestDelegate next, 
            IConfiguration configuration, 
            ILogger<JwtAuthMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Валидация JWT-токена при запросе
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context != null)
            {
                var token = context?.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    attachAccountToContext(context, token);
                }
                
            }

            await _next(context);
        }

        /// <summary>
        /// Закрепление токена за контекстом
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void attachAccountToContext(HttpContext context, string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtOrigin:Key"]);

                handler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _configuration["JwtOrigin:Issuer"],
                    ValidAudience = _configuration["JwtOrigin:Audience"]
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                context.Items["User"] = userId;
            }
            catch(Exception ex)
            {
                _logger.LogWarning($"Auth failed.\n {ex.Message}");
            }
        }
    }
}
