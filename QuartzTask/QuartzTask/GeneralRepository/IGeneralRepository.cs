using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace NewProject.Repositories
{
    public interface IGeneralRepository<T, TKey> where T : class
    {
        EntityEntry<T> Add(T entry);
        Task<bool> AddRangeAsync(List<T> entry);
        Task<EntityEntry<T>> AddAsync(T entry);
        IQueryable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderby = null, string? orderbyDirection = "ASC", int? take = null, int? skip = null, params string[] include);
        IQueryable<T> Where(Expression<Func<T, bool>>? expression = null, params string[] include);
        T? GetBy(Expression<Func<T,bool>> expression, params string[] include);
        T? GetBy(Expression<Func<T,bool>> expression);
        EntityEntry<T> Remove(T entry);
        EntityEntry<T> Update(T entry);
        Task<T?> GetById(TKey Id);
        int SaveChanges();
        void EntityStateModified(T Entity);
        Task<int> SaveChangesAsync();
        Task<bool> IsExistedAsync(Expression<Func<T, bool>> expression);
        bool IsExisted(Expression<Func<T, bool>> expression);
        int Count(Expression<Func<T, bool>>? expression = null);
    }
}
