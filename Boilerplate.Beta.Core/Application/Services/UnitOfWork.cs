using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Data;

namespace Boilerplate.Beta.Core.Application.Services
{
    /// <summary>
    /// EF Core implementation of Unit of Work pattern.
    /// Provides transactional boundaries without exposing EF Core to service layer.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
