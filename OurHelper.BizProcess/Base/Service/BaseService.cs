using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;   
using OurHelper;
using OurHelper.DataAccess.Base.Interface;
using OurHelper.BizProcess.Base.Interface; 

namespace OurHelper.BizProcess.Base.Service
{
    public abstract class BaseService<T> : IBaseService<T> where T : class,new()
    { 

        //操作类
        public IRepository<T> repository;
        /// <summary>
        /// 默认构造
        /// </summary>
        public BaseService()
        { 
        }
        public BaseService(IRepository<T> repository)
        {
            this.repository = repository;
        }
          
        public T Update(T entity)
        {
            //验证
            //string valResult = ValidateHelper.ValidateObject<T>(entity, false);
            //if (!string.IsNullOrEmpty(valResult))
            //{
            //    throw new Exception(valResult);
            //}

            return repository.Update(entity);
        }

        public T Insert(T entity)
        {
            //验证
            //string valResult = ValidateHelper.ValidateObject<T>(entity);
            //if (!string.IsNullOrEmpty(valResult))
            //{
            //    throw new Exception(valResult);
            //}

            return repository.Insert(entity);
        }



        public int InsertBatchBySql(List<T> list)
        {
            return repository.InsertBatchBySql(list);
        }
         

        public T GetEntityByColNameAndColValue(string colName, object colVal)
        {
            return repository.GetEntityByColNameAndColValue(colName, colVal);
        }
         
        public PageHelper<T> GetListPage(PageHelper<T> pageQuery)
        {
            return repository.GetListPage(pageQuery);
        }
    }
}
