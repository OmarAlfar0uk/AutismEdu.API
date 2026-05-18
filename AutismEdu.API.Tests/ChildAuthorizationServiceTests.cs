using System.Linq.Expressions;
using System.Security.Claims;
using AutismEdu.API.Contracts;
using AutismEdu.API.Features.Auth.Authorization;
using AutismEdu.API.Models;
using Xunit;

namespace AutismEdu.API.Tests
{
    public class ChildAuthorizationServiceTests
    {
        [Fact]
        public void IsAuthorizedForChild_AllowsSpecialistForAnyChild()
        {
            var service = CreateService();
            var user = CreateUser(Guid.NewGuid(), "specialist");

            var allowed = service.IsAuthorizedForChild(Guid.NewGuid(), user);

            Assert.True(allowed);
        }

        [Fact]
        public void IsAuthorizedForChild_AllowsParentForOwnedChild()
        {
            var parentId = Guid.NewGuid();
            var childId = Guid.NewGuid();
            var service = CreateService(new ChildProfile { Id = childId, UserId = parentId, Name = "A", Age = 6 });
            var user = CreateUser(parentId, "parent");

            var allowed = service.IsAuthorizedForChild(childId, user);

            Assert.True(allowed);
        }

        [Fact]
        public void IsAuthorizedForChild_DeniesParentForAnotherParentsChild()
        {
            var childId = Guid.NewGuid();
            var service = CreateService(new ChildProfile { Id = childId, UserId = Guid.NewGuid(), Name = "A", Age = 6 });
            var user = CreateUser(Guid.NewGuid(), "parent");

            var allowed = service.IsAuthorizedForChild(childId, user);

            Assert.False(allowed);
        }

        [Fact]
        public void IsAuthorizedForChild_DeniesParentWithoutValidUserIdClaim()
        {
            var service = CreateService(new ChildProfile { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "A", Age = 6 });
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "parent") }, "test"));

            var allowed = service.IsAuthorizedForChild(Guid.NewGuid(), user);

            Assert.False(allowed);
        }

        private static ChildAuthorizationService CreateService(params ChildProfile[] children)
        {
            return new ChildAuthorizationService(new FakeUnitOfWork(children));
        }

        private static ClaimsPrincipal CreateUser(Guid userId, string role)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            }, "test"));
        }

        private sealed class FakeUnitOfWork : IUnitOfWork
        {
            private readonly IReadOnlyList<ChildProfile> _children;

            public FakeUnitOfWork(IReadOnlyList<ChildProfile> children)
            {
                _children = children;
            }

            public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
            {
                if (typeof(TEntity) == typeof(ChildProfile))
                    return (IGenericRepository<TEntity>)(object)new FakeRepository<ChildProfile>(_children);

                return new FakeRepository<TEntity>(Array.Empty<TEntity>());
            }

            public Task<int> SaveChangesAsync() => Task.FromResult(0);
        }

        private sealed class FakeRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
        {
            private readonly IReadOnlyList<TEntity> _items;

            public FakeRepository(IReadOnlyList<TEntity> items)
            {
                _items = items;
            }

            public Task CreateAsync(TEntity entity) => Task.CompletedTask;

            public void Update(TEntity entity)
            {
            }

            public void Delete(TEntity entity)
            {
            }

            public IQueryable<TEntity> GetAllAsync(bool trackChanges = false)
            {
                return _items.Where(item => !item.IsDeleted).AsQueryable();
            }

            public Task<TEntity?> GetByIdAsync(Guid id)
            {
                return Task.FromResult(_items.FirstOrDefault(item => item.Id == id && !item.IsDeleted));
            }

            public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return Task.FromResult(GetAllAsync().Where(predicate).ToList());
            }

            public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return Task.FromResult(GetAllAsync().Any(predicate));
            }
        }
    }
}
