using Boilerplate.Beta.Core.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiSuccessTestController : Controller
    {
        public ApiSuccessTestController()
        {
        }

        [HttpGet("test-success")]
        public async Task<IActionResult> TestSuccess()
        {
            return Ok();
        }
    }
}
