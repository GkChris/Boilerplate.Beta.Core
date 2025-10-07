namespace Boilerplate.Beta.Core.Application.Models.DTOs
{
    public class ApiErrorResponse
    {
        public ApiErrorDetail Error { get; set; }
    }

    public class ApiErrorDetail
    {
        public string Type { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }
    }

}
