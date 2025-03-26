namespace Boilerplate.Beta.Core.Infrastructure
{
    public class SwaggerSettings
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public ContactSettings Contact { get; set; }
    }

    public class ContactSettings
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }
}
