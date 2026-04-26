using AutismEdu.API.Contracts;
using AutismEdu.API.Data;
using AutismEdu.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutismEdu.API.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }





        public async Task CreateAsync(TEntity entity)
          => await _context.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.Now;
            _context.Set<TEntity>().Update(entity);
        }

        public IQueryable<TEntity> GetAllAsync(bool trackChanges = false)
        {
            var query = _context.Set<TEntity>()
               .Where(e => !e.IsDeleted)
               .AsQueryable();

            return trackChanges ? query : query.AsNoTracking();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity is not null && !entity.IsDeleted ? entity : null;
        }

        public void Update(TEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _context.Set<TEntity>().Update(entity);
        }


        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>()
                .AnyAsync(predicate);
        }

    }
}
