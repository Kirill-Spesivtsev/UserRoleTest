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

        /// <summary>
        /// Получение всех пользователей с фильтрацией
        /// </summary>
        /// <param name="pagingOptions">Параметры пагинации</param>
        /// <param name="filterOptions">Фильтры</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] PaginationOptions pagingOptions, 
            [FromQuery] UsersFilteringOptions filterOptions,
            [FromQuery] UsersSortingHelper sortingOptions)
        {
            try
            {
                var pagingFilter = new PaginationOptions(pagingOptions.PageNumber, pagingOptions.PageSize);


                var filteredUsers = await _userService
                    .GetAllUsersFiltered(pagingOptions, filterOptions, sortingOptions);


                return Ok(filteredUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Request '{HttpContext.Request?.Method} {HttpContext.Request?.Path.Value}' failed. \n");
                return BadRequest(new {errors = "Invalid request"});
            }
        }

        /// <summary>
        /// Получение пользователя по ID
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(int? userId)
        {
            
            if (userId == null)
            {
                return BadRequest(new {errors = "User id was not entered" });
            }

            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound(new {errors = "User was not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Request '{HttpContext.Request?.Method} {HttpContext.Request?.Path.Value}' failed. \n");
                return BadRequest(new {errors = "Invalid request" });
            }
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="user">Набор свойств пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody]User user)
        {

            if (user == null)
            {
                return BadRequest(new {errors = "User data was not entered"}); 
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
                    return BadRequest(new {errors = ModelState});
                }

                var userId = await _userService.AddUser(user);

                if (userId > 0)
                {
                    return Ok(userId);
                }

                return BadRequest(new {errors = "Invalid request"});

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Request '{HttpContext.Request?.Method} {HttpContext.Request?.Path.Value}' failed. \n");
                return BadRequest(new {errors = "Invalid request"});
            }

        }


        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int? userId)
        {

            if (userId == null)
            {
                return BadRequest(new {errors = "User id was not entered"});
            }

            try
            {
                int result = await _userService.DeleteUser(userId);
                if (result == 0)
                {
                    return NotFound(new {errors = "User was not found"});
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Request '{HttpContext.Request?.Method} {HttpContext.Request?.Path.Value}' failed. \n");
                return BadRequest(new {errors = "Invalid request"});
            }
        }

        /// <summary>
        /// Обновление пользователя по ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user">Набор новых свойств пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int? userId, [FromBody]User user)
        {
            if (userId == null)
            {
                return BadRequest(new {errors = "User id was not entered"});
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
                    return BadRequest(new {errors = ModelState});
                }

                var code = await _userService.UpdateUser(userId, user);
                if (code == 0)
                {
                    return NotFound(new {errors = "User was not found"});
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Request '{HttpContext.Request?.Method} {HttpContext.Request?.Path.Value}' failed. \n");
                return BadRequest(new {errors = "Invalid request"});
            }
        }

        /// <summary>
        /// Добавление пользователю новой роли
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="roleId">ID роли</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(int? userId, int? roleId)
        {
            if (userId == null || roleId == null)
            {
                return BadRequest(new {errors = "Invalid request"});
            }

            try
            {     
                var code = await _userService.AddUserToRole(userId, roleId);

                if (code > 0)
                {
                    return Ok();
                }
                return NotFound(new {errors = "User was not found"});
            }

            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Request '{HttpContext.Request?.Method} {HttpContext.Request?.Path.Value}' failed. \n");
                return BadRequest(new {errors = "Invalid request"});
            }

        }

    }
}
