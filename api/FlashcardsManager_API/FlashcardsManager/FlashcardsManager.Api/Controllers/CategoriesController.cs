using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Api.Filters;
using FlashcardsManager.Api.Services;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Options;
using Microsoft.Extensions.Options;

namespace FlashcardsManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly CategoryServices _services;
        private readonly FilteringServices _filteringServices;
        private readonly UserManager<User> _userManager;
        private readonly RoleNamesOptions _roleNamesOptions;

        public CategoriesController(CategoryServices services, FilteringServices filteringServices, UserManager<User> userManager, IOptions<RoleNamesOptions> roleNamesOptions)
        {
            _services = services;
            _filteringServices = filteringServices;
            _userManager = userManager;
            _roleNamesOptions = roleNamesOptions.Value;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>Array of categories.</returns>
        /// <response code="200">Returns array of categories.</response>
        // GET api/categories
        [HttpGet]
        [ProducesResponseType(typeof(CategoryDto[]), 200)]
        public IActionResult Get()
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            var list = _services.Get(userId, isAdmin).Select(c => new CategoryDto(c)).ToList();
            return Json(list);
        }

        /// <summary>
        /// Get categories owned by current user.
        /// </summary>
        /// <returns>Array of categories.</returns>
        /// <response code="200">Returns array of categories.</response>
        // GET api/categories
        [HttpGet("mine")]
        [ProducesResponseType(typeof(CategoryDto[]), 200)]
        public IActionResult GetUsersOwnCategories()
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            var list = _services.GetUsersOwnCategories(userId, isAdmin).Select(c => new CategoryDto(c)).ToList();
            return Json(list);
        }

        /// <summary>
        /// Get category by id.
        /// </summary>
        /// <returns>Category with given id.</returns>
        /// <response code="200">Returns category with given id.</response>
        /// <response code="404">If there is no category with given id.</response>
        // GET api/categories/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            var category = await _services.Get(id, userId, isAdmin);
            var users = await _userManager.GetUsersInRoleAsync("Administrator");

            if (category == null)
                return NotFound();
            return new JsonResult(new CategoryDto(category));
        }

        /// <summary>
        /// Add category.
        /// </summary>
        /// <returns>Created category.</returns>
        /// <response code="201">Returns newly created category.</response>
        /// <response code="400">If model is invalid.</response>
        // POST api/categories
        [HttpPost]
        [ValidateModelState]
        [ProducesResponseType(typeof(CategoryDto), 201)]
        public async Task<IActionResult> Post([FromBody]CategoryDto categoryDto)
        {
            var userId = _userManager.GetUserId(this.User);
            var category = await _services.Post(categoryDto, userId);
            return new JsonResult(new CategoryDto(category));
        }

        /// <summary>
        /// Update category.
        /// </summary>
        /// <response code="200">Entity was successfully updated.</response>
        /// <response code="400">If model is invalid.</response>
        /// <response code="404">If there is no category with specified id.</response>
        // PUT api/categories/5
        [HttpPut("{id}")]
        [ValidateModelState]
        public async Task<IActionResult> Put(int id, [FromBody]CategoryDto newCategory)
        {
            var userId = _userManager.GetUserId(this.User);
            if (await _services.Update(id, newCategory, userId))
                return Ok();
            return BadRequest();

        }

        /// <summary>
        /// Delete category.
        /// </summary>
        /// <response code="200">Category was successfully deleted.</response>
        /// <response code="404">If there is no category with specified id.</response>
        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            if (await _services.Delete(id, userId, isAdmin)) return Ok();
            return NotFound();
        }

        /// <summary>
        /// Get filtered and sorted categories.
        /// </summary>
        /// <response code="200">Filtering criteria were applied correctly.</response>
        /// <response code="400">If filtering criteria are invalid.</response>
        // POST: api/Users/filter
        [HttpPost("filter")]
        [ValidateModelState]
        [ProducesResponseType(typeof(CategoryDto[]), 200)]
        public IActionResult Filter([FromBody] CategoriesFilteringModel model)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            var query = _filteringServices.Filter(_services.Get(userId, isAdmin), model);
            var list = query.ToList();
            return new JsonResult(list);
        }
    }
}

