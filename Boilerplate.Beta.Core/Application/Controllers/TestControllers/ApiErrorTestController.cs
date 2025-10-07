using Boilerplate.Beta.Core.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiErrorTestController : Controller
    {
        public ApiErrorTestController()
        {
        }

        [HttpGet("test-404")]
        public async Task<IActionResult> Test404()
        {
            return NotFound();
        }

        [HttpGet("test-non-implemented-exception")]
        public async Task<IActionResult> TestError()
        {
            throw new NotImplementedException();
        }

        [HttpGet("test-custom-resource-not-found-exception")]
        public async Task<IActionResult> TestCustom404()
        {
            throw new ResourceNotFoundException("Test warning message for developers");
        }

        [HttpGet("test-custom-add-custom-exception")]
        public async Task<IActionResult> TestCustomError()
        {
            throw new AddCustomException("Test error message for developers");
        }
    }
}
