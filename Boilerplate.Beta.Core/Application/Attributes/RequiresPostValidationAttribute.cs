namespace Boilerplate.Beta.Core.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RequiresPostValidationAttribute : Attribute
    {
    }
}
