using Microsoft.AspNetCore.Mvc;
using UserRoleTest.Interfaces;
using UserRoleTest.Models;

namespace UserRoleTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                if (users == null)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(int? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody]User user)
        {

            if (user != null)
            {
                try
                {
                    var users = await _userService.GetAllUsers();
                    if (users.Any(q => q.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "Email should be unique");
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var userId = await _userService.AddUser(user);

                    if (userId > 0)
                    {
                        return Ok(userId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int? userId)
        {

            if (userId == null)
            {
                return BadRequest();
            }

            try
            {
                int result = await _userService.DeleteUser(userId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int? userId, [FromBody]User user)
        {
            if (userId != null)
            {
                try
                {
                    var users = await _userService.GetAllUsers();
                    if (users.Any(q => q.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "Email should be unique");
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var code = await _userService.UpdateUser(userId, user);
                    if (code == 0)
                    {
                        return NotFound();
                        
                    }
                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }
    }
}
