using System.Linq.Expressions;

namespace Server.Core
{
    public interface IRepo<T> where T : class
    {
        Task<T?> Get(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task<bool> Remove(Guid id);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
