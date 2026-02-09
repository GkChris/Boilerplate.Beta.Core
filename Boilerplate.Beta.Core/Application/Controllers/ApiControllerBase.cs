using Boilerplate.Beta.Core.Application.Mappers;
using Boilerplate.Beta.Core.Application.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult MapException(Exception ex)
        {
            var (statusCode, message, friendlyMessage, typeName) = ExceptionMapper.MapToHttpResponse(ex);

            var response = new ApiErrorResponse
            {
                Error = new ApiErrorDetail
                {
                    Type = typeName,
                    Message = string.IsNullOrWhiteSpace(message) ? friendlyMessage : message,
                    FriendlyMessage = friendlyMessage
                }
            };

            return StatusCode(statusCode, response);
        }
    }
}
