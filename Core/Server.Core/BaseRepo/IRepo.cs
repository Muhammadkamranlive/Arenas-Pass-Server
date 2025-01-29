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
        Task<bool> Remove(dynamic id);
        void RemoveRange(IEnumerable<T> entities);
        Task<string> DeletRange(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        Task<T?> FindOne(Expression<Func<T, bool>> predicate);
        void Update(T data, object dataModel);
        void Update(T data, params Expression<Func<T, object>>[] includeProperties);
        void Update(IEnumerable<T> data);
        void Update(IEnumerable<T> data, IList<object> dataModel);
        void Update(IEnumerable<T> data, params Expression<Func<T, object>>[] includeProperties);
        Task Save();
        Task Transaction();
        Task Commit();
        Task Rollback();
        Task<T> AddAsync(T entity);
        Task<T> AddReturn(T entity);
    }
}
