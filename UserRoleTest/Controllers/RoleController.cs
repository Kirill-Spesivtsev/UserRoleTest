using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserRoleTest.Helpers;
using UserRoleTest.Interfaces;

namespace UserRoleTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
     private readonly IRoleService _roleService;
        private readonly ILogger<UserController> _logger;

        public RoleController(IRoleService roleService, ILogger<UserController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Получение всех ролей с фильтрацией
        /// </summary>
        /// <param name="pagingOptions">Параметры пагинации</param>
        /// <param name="filterOptions">Фильтры</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetRoles(
            [FromQuery] PaginationOptions pagingOptions, 
            [FromQuery] RolesFilteringOptions filterOptions,
            [FromQuery] RolesSortingHelper sortingOptions)
        {
            try
            {
                var pagingFilter = new PaginationOptions(pagingOptions.PageNumber, pagingOptions.PageSize);


                var filteredRoles = await _roleService.GetAllRolesFiltered(pagingOptions, filterOptions, sortingOptions);


                if (filteredRoles == null)
                {
                    return NotFound();
                }

                return Ok(filteredRoles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Получение роли по ID
        /// </summary>
        /// <param name="roleId">ID роли</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка Запроса API</response>
        /// <response code="404">Ресурс не найден</response>
        [HttpGet]
        [Route("GetRole")]
        public async Task<IActionResult> GetRole(int? roleId)
        {
            if (roleId == null)
            {
                return BadRequest();
            }

            try
            {
                var user = await _roleService.GetRoleById(roleId);

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


    } 
}
