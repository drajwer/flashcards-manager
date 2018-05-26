using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FlashcardsManager.Api.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;

            if (!controller.ModelState.IsValid)
            {
                context.Result = new BadRequestResult();
            }
            base.OnActionExecuting(context);
        }
    }
}
