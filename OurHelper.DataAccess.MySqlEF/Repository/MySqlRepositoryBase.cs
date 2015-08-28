using OurHelper.DataAccess.Base.Interface;
using OurHelper.Entity.Extend;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OurHelper.DataAccess.MySqlEF.Repository
{
    /// <summary>
    /// 本类为数据库访问层对Service 提供操作方法的具体实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MySqlRepositoryBase<T> : IRepository<T> where T : class,new()
    {
        /// <summary>
        /// 数据库操作
        /// </summary>
        private DbContext context;

        /// <summary>
        /// 架构名称
        /// </summary>
        public string SchemaName
        {
            get;
            set;
        }

        public MySqlRepositoryBase()
        {
        }
        /// <summary>
        /// 默认构造
        /// </summary>
        public MySqlRepositoryBase(DbContext dbContext)
        {
            this.context = dbContext;
        }


        #region 自定义查询条件

        /// <summary>
        /// 通用分页使用，需要在具体的数据库操作类中根据需求处理得到 DataAccess 构造sql语句
        /// </summary>
        protected string PageSqlWhere { get; set; }

        /// <summary>
        /// 通用分页使用，需要在具体的数据库操作类中根据需求处理得到 在dataaccess中使用 sql参数实际值，在service中构造出来
        /// </summary>
        protected List<object> PageParameters = new List<object>();

        ///// <summary>
        ///// 对某查询sql语句进行分页
        ///// </summary>
        protected string PageSqlTable { get; set; }
        #endregion

        #region  本项目数据库操作
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Update(T entity)
        {
            try
            {
                //执行验证业务
                context.Entry<T>(entity).GetValidationResult();
                //if (context.Entry<T>(entity).State == EntityState.Modified)
                //{
                context.SaveChanges();
                //}

            }
            catch (DbEntityValidationException dbEx)
            {
                throw dbEx;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
            return entity;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Insert(T entity)
        {
            return this.Insert(entity, true);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        public T Insert(T entity, bool isSubmit = true)
        {
            try
            {
                context.Set<T>().Add(entity);
                if (isSubmit)
                {
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                throw dbEx;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
            return entity;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int InsertBatchBySql(List<T> list)
        {
            string tableName = this.SchemaName + typeof(T).Name;
            System.Reflection.PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            StringBuilder sql = new StringBuilder();
            foreach (var item in list)
            {
                //列名
                List<string> ColuNames = new List<string>();

                //值
                List<object> ColuValues = new List<object>();

                foreach (var pi in propertyInfos)
                {
                    var tempValue = pi.GetValue(item, null);
                    if (tempValue != null)
                    {
                        Type t = tempValue.GetType();// yourType指的是你要判断的类型
                        var attrs = pi.GetCustomAttributes(typeof(EntityMemberAttribute), true);
                        //是否生成对应的sql语句
                        bool needSql = true;
                        if (attrs.Length == 1)
                        {
                            EntityMemberAttribute attr = (EntityMemberAttribute)attrs[0];
                            if (attr.eemu == EnumEntityMemberUsage.NoDBField)
                            {
                                needSql = false;
                            }
                        }
                        //如果需要生成对应的sql则生成
                        if (needSql && (t.IsValueType || t == typeof(System.String))) //为true，表示是.net的原生类型，即值类型，注意string类型，自定义的struct，class不是值类型
                        {
                            ColuNames.Add(pi.Name);
                            ColuValues.Add("'" + tempValue + "'");
                        }
                    }
                }
                string whereSql = string.Empty;
                for (int i = 0; i < ColuNames.Count; i++)
                {
                    whereSql = whereSql + " and " + ColuNames[i] + "=" + ColuValues[i];
                }

                string isExist = " if(NOT EXISTS(Select * From " + tableName + " Where 1=1 " + whereSql + " ))";

                sql.AppendFormat(isExist + " Begin insert into {0} ({1}) values({2}) End", tableName, string.Join(",", ColuNames.ToArray()), string.Join(",", ColuValues.ToArray()));
            }

            return context.Database.ExecuteSqlCommand(sql.ToString());
        }

        /// <summary>
        /// 根据列名和值获取单个对象
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="colVal"></param>
        /// <returns></returns>
        public T GetEntityByColNameAndColValue(string colName, object colVal)
        {
            try
            {
                string tableName = this.SchemaName + typeof(T).Name;
                List<object> list = new List<object>();
                string sqlString = "select * from " + tableName + " where 1=1 ";
                sqlString += " and " + colName + "={" + list.Count + "} ";
                list.Add(colVal);
                return this.SqlQuery(sqlString, list.ToArray()).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        /// <summary>
        /// 根据多个列名和值获取单个对象
        /// </summary>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public T GetEntityByColNamesAndColValues(Dictionary<string, object> dicParams)
        {
            try
            {
                string tableName = this.SchemaName + typeof(T).Name;
                List<object> list = new List<object>();
                string sqlString = "select * from " + tableName + " where 1=1 ";
                if (dicParams.Count > 0)
                {
                    foreach (KeyValuePair<string, object> kvp in dicParams)
                    {
                        sqlString += " and " + kvp.Key + "={" + list.Count + "} ";
                        list.Add(kvp.Value);
                    }
                    return this.SqlQuery(sqlString, list.ToArray()).FirstOrDefault();
                }
                throw new Exception("Dictionary里参数数量不能小于1");
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 默认分页
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public virtual PageHelper<T> GetListPage(PageHelper<T> pageQuery)
        {
            //构造查询条件
            StringBuilder sb = GenSqlAndSqlParameters(pageQuery);
            List<T> list = this.SqlQuery(sb.ToString(), PageParameters.ToArray());
            pageQuery.Data = list;
            PageSqlWhere = string.Empty;
            PageParameters.Clear();
            PageSqlTable = string.Empty;
            return pageQuery;
        }

        /// <summary>
        /// 分页带扩展属性
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        protected virtual PageHelper<T> GetListPageByCustomSql(PageHelper<T> pageQuery)
        {
            //构造查询条件
            StringBuilder sb = GenSqlAndSqlParameters(pageQuery);
            List<T> list = this.SqlQueryForListByCustomSql(sb.ToString(), PageParameters.ToArray());
            pageQuery.Data = list;
            PageSqlWhere = string.Empty;
            PageParameters.Clear();
            PageSqlTable = string.Empty;
            return pageQuery;
        }
        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="list"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int LogicDeleteBatch(List<T> list, string columnName)
        {

            #region 拼Sql语句
            var strWhereSql = new StringBuilder();
            List<object> objList = new List<object>();
            //避免注入
            //构造in条件
            if (list.Count > 0)
            {
                strWhereSql.Append("  " + columnName + " in (");
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        if (i == 0)
                        {
                            strWhereSql.Append(" {" + i + "} ");
                        }
                        else
                        {
                            strWhereSql.Append(" ,{" + i + "} ");
                        }
                        T RowInstance = Activator.CreateInstance<T>();//动态创建数据实体对象 


                        PropertyInfo Property = typeof(T).GetProperty(columnName);

                        object value = Property.GetValue(list[i], null);
                        objList.Add(value);
                    }
                    catch
                    {
                        continue;
                    }

                }
                strWhereSql.Append(" )");
            }
            #endregion


            string tableName = this.SchemaName + typeof(T).Name;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update  {0}  set Status =0  where 1=2 ", tableName);
            if (!string.IsNullOrEmpty(strWhereSql.ToString()))
            {
                sql.AppendFormat(" or ({0})", strWhereSql.ToString());
            }
            return context.Database.ExecuteSqlCommand(sql.ToString(), objList.ToArray());
        }

        /// <summary>
        /// 批量物理删除
        /// </summary>
        /// <param name="list"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int DeleteBatch(List<T> list, string columnName)
        {

            #region 拼Sql语句
            var strWhereSql = new StringBuilder();
            List<object> objList = new List<object>();
            //避免注入
            //构造in条件
            if (list.Count > 0)
            {
                strWhereSql.Append("  " + columnName + " in (");
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        if (i == 0)
                        {
                            strWhereSql.Append(" {" + i + "} ");
                        }
                        else
                        {
                            strWhereSql.Append(" ,{" + i + "} ");
                        }
                        T RowInstance = Activator.CreateInstance<T>();//动态创建数据实体对象 


                        PropertyInfo Property = typeof(T).GetProperty(columnName);

                        object value = Property.GetValue(list[i], null);
                        objList.Add(value);
                    }
                    catch
                    {
                        continue;
                    }

                }
                strWhereSql.Append(" )");
            }
            #endregion
            string tableName = this.SchemaName + typeof(T).Name;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("delete from {0} where 1=2 ", tableName);
            if (!string.IsNullOrEmpty(strWhereSql.ToString()))
            {
                sql.AppendFormat(" or ({0})", strWhereSql.ToString());
            }
            return context.Database.ExecuteSqlCommand(sql.ToString(), objList.ToArray());
        }

        /// <summary>
        /// 根据指定列值物理删除对象
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public int DelEntityByColNamesAndColValues(Dictionary<string, object> dic)
        {
            string tableName = this.SchemaName + typeof(T).Name;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("delete from {0} where 1=2 ", tableName);

            List<object> list = new List<object>();

            if (dic.Count > 0)
            {
                sql.Append("OR ( 1=1");
                foreach (KeyValuePair<string, object> item in dic)
                {
                    sql.Append(" AND " + item.Key + "={" + list.Count + "}");
                    list.Add(item.Value);
                }
                sql.Append(")");
            }
            return context.Database.ExecuteSqlCommand(sql.ToString(), list.ToArray());
        }

        /// <summary>
        /// 自定义sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<T> SqlQueryForListByCustomSql(string sql, params object[] parameters)
        {
            DataTable table = this.SqlQueryForDataTable(sql, parameters);
            return DataHelper.FillList<T>(table);
        }

        /// <summary>
        /// 根据条件构造查询条件和参数值
        /// </summary>
        /// <param name="queryKeyValues"></param>
        private StringBuilder GenSqlAndSqlParameters(PageHelper<T> pageQuery)
        {
            string firstWhere = " 1=1 ";

            //如果没有自定义处理，则默认处理
            if (PageParameters.Count == 0)
            {
                //根据复合条件查询
                for (int i = 0; i < pageQuery.ComplexKeyValues.Count; i++)
                {
                    ComplexKeyValue complexKeyValue = pageQuery.ComplexKeyValues[i];
                    if (complexKeyValue.WhereType == OurHelper.KeyValueWhereType.And)
                    {
                        PageSqlWhere += " and ( ";
                    }
                    if (complexKeyValue.WhereType == OurHelper.KeyValueWhereType.Or)
                    {
                        PageSqlWhere += " or ( ";
                        //如果有一个是or 
                        firstWhere = " 1=2 ";

                    }
                    for (int j = 0; j < complexKeyValue.QueryKeyValues.Count; j++)
                    {
                        KeyValue item = complexKeyValue.QueryKeyValues[j];
                        //连接词
                        string preLink = " and ";
                        if (item.WhereType == OurHelper.KeyValueWhereType.And)
                        {
                            preLink = " and ";
                        }
                        if (item.WhereType == OurHelper.KeyValueWhereType.Or)
                        {
                            preLink = " or ";
                        }
                        if (j == 0)
                        {
                            preLink = "  ";
                        }
                        if (item.OperatorType == KeyValueOperatorType.Equal)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + "= {" + PageParameters.Count + "} ";
                            PageParameters.Add(item.Value.ToString());
                        }
                        if (item.OperatorType == KeyValueOperatorType.LikeAll)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " like {" + PageParameters.Count + "} ";
                            PageParameters.Add("%" + item.Value.ToString() + "%");
                        }
                        if (item.OperatorType == KeyValueOperatorType.LikeLeft)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " like {" + PageParameters.Count + "} ";
                            PageParameters.Add("%" + item.Value.ToString());
                        }
                        if (item.OperatorType == KeyValueOperatorType.LikeRight)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " like {" + PageParameters.Count + "} ";
                            PageParameters.Add(item.Value.ToString() + "%");
                        }
                        if (item.OperatorType == KeyValueOperatorType.Inequality)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " != {" + PageParameters.Count + "} ";
                            PageParameters.Add(item.Value.ToString());
                        }
                        if (item.OperatorType == KeyValueOperatorType.GT)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " > {" + PageParameters.Count + "} ";
                            PageParameters.Add(item.Value.ToString());
                        }
                        if (item.OperatorType == KeyValueOperatorType.GTE)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " >= {" + PageParameters.Count + "} ";
                            PageParameters.Add(item.Value.ToString());
                        }
                        if (item.OperatorType == KeyValueOperatorType.LT)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " < {" + PageParameters.Count + "} ";
                            PageParameters.Add(item.Value.ToString());
                        }
                        if (item.OperatorType == KeyValueOperatorType.LTE)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " <= {" + PageParameters.Count + "} ";
                            PageParameters.Add(item.Value.ToString());
                        }
                        if (item.OperatorType == KeyValueOperatorType.In)
                        {
                            string[] ins = item.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (ins.Length > 0)
                            {
                                PageSqlWhere += preLink + item.Key.ToString() + " in ( ";
                                for (int m = 0; m < ins.Length; m++)
                                {
                                    if (m == 0)
                                    {
                                        PageSqlWhere += " {" + PageParameters.Count + "} ";
                                    }
                                    else
                                    {
                                        PageSqlWhere += " ,{" + PageParameters.Count + "} ";
                                    }
                                    PageParameters.Add(ins[m]);
                                }
                                PageSqlWhere += " )";
                            }
                        }
                    }
                    PageSqlWhere += " ) ";
                }
                //根据查询条件直接构造
                foreach (var item in pageQuery.QueryKeyValues)
                {
                    //连接词
                    string preLink = " and ";
                    if (item.WhereType == OurHelper.KeyValueWhereType.And)
                    {
                        preLink = " and ";
                    }
                    if (item.WhereType == OurHelper.KeyValueWhereType.Or)
                    {
                        preLink = " or ";
                        //如果有一个是or 
                        firstWhere = " 1=2 ";
                    }

                    if (item.OperatorType == KeyValueOperatorType.Equal)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + "= {" + PageParameters.Count + "} ";
                        PageParameters.Add(item.Value.ToString());
                    }
                    if (item.OperatorType == KeyValueOperatorType.LikeAll)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " like {" + PageParameters.Count + "} ";
                        PageParameters.Add("%" + item.Value.ToString() + "%");
                    }
                    if (item.OperatorType == KeyValueOperatorType.LikeLeft)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " like {" + PageParameters.Count + "} ";
                        PageParameters.Add("%" + item.Value.ToString());
                    }
                    if (item.OperatorType == KeyValueOperatorType.LikeRight)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " like {" + PageParameters.Count + "} ";
                        PageParameters.Add(item.Value.ToString() + "%");
                    }
                    if (item.OperatorType == KeyValueOperatorType.Inequality)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " != {" + PageParameters.Count + "} ";
                        PageParameters.Add(item.Value.ToString());
                    }
                    if (item.OperatorType == KeyValueOperatorType.GT)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " > {" + PageParameters.Count + "} ";
                        PageParameters.Add(item.Value.ToString());
                    }
                    if (item.OperatorType == KeyValueOperatorType.GTE)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " >= {" + PageParameters.Count + "} ";
                        PageParameters.Add(item.Value.ToString());
                    }
                    if (item.OperatorType == KeyValueOperatorType.LT)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " < {" + PageParameters.Count + "} ";
                        PageParameters.Add(item.Value.ToString());
                    }
                    if (item.OperatorType == KeyValueOperatorType.LTE)
                    {
                        PageSqlWhere += preLink + item.Key.ToString() + " <= {" + PageParameters.Count + "} ";
                        PageParameters.Add(item.Value.ToString());
                    }
                    if (item.OperatorType == KeyValueOperatorType.In)
                    {
                        string[] ins = item.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (ins.Length > 0)
                        {
                            PageSqlWhere += preLink + item.Key.ToString() + " in ( ";
                            for (int m = 0; m < ins.Length; m++)
                            {
                                if (m == 0)
                                {
                                    PageSqlWhere += " {" + PageParameters.Count + "} ";
                                }
                                else
                                {
                                    PageSqlWhere += " ,{" + PageParameters.Count + "} ";
                                }
                                PageParameters.Add(ins[m]);
                            }
                            PageSqlWhere += " )";
                        }
                    }
                }
            }
            string fields = string.IsNullOrEmpty(pageQuery.Fields) ? " * " : pageQuery.Fields;
            string tableName = this.SchemaName + typeof(T).Name;
            if (!string.IsNullOrEmpty(PageSqlTable))
            {
                tableName = PageSqlTable;
            }
            else
            {
                tableName = " select " + fields + " from " + tableName;
            }


            string orderBy = string.IsNullOrEmpty(pageQuery.OrderBy) ? " DisplayIndex " : pageQuery.OrderBy;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select count(0) from ({0}  where  " + firstWhere + "  {1}) A ", tableName, PageSqlWhere);
            pageQuery.TotalCount = this.GetCount(sb.ToString(), PageParameters.ToArray());
            sb.Clear();
            sb.AppendFormat(" select {0} from ( ", fields);
            sb.AppendFormat(" select {0}, ROW_NUMBER() OVER(order by {1}) as row from ({2} where " + firstWhere + " {3}) B ", fields, orderBy, tableName, PageSqlWhere);
            sb.AppendFormat(" )as a ");
            sb.AppendFormat(" where row between {0} and {1} ", (pageQuery.PageIndex - 1) * pageQuery.PageSize + 1, pageQuery.PageIndex * pageQuery.PageSize);
            return sb;
        }

        /// <summary>
        /// 构造查询语句
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<T> SqlQuery(string sqlString, params object[] parameters)
        {
            try
            {
                return context.Set<T>().SqlQuery(sqlString, parameters).ToList();

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected int GetCount(string sqlString, params object[] parameters)
        {
            try
            {
                return context.Database.SqlQuery<int>(sqlString, parameters).ToList().FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        #endregion

        #region 私有方法，使用ado.net实现对包含扩展属性但未映射的查询，适用本项目数据库

        /// <summary>
        /// EF SQL 语句返回 dataTable
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DataTable SqlQueryForDataTable(
                 string sql,
                  params object[] parameters)
        {
            //替换sql
            sql = Regex.Replace(sql, @"{", "@");
            sql = Regex.Replace(sql, @"}", "");
            SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = this.context.Database.Connection.ConnectionString;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                if (parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var item = new SqlParameter(i.ToString(), parameters[i]);
                        cmd.Parameters.Add(item);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }



        private DataTable SqlQueryForDataTable(string tableName, string whereStr, string orderStr, ref int pageIndex, ref int pageSize, ref int totalCount, params object[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select count(0) from ({0}  where 1=1 {1}) A ", tableName, whereStr);
            totalCount = this.GetCount(sb.ToString(), parameters);
            sb.Clear();
            sb.AppendFormat(" select * from ( ");
            sb.AppendFormat(" select *, ROW_NUMBER() OVER(order by {0}) as row from ({1} where 1=1 {2}) B ", orderStr, tableName, whereStr);
            sb.AppendFormat(" )as a ");
            sb.AppendFormat(" where row between {0} and {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            DataTable table = this.SqlQueryForDataTable(sb.ToString(), parameters);
            return table;
        }




        #endregion


        public T Update(T entity, System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            throw new NotImplementedException();
        }

        public int DeleteEntity(System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            throw new NotImplementedException();
        }

        public T GetEntity(System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            throw new NotImplementedException();
        }

        public PageHelper<T> GetListPage(PageHelper<T> pageQuery, System.Linq.Expressions.Expression<Func<T, bool>> func)
        {
            throw new NotImplementedException();
        }
    }
}
