using AutismEdu.API.Models;
using System.Linq.Expressions;

namespace AutismEdu.API.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> GetAllAsync(bool trackChanges = false);
        Task<TEntity?> GetByIdAsync(Guid id);



        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    }
}
