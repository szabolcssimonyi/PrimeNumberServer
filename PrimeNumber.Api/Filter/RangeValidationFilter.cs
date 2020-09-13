using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PrimeNumber.Extensibility;

namespace PrimeNumber.Api.Filter
{
    public class RangeValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var options = context.HttpContext.RequestServices.GetService<IOptions<Configuration>>();
            var configuration = options.Value;
            var value = context.ActionArguments["value"] as int?;
            if (value == null)
            {
                context.Result = new BadRequestObjectResult("Missing input");
            }
            else if (configuration.Range.Min > value || value > configuration.Range.Max)
            {
                var min = configuration.Range.Min;
                var max = configuration.Range.Max;
                context.Result = new BadRequestObjectResult($@"input out of range, it must be between {min} and {max}");
            }
        }
    }
}
