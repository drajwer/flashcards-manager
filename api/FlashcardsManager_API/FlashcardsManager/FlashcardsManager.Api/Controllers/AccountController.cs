using FlashcardsManager.Api.Filters;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlashcardsManager.Api.Controllers
{
    [Route("api/[controller]/")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Register a user.
        /// </summary>
        /// <response code="200">User registered successfully.</response>
        /// <response code="302">If model was invalid.</response>
        // GET api/account/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username, Name = model.Name, Surname = model.Surname };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}
