﻿using Server.Domain;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Server.Core
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly ERPDb                _crmContext;
        public readonly DbSet<T>             _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IDbContextTransaction         _currentTransaction;

        public Repo
        (
            ERPDb                coursecontext, 
            IHttpContextAccessor httpContextAccessor
        )
        {
            
            _httpContextAccessor  = httpContextAccessor;
            _crmContext           = coursecontext ?? throw new ArgumentNullException("dbContext");
            _dbSet                = _crmContext.Set<T>();
        }


        /// <summary>
        /// Add Data and return the entity with the primary key.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity with the primary key set.</returns>
        public async Task<T> AddAsync(T entity)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);

            if (entity.GetType() != typeof(ArenasTenants))
            {
                PropertyInfo property = entity.GetType().GetProperty("TenantId");
                if (property != null)
                {
                    property.SetValue(entity, tenantId);
                }
            }

            // Add entity to the DbSet
            await _dbSet.AddAsync(entity);

            // Save changes to generate the primary key value
            await _crmContext.SaveChangesAsync();

            // Log the operation
            await LogOperationAsync("Add", entity);

            // Return the entity, which now includes the primary key
            return entity;
        }


        public async Task<T> AddReturn(T entity)
        {
            // Add entity to the DbSet
            await _dbSet.AddAsync(entity);
            return entity;
        }


        /// <summary>
        /// Add Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Add(T entity)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            if (entity.GetType() != typeof(ArenasTenants))
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

        /// <summary>
        /// Add Range 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
               
                if (entity.GetType() != typeof(ArenasTenants))
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
        
        public async Task<IEnumerable<T>> AddBulk(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }
        /// <summary>
        /// Generic Find with Lambda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Generic Find to Get One with Lambda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<T?> FindOne(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Get one with Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> Get(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Remove(dynamic id)
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

        /// <summary>
        /// Remove Range
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            
        }

        public async Task<string> DeletRange(Expression<Func<T, bool>> predicate)
        {
           var entities  = await _dbSet.Where(predicate).ToListAsync();
           if (entities.Count != 0)
           {
               _dbSet.RemoveRange(entities);
               LogOperationAsync("Remove", entities);
            }
           return "OK";
        }
        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            if (entity.GetType() != typeof(ArenasTenants))
            {
                var property = entity.GetType().GetProperty("TenantId");
                if (property != null)
                {
                    property.SetValue(entity, tenantId);
                }
            }
            _crmContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Serialize 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string SerializeEntity(T entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
        /// <summary>
        /// Log Operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
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
        }


        private string  LogOperationAsync(string operation, IList<T> entity)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var UserId   = _httpContextAccessor.HttpContext?.Items["CurrentUserId"]?.ToString();
            
            IList<AdminLogs> Logs = new List<AdminLogs>();
            foreach (var item in entity)
            {
                var log = new AdminLogs
                {
                    EntityType    = typeof(T).Name,
                    Content       = SerializeEntity(item),
                    Timestamp     = DateTime.UtcNow,
                    OperationType = operation,
                    TenantId      = tenantId,
                    UserId        = UserId == null ? "uid" : UserId
                };

                Logs.Add(log);
            }
            _crmContext.Logs.AddRange(Logs);

            return "OK";
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataModel"></param>
        public void Update(T data, object dataModel)
        {
            string text = "";
            PropertyInfo[] properties = dataModel.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.ToLower() == "iud_value")
                {
                    text = (string)properties[i].GetValue(dataModel, null);
                    break;
                }
            }

            switch (text)
            {
                case "I":
                _crmContext.Entry(data).State = EntityState.Added;
                break;
                case "U":
                _crmContext.Entry(data).State = EntityState.Modified;
                break;
                case "D":
                _crmContext.Entry(data).State = EntityState.Deleted;
                break;
                default:
                _crmContext.Entry(data).State = EntityState.Unchanged;
                break;
            }
        }

        /// <summary>
        /// Update with Column Names 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="includeProperties"></param>
        public void Update(T data, params Expression<Func<T, object>>[] includeProperties)
        {
            EntityEntry<T> entityEntry = _crmContext.Entry(data);
            foreach (Expression<Func<T, object>> propertyExpression in includeProperties)
            {
                entityEntry.Property(propertyExpression).IsModified = true;
            }
        }
        /// <summary>
        /// Update Range
        /// </summary>
        /// <param name="data"></param>
        public void Update(IEnumerable<T> data)
        {
            _crmContext.UpdateRange(data);
        }

        /// <summary>
        /// Update Range
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataModel"></param>
        public void Update(IEnumerable<T> data, IList<object> dataModel)
        {
            string[] array = new string[dataModel.Count];
            int num = 0;
            foreach (object item in dataModel)
            {
                PropertyInfo[] properties = item.GetType().GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name.ToLower() == "iud_value")
                    {
                        array[num] = (string)properties[i].GetValue(item, null);
                        num++;
                        break;
                    }
                }
            }

            num = 0;
            foreach (T datum in data)
            {
                if (array[num] == "I")
                {
                    _crmContext.Entry(datum).State = EntityState.Added;
                }
                else if (array[num] == "U")
                {
                    _crmContext.Entry(datum).State = EntityState.Modified;
                }
                else if (array[num] == "D")
                {
                    _crmContext.Entry(datum).State = EntityState.Deleted;
                }
                else
                {
                    _crmContext.Entry(datum).State = EntityState.Unchanged;
                }

                num++;
            }
        }

        /// <summary>
        /// Update Range only Properties
        /// </summary>
        /// <param name="data"></param>
        /// <param name="includeProperties"></param>
        public void Update(IEnumerable<T> data, params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (T datum in data)
            {
                EntityEntry<T> entityEntry = _crmContext.Entry(datum);
                foreach (Expression<Func<T, object>> propertyExpression in includeProperties)
                {
                    entityEntry.Property(propertyExpression).IsModified = true;
                }
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns></returns>
        public async Task Save()
        {
            await _crmContext.SaveChangesAsync();
        }
        /// <summary>
        /// Transactions
        /// </summary>
        /// <returns></returns>
        public async Task Transaction()
        {
            await _crmContext.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commit
        /// </summary>
        /// <returns></returns>
        public async Task Commit()
        {
            if (_currentTransaction != null)
            {
                await _crmContext.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <summary>
        /// RollBack
        /// </summary>
        /// <returns></returns>
        public async Task Rollback()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

    }

}
