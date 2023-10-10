using Microsoft.AspNetCore.Mvc;
using UserRoleTest.Helpers;
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
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationOptions paging, [FromQuery] UsersFilteringOptions filters)
        {
            try
            {
                var pagingFilter = new PaginationOptions(paging.PageNumber, paging.PageSize);


                var users = await _userService.GetAllUsers();

                var filtered = users
                    .Where( i => i.Id.ToString().ToUpper().Contains(filters.Id.ToUpper()))
                    .Where( n => n.Name.ToUpper().Contains(filters.Name.ToUpper()))
                    .Where( a => a.Age.ToString().ToUpper().Contains(filters.Age.ToUpper()))
                    .Where( e => e.Email.ToUpper().Contains(filters.Email.ToUpper()));

                var paged = filtered
                    .Skip((pagingFilter.PageNumber - 1) * pagingFilter.PageSize)
                    .Take(pagingFilter.PageSize).ToList();

                if (paged == null)
                {
                    return NotFound();
                }

                return Ok(paged);
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

            if (user == null)
            {
                return BadRequest(); 
            }

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

                return NotFound();

            }
            catch (Exception)
            {
                return BadRequest();
            }

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
            if (userId == null)
            {
                return BadRequest();
            }

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


        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(int? userId, int? roleId)
        {
            if (userId == null || roleId == null)
            {
                return BadRequest();
            }

            try
            {     
                var code = await _userService.AddUserToRole(userId, roleId);

                if (code > 0)
                {
                    return Ok();
                }
                return NotFound();
            }

            catch (Exception)
            {
                return BadRequest();
            }

        }

    }
}
