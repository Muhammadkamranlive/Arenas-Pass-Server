using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Server.BaseService
{
    public class Base_Service<T> : IBase_Service<T> where T : class
    {
        private readonly IUnit_Of_Work_Repo _unitOfWork;
        private readonly IRepo<T> _genericRepository;
        public Base_Service(IUnit_Of_Work_Repo unitOfWork, IRepo<T> genericRepository)
        {
            _unitOfWork        = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<int> CompleteAync()
        {
            return await _unitOfWork.Save();
        }

        public async Task<bool> Delete(dynamic id)
        {
            var tempRecord = await _genericRepository.Get(id);
            if (tempRecord != null)
            {
                return await _genericRepository.Remove(id);
            }
            throw new Exception($"{typeof(T).Name} with the {id} is  not Found sorry...!");

        }

        public async Task<IList<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return (IList<T>)await _genericRepository.Find(predicate);
            
            
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            await _genericRepository.AddRange(entities);
        }
        public async Task<T?> Get(Guid id)
        {
            var tempRecord = await _genericRepository.Get(id);
            return tempRecord;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _genericRepository.GetAll();
            
        }

        public async Task InsertAsync(T entity)
        {
            await _genericRepository.Add(entity);
        }

        public void UpdateRecord(T entity)
        {
            _genericRepository.Update(entity);
        }

        public async Task<T> FindOne(Expression<Func<T, bool>> predicate)
        {
            return await _genericRepository.FindOne(predicate);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _genericRepository.RemoveRange(entities);
        }


        public ResponseModel<string> CatchException( Exception ex)
        {
            ResponseModel<string> giftResponse = new ResponseModel<string>()
            {
                Status_Code = "500",
                Description = ex.Message,
                Response    = "Error Occurred"
            };
            return giftResponse;
        }

        public ResponseModel<string> CatchExceptionNull(dynamic entity)
        {
            ResponseModel<string> giftResponse = new ResponseModel<string>()
            {
                Status_Code = "200",
                Description = "OK",
                Response    = "OK"
            };
            if (entity == null)
            {
                giftResponse    = new ResponseModel<string>()
                {
                    Status_Code = "400",
                    Description = "Error Could not find record",
                    Response    = "Error Occurred"
                };
            }
            
            return giftResponse;
        }


        public ResponseModel<string> CatchExceptionNull(IList<dynamic> entity)
        {
            ResponseModel<string> giftResponse = new ResponseModel<string>()
            {
                Status_Code = "200",
                Description = "OK",
                Response    = "OK"
            };
            if (entity==null || entity.Count == 0)
            {
                giftResponse = new ResponseModel<string>()
                {
                    Status_Code = "400",
                    Description = "Error Could not find record",
                    Response    = "Error Occurred"
                };
            }

            return giftResponse;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataModel"></param>
        /// <exception cref="Exception"></exception>
        public  void Update(T data, object dataModel)
        {
            try
            {
                _genericRepository.Update(data, dataModel);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data"></param>
        /// <param name="includeProperties"></param>
        /// <exception cref="Exception"></exception>
        public void Update(T data, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                _genericRepository.Update(data,includeProperties);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="Exception"></exception>
        public void Update(IEnumerable<T> data)
        {
            try
            {
                _genericRepository.Update(data);
                _genericRepository.Save();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataModel"></param>
        /// <exception cref="Exception"></exception>
        public void Update(IEnumerable<T> data, IList<object> dataModel)
        {
            try
            {
                _genericRepository.Update(data,dataModel);
                _genericRepository.Save();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data"></param>
        /// <param name="includeProperties"></param>
        /// <exception cref="Exception"></exception>
        public void Update(IEnumerable<T> data, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                _genericRepository.Update(data,includeProperties);            
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <returns></returns>
        public async Task Save()
        {
            await _genericRepository.Save();
        }
        /// <summary>
        /// Transaction
        /// </summary>
        /// <returns></returns>
        public async Task Transaction()
        {
            await _genericRepository.Transaction();
        }
        /// <summary>
        /// Commit
        /// </summary>
        /// <returns></returns>
        public async Task Commit()
        {
            await _genericRepository.Commit();
        }
        /// <summary>
        /// Rollback
        /// </summary>
        /// <returns></returns>
        public async Task Rollback()
        {
            await _genericRepository.Rollback();
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            return await _genericRepository.AddAsync(entity);
        }

        public async Task<T> AddReturn(T entity)
        {
            return await _genericRepository.AddReturn(entity);
        }
    }

}
