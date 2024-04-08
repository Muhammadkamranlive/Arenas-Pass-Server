using Server.Domain;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Server.Core
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly ERPDb _crmContext;
        private readonly DbSet<T> _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Repo(ERPDb coursecontext, IHttpContextAccessor httpContextAccessor)
        {
            _crmContext = coursecontext;
            _dbSet = _crmContext.Set<T>();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Add(T entity)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            if (entity.GetType() != typeof(PelicanHRMTenant))
            {
                PropertyInfo property = entity.GetType().GetProperty("TenantId");
                if (property != null)
                {
                    property.SetValue(entity, tenantId);
                }
            }
            await _dbSet.AddAsync(entity);
            await LogOperationAsync("Add", entity);
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
               
                if (entity.GetType() != typeof(PelicanHRMTenant))
                {
                    var property = entity.GetType().GetProperty("TenantId");
                    if (property != null)
                    {
                        property.SetValue(entity, tenantId);
                    }
                }
            }

            await _dbSet.AddRangeAsync(entities);

            foreach (var entity in entities)
            {
                await LogOperationAsync("AddRange", entity);
            }

        }
        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T?> Get(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public async Task<bool> Remove(Guid id)
        {
            var GernericeEntitiy = await _dbSet.FindAsync(id);
            if (GernericeEntitiy != null)
            {
                _dbSet.Remove(GernericeEntitiy);
                await LogOperationAsync("Remove", GernericeEntitiy);
                return true;
            }
            return false;
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            foreach (var entity in entities)
            {
                LogOperationAsync("Remove", entity);
            }
        }
        public void Update(T entity)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            if (entity.GetType() != typeof(PelicanHRMTenant))
            {
                var property = entity.GetType().GetProperty("TenantId");
                if (property != null)
                {
                    property.SetValue(entity, tenantId);
                }
            }
            _crmContext.Entry(entity).State = EntityState.Modified;
        }


        private string SerializeEntity(T entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
        private async Task LogOperationAsync(string operation, T entity)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var UserId   = _httpContextAccessor.HttpContext?.Items["CurrentUserId"]?.ToString();
            var log = new AdminLogs
            {
                EntityType    = typeof(T).Name,
                Content       = SerializeEntity(entity),
                Timestamp     = DateTime.UtcNow,
                OperationType = operation,
                TenantId      = tenantId,
                UserId        = UserId==null? "uid":UserId
            };
            _crmContext.Logs.Add(log);
            await _crmContext.SaveChangesAsync();
        }

    }

}
