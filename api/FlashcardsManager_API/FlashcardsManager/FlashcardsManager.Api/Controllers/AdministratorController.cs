using FlashcardsManager.Api.Services;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardsManager.Api.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    public class AdministratorController : Controller
    {
        private readonly AdministratorServices _administratorServices;

        public AdministratorController(AdministratorServices administratorServices)
        {
            _administratorServices = administratorServices;
        }

        [HttpGet("categories")]
        public IActionResult GetPendingCategories()
        {
            return new JsonResult(_administratorServices.GetPendingCategories().ToList());
        }

        [HttpPut("confirm/{id}")]
        public async Task<IActionResult> ConfirmCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (await _administratorServices.ChangeCategoryAvailability(id, AvailabilityEnum.Public))
                return Ok();
            return BadRequest();
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (await _administratorServices.ChangeCategoryAvailability(id, AvailabilityEnum.Private))
                return Ok();
            return BadRequest();
        }
    }
}
