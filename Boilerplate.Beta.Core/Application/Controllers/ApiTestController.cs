using Boilerplate.Beta.Core.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApiTestController : Controller
	{
		public ApiTestController()
		{
		}

		[HttpGet("test-success")]
		public async Task<IActionResult> TestSuccess()
		{
			return Ok();
		}

		[HttpGet("test-404")]
		public async Task<IActionResult> Test404()
		{
			return NotFound();
		}

		[HttpGet("test-error")]
		public async Task<IActionResult> TestError()
		{
			throw new NotImplementedException();
		}

		[HttpGet("test-custom-404")]
		public async Task<IActionResult> TestCustom404()
		{
			throw new ResourceNotFoundException("Test warning message for developers");
		}

		[HttpGet("test-custom-error")]
		public async Task<IActionResult> TestCustomError()
		{
			throw new AddCustomErrorException("Test error message for developers");
		}
	}
}
