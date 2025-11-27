using AutismEdu.API.Models;

namespace AutismEdu.API.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        Task<int> SaveChangesAsync();
    }
}
