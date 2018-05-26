using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Api.Filters;
using FlashcardsManager.Api.Services;
using FlashcardsManager.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using FlashcardsManager.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Options;
using Microsoft.Extensions.Options;

namespace FlashcardsManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FlashcardsController : Controller
    {
        private readonly FlashcardsServices _services;
        private readonly FilteringServices _filteringServices;
        private readonly UserManager<User> _userManager;
        private readonly RoleNamesOptions _roleNamesOptions;

        public FlashcardsController(FlashcardsServices services, FilteringServices filteringServices, UserManager<User> userManager, IOptions<RoleNamesOptions> roleNamesOptions)
        {
            _services = services;
            _filteringServices = filteringServices;
            _userManager = userManager;
            _roleNamesOptions = roleNamesOptions.Value;
        }

        /// <summary>
        /// Get all flashcards.
        /// </summary>
        /// <returns>Array of flashcards</returns>
        /// <response code="200">Returns array of flashcards.</response>
        // GET: api/Flashcards
        [HttpGet]
        [ProducesResponseType(typeof(FlashcardDto[]), 200)]
        public IActionResult Get()
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            return Json(_services.Get(userId, isAdmin).Select(f => new FlashcardDto(f)).ToList());
        }

        /// <summary>
        /// Get flashcard by id.
        /// </summary>
        /// <returns>Flashcard with given id.</returns>
        /// <response code="200">Returns flashcard with given id.</response>
        /// <response code="404">If there is no flashcard with given id.</response>
        // GET: api/Flashcards/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlashcardDto), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            var flashcard = await _services.Get(id, userId, isAdmin);
            if (flashcard == null)
                return NotFound();
            return new JsonResult(flashcard);
        }

        /// <summary>
        /// Get flashcards of category specified by id.
        /// </summary>
        /// <returns>Array of flashcards</returns>
        /// <response code="200">Returns array of flashcards.</response>
        // GET: api/Flashcards/category/5
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(typeof(FlashcardDto), 200)]
        public IActionResult GetFlashcardsOfCategory(int categoryId)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            return Json(_services.GetFlashcardsOfCategory(categoryId, userId, isAdmin).Select(f => new FlashcardDto(f)).ToList());
        }

        /// <summary>
        /// Add flashcard.
        /// </summary>
        /// <returns>Created flashcard.</returns>
        /// <response code="201">Returns newly created flashcard.</response>
        /// <response code="400">If model is invalid.</response>
        // POST: api/Flashcards
        [HttpPost]
        [ValidateModelState]
        [ProducesResponseType(typeof(FlashcardDto), 201)]
        public async Task<IActionResult> Post([FromBody]FlashcardDto flashcardDto)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            var flashcard = await _services.Post(flashcardDto, userId, isAdmin);
            return new JsonResult(new FlashcardDto(flashcard));
        }

        /// <summary>
        /// Delete flashcard.
        /// </summary>
        /// <response code="200">Flashcard was successfully deleted.</response>
        /// <response code="404">If there is no flashcard with specified id.</response>
        // DELETE: api/flashcards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            if (await _services.Delete(id, userId, isAdmin)) return Ok();
            return NotFound();
        }

        /// <summary>
        /// Get filtered and sorted flashcards.
        /// </summary>
        /// <response code="200">Filtering criteria were applied correctly.</response>
        /// <response code="400">If filtering criteria are invalid.</response>
        // POST: api/Users/filter
        [HttpPost("filter")]
        [ValidateModelState]
        [ProducesResponseType(typeof(FlashcardDto[]), 200)]
        public IActionResult Filter([FromBody] FlashcardsFilteringModel model)
        {
            var userId = _userManager.GetUserId(this.User);
            var isAdmin = this.User.IsInRole(_roleNamesOptions.AdminRoleName);
            var query = _filteringServices.Filter(_services.Get(userId, isAdmin), model);
            var list = query.ToList();

            return new JsonResult(list);
        }
    }
}
