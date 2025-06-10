using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProtectedController : ControllerBase
	{
		[HttpGet("public")]
		public IActionResult Public() => Ok("Anyone can access this");

		[Authorize]
		[HttpGet("protected")]
		public IActionResult Protected() => Ok($"Hello {User.Identity?.Name}, you are authenticated!");

		[Authorize(Roles = "admin")]
		[HttpGet("admin")]
		public IActionResult AdminOnly() => Ok("You have admin access.");
	}
}
