using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Beta.Core.Repositories
{
    public class EntityRepository : IRepository<Entity>
	{
		private readonly ApplicationDbContext _context;

		public EntityRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Entity> GetByIdAsync(Guid id)
		{
			return await _context.Entity.FindAsync(id);
		}

		public async Task<IEnumerable<Entity>> GetAllAsync()
		{
			return await _context.Entity.ToListAsync();
		}

        public async Task<Entity> AddAsync(Entity entity)
        {
            await _context.Entity.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Entity> UpdateAsync(Entity entity)
        {
            _context.Entity.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public async Task DeleteAsync(Guid id)
		{
			var user = await _context.Entity.FindAsync(id);
			if (user != null)
			{
				_context.Entity.Remove(user);
				await _context.SaveChangesAsync();
			}
		}
	}
}