namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    /// <summary>
    /// Provides transactional boundaries for database operations.
    /// Encapsulates EF Core transaction management to maintain clean architecture.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Executes an action within a database transaction.
        /// Automatically commits on success, rolls back on exception.
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
