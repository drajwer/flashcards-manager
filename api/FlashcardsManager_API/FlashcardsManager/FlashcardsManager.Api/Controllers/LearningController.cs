using System;
using System.Threading.Tasks;
using FlashcardsManager.Api.Filters;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FlashcardsManager.Core.ViewModels;

namespace FlashcardsManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LearningController : Controller
    {
        private readonly ILearningService _service;
        private readonly UserManager<User> _userManager;
        private async Task<User> GetCurrentUser() => await _userManager.GetUserAsync(User);


        public LearningController(ILearningService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        /// <summary>
        /// Get flashcards to learn.
        /// </summary>
        /// <returns>Array of flashcards</returns>
        /// <response code="200">
        /// Returns array of flashcards from specified
        /// category depending on learning strategy.</response>
        /// <param name="categoryId">Id of flashcards category.</param>
        /// <param name="strategy">Learning strategy</param>
        /// <param name="count">Max number of result flashcards.</param>
        /// <remarks>
        ///     Strategies:
        ///     0) All flashcards
        ///     1) Only flashcards not seen by user
        ///     2) All flashcard seen by user
        ///     3) Only flashcards user had problem with
        ///     4) Only flashcards mastered by user
        /// </remarks>
        // GET api/learning/flashcards
        [HttpGet("flashcards/{categoryId}")]
        [ValidateModelState]
        [ProducesResponseType(typeof(Flashcard[]), 200)]
        public async Task<IActionResult> GetFlashcards([FromRoute] int categoryId, [FromQuery] FlashcardsSearchCriterionEnum strategy, [FromQuery] int count = 10)
        {
            try
            {
                var flashcards = await _service.GetFlashcards(await GetCurrentUser(), categoryId, strategy, count);
                return Json(flashcards);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get user's whole score.
        /// </summary>
        /// <returns>Score</returns>
        /// <response code="200">Returns score.</response>
        // GET api/learning/score
        [HttpGet("score")]
        [ProducesResponseType(typeof(Score), 200)]
        public async Task<IActionResult> GetScoreAsync()
        {
            var score = _service.GetUserScore(await GetCurrentUser());
            return Json(score);
        }

        /// <summary>
        /// Get user's score for specified category.
        /// </summary>
        /// <response code="200">Entity was updated</response>
        /// <response code="400">If model is invalid.</response>
        // GET api/learning/score/{categoryId}
        [HttpGet("score/{categoryId}")]
        [ProducesResponseType(typeof(Score), 200)]
        public async Task<IActionResult> GetScoreForCategory(int categoryId)
        {
            try
            {
                var score = await _service.GetUserScoreForCategory(await GetCurrentUser(), categoryId);
                return Json(score);
            }
            catch (ArgumentException e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Post result for flashcard.
        /// </summary>
        /// <response code="200">Result was proceeded</response>
        /// <response code="400">If model is invalid.</response>
        // POST api/learning/result
        [HttpPost("result")]
        [ValidateModelState]
        public async Task<IActionResult> PostResult([FromBody] FlashcardResultViewModel dto)
        {
            try
            {
                await _service.ProceedFlashcard(dto.FlashcardId, await GetCurrentUser(), dto.Result);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest();
            }
        }

    }
}
