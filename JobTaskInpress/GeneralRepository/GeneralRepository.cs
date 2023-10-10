using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using JobTaskInpress.db;

namespace NewProject.Repositories
{
    public class GeneralRepository<T, TKey>: IGeneralRepository<T, TKey> where T : class
    {
        private readonly MainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public GeneralRepository(MainDbContext _DBContext, IUnitOfWork unitOfWork)
        {
            _dbContext = _DBContext;
            _unitOfWork = unitOfWork;
        }
        private IQueryable<T> SetEntity()
        {
            return _dbContext.Set<T>();
        }
        public IQueryable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderBy = null, string? orderDirection = "ASC" , int? take = null, int? skip = null, params string[] include)
        {
            var query = this.Where(expression: expression, include);
            
            if (take.HasValue)
                query.Take(take.Value);

            if (skip.HasValue)
                query.Skip(skip.Value);

            if (orderBy is not null)
            {
                if(orderDirection == "ASC")
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
        
            return query;
        }
        public IQueryable<T> Where(Expression<Func<T, bool>>? expression = null, params string[] include)
        {
            var query = this.SetEntity();
            if (include != null && include.Length > 0)
                foreach (string i in include)
                    query = query.Include(i);
            if (expression != null)
                query = query.Where(expression);
            return query;
        }
        
        public T? GetBy(Expression<Func<T, bool>> expression, params string[] include) => Where(expression, include).FirstOrDefault();
        public T? GetBy(Expression<Func<T, bool>> expression) => Where(expression).FirstOrDefault();
        public async Task<T?> GetById(TKey Id) => await _dbContext.Set<T>().FindAsync(Id);
        
        public async Task<EntityEntry<T>> AddAsync(T entry) => await _dbContext.Set<T>().AddAsync(entry);
        public EntityEntry<T> Add(T entry) => _dbContext.Set<T>().Add(entry);
        public EntityEntry<T> Update(T entry) => _dbContext.Set<T>().Update(entry);
        public EntityEntry<T> Remove(T entry) => _dbContext.Set<T>().Remove(entry);

        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChangesAsync();
        }
        public int SaveChanges()
        {
            return _unitOfWork.SaveChanges();
        }
        
        public bool IsExisted(Expression<Func<T, bool>> expression) => SetEntity().Any(expression);
        public async Task<bool> IsExistedAsync(Expression<Func<T, bool>> expression) =>  await SetEntity().AnyAsync(expression);
        public int Count(Expression<Func<T, bool>>? expression)
        {
            return expression is not null ? this.SetEntity().Count(expression) : this.SetEntity().Count();
        }
        public void EntityStateModified(T Entity)
        {
            _dbContext.Entry(Entity).State = EntityState.Modified;
        }
    }
}
