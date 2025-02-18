using Server.Models;
using System.Linq.Expressions;

namespace Server.Core
{
    public interface IBase_Service<T> where T : class
    {
        Task<T?> Get(Guid id);
        Task<IList<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> FindOne(Expression<Func<T, bool>> predicate);
        Task<ResponseModel<string>> FindOneAndDelete(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Delete(dynamic id);
        Task InsertAsync(T entity);
        void UpdateRecord(T entity);
        Task<int> CompleteAync();
        
        Task AddRange(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);

        ResponseModel<string> CatchException(Exception ex);
        ResponseModel<string> CatchExceptionNull(dynamic entity);
        ResponseModel<string> CatchExceptionNull(IList<dynamic> entity);


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
        Task<IEnumerable<T>> AddBulk(IEnumerable<T> entities);
    }
}
