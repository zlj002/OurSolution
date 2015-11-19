using MongoDB;
using MongoDB.Configuration;
using OurHelper.DataAccess.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.DataAccess.MongoDB.Repository
{
    public abstract class MongoDBRepositoryBase<T> : IRepository<T> where T : class,new()
    {
        #region 实现linq查询的映射配置
        /// <summary>
        /// 实现linq查询的映射配置
        /// </summary>
        public MongoConfiguration configuration
        {
            get
            {
                var config = new MongoConfigurationBuilder();

                config.Mapping(mapping =>
                {
                    mapping.DefaultProfile(profile =>
                    {
                        profile.SubClassesAre(t => t.IsSubclassOf(typeof(T)));
                    });
                    mapping.Map<T>();
                    mapping.Map<T>();
                });

                config.ConnectionString(connectionString);

                return config.BuildConfiguration();
            }
        }
        #endregion
        /// <summary>
        /// 数据库操作
        /// </summary>
        string connectionString = string.Empty;

        string databaseName = string.Empty;

        string collectionName = string.Empty;




        /// <summary>
        /// 默认构造
        /// </summary>
        public MongoDBRepositoryBase(string connString, string dbName, string collName)
        {
            connectionString = connString;
            databaseName = dbName;
            collectionName = collName;
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public T Insert(T entity)
        {
            using (Mongo mongo = new Mongo(configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(databaseName);

                    var collection = db.GetCollection<T>(collectionName);

                    collection.Insert(entity, true);

                    mongo.Disconnect();
                    return entity;
                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }

        public T Insert(T entity, bool isSubmit)
        {
            return this.Insert(entity);
        }

        public int DeleteBatch(List<T> list, string columnName)
        {
            throw new NotImplementedException();
        }

        public int DelEntityByColNamesAndColValues(Dictionary<string, object> dic)
        {
            throw new NotImplementedException();
        }

        public int LogicDeleteBatch(List<T> list, string columnName)
        {
            throw new NotImplementedException();
        }

        public int InsertBatchBySql(List<T> list)
        {
            throw new NotImplementedException();
        }

        public T GetEntityByColNameAndColValue(string colName, object colVal)
        {
            throw new NotImplementedException();
        }

        public T GetEntityByColNamesAndColValues(Dictionary<string, object> dicParams)
        {
            throw new NotImplementedException();
        }

        public OurHelper.PageHelper<T> GetListPage(OurHelper.PageHelper<T> pageQuery)
        {
            throw new NotImplementedException();
        }


        public T Update(T entity, System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(databaseName);

                    var collection = db.GetCollection<T>(collectionName);

                    collection.Update(entity, func, true);

                    mongo.Disconnect();
                    return entity;
                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }

        public int DeleteEntity(System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(databaseName);

                    var collection = db.GetCollection<T>(collectionName);

                    //这个地方要注意，一定要加上T参数，否则会当作object类型处理
                    //导致删除失败
                    collection.Remove(func);
                    
                    mongo.Disconnect();
                    return 1;
                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }

        public T GetEntity(System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(databaseName);

                    var collection = db.GetCollection<T>(collectionName);

                    var single = collection.FindOne(func);

                    mongo.Disconnect();

                    return single;

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }

        public PageHelper<T> GetListPage(PageHelper<T> pageQuery, System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(databaseName);

                    IMongoCollection<T> collection = db.GetCollection<T>(collectionName);

                    pageQuery.TotalCount = Convert.ToInt32(collection.Find(func).Documents.Count());

                    //var personList = collection.Find(func, pageQuery.PageSize, pageQuery.PageSize * (pageQuery.PageIndex - 1));
                    var personList = collection.Find(func).Limit(pageQuery.PageSize).Skip(pageQuery.PageSize * (pageQuery.PageIndex - 1));
                    pageQuery.Data = personList.Documents.ToList();

                    mongo.Disconnect();

                    return pageQuery;

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }
    }
}
