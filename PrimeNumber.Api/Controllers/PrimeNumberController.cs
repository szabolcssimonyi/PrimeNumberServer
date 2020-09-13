using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeNumber.Api.Filter;
using PrimeNumber.Extensibility.Dto;
using PrimeNumber.Extensibility.Interfaces;
using System.Threading.Tasks;

namespace PrimeNumber.Api.Controllers
{
    /// <summary>
    /// Controller for prime number calculation
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PrimeNumberController : ControllerBase
    {
        private readonly IPrimeNumberCacheProxyService primeNumberCacheProxyService;

        public PrimeNumberController(IPrimeNumberCacheProxyService primeNumberCacheProxyService)
        {
            this.primeNumberCacheProxyService = primeNumberCacheProxyService;
        }

        /// <summary>
        /// Evaluates if test value is a prime number
        /// </summary>
        /// <param name="value">Test number</param>
        /// <returns>200 Ok, 400 Bad request, or 500 Internal server error</returns>
        /// <response code="200">Returns the evaluated value</response>
        /// <response code="400">If the item is null, out of server range</response>
        /// <response code="500">At internal server error</response>
        [HttpGet("isprime/{value:int}")]
        [RangeValidationFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IsPrimeNumberDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsPrimeNumber(int value)
        {
            return Ok(new IsPrimeNumberDto
            {
                IsPrimeNumber = await primeNumberCacheProxyService.IsPrimeNumberAsync(value),
                TestNumber = value
            });
        }

        /// <summary>
        /// Search for next prime number greater than test number
        /// </summary>
        /// <param name="value">Test number</param>
        /// <returns>200 Ok, 400 Bad request, or 500 Internal server error</returns>
        /// <response code="200">Returns the evaluated value</response>
        /// <response code="400">If the item is null or test number is out of server range, or next prime number generates out of range error</response>
        /// <response code="500">At internal server error</response>
        [HttpGet("getnext/{value:int}")]
        [RangeValidationFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NextPrimeNumberDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> NextPrimeNumber(int value)
        {
            return Ok(new NextPrimeNumberDto
            {
                NextPrimeNumber = await primeNumberCacheProxyService.GetNextPrimeNumberAsync(value),
                TestNumber = value
            });
        }
    }
}
