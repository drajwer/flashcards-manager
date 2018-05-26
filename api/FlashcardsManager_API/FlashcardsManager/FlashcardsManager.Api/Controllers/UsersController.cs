using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Api.Filters;
using FlashcardsManager.Api.Services;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FlashcardsManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserServices _services;
        private readonly FilteringServices _filteringServices;
        private readonly UserManager<User> _userManager;
        private readonly RoleNamesOptions _roleNamesOptions;

        public UsersController(UserServices services, FilteringServices filteringServices, UserManager<User> userManager, IOptions<RoleNamesOptions> roleNamesOptions)
        {
            _services = services;
            _filteringServices = filteringServices;
            _userManager = userManager;
            _roleNamesOptions = roleNamesOptions.Value;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Array of users</returns>
        /// <response code="200">Returns array of users.</response>
        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(typeof(UserDto[]), 200)]
        public IActionResult Get()
        {
            var query = _services.Get().Select(u => new UserDto(u.Name, u.Surname, u.UserName, new Score(u.UserProgress.Sum(up => up.Progress))));
            return new JsonResult(query.ToList());
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <returns>User with given id</returns>
        /// <response code="200">Returns user with given id.</response>
        /// <response code="404">If there is no user with given id.</response>
        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public IActionResult Get(string id)
        {
            var user = _services.Get(id);
            if (user == null)
                return NotFound();
            return new JsonResult(new UserDto(user.Name, user.Surname, user.UserName, new Score(user.UserProgress.Sum(up => up.Progress))));
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <response code="200">Entity was successfully updated.</response>
        /// <response code="400">If model is invalid.</response>
        /// <response code="404">If there is no user with specified id.</response>
        // PUT: api/Users/5
        [HttpPut("{id}")]
        [ValidateModelState]
        public async Task<IActionResult> Put(string id, [FromBody]UserDto userToUpdate)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            if (await _services.Update(id, userToUpdate, userId, isAdmin))
                return NotFound();
            return Ok();
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <response code="200">User was successfully deleted.</response>
        /// <response code="404">If there is no user with specified id.</response>
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            if (await _services.Delete(id, userId, isAdmin)) return Ok();
            return NotFound();
        }

        /// <summary>
        /// Get filtered and sorted users.
        /// </summary>
        /// <response code="200">Filtering criteria were applied correctly.</response>
        /// <response code="400">If filtering criteria are invalid.</response>
        // POST: api/Users/filter
        [HttpPost("filter")]
        [ValidateModelState]
        [ProducesResponseType(typeof(UserDto[]), 200)]
        public IActionResult Filter([FromBody] UsersFilteringModel model)
        {
            var query = _filteringServices.Filter(_services.Get(), model);
            var list = query.ToList();

            return new JsonResult(list);
        }
    }
}
