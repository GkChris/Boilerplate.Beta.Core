namespace Boilerplate.Beta.Core.Infrastructure
{
    public sealed class SwaggerSettings
    {
        public required string Title { get; init; }
        public required string Version { get; init; }
        public string? Description { get; init; }
        public ContactSettings? Contact { get; init; }
    }

    public sealed class ContactSettings
    {
        public string? Name { get; init; }
        public string? Email { get; init; }
        public Uri? Url { get; init; }
    }
}
