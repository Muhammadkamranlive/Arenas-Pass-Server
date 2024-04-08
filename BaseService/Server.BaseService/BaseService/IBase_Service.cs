using System.Linq.Expressions;

namespace Server.Core
{
    public interface IBase_Service<T> where T : class
    {
        Task<T?> Get(Guid id);
        Task<IList<T>> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Delete(Guid id);
        Task InsertAsync(T entity);
        void UpdateRecord(T entity);
        Task<int> CompleteAync();
        Task AddRange(IEnumerable<T> entities);

    }
}
