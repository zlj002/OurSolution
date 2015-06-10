using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace OurHelper
{
    /// <summary>
    /// 分页页面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageHelper<T> where T : class, new()
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页条目数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序，注意指定排序字段时，排序字段需包含在 所查询的字段中，不指定默认为公共字段 DisplayIndex
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 查询指定列,不指定为 全部字段 *
        /// </summary>
        public string Fields { get; set; }



        public PageHelper()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = " DisplayIndex ";
            QueryKeyValues = new List<KeyValue>();
            Data = new List<T>();
            ComplexKeyValues = new List<ComplexKeyValue>();
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        public List<KeyValue> QueryKeyValues;

        public List<ComplexKeyValue> ComplexKeyValues;
        /// <summary>
        /// 分页数据
        /// </summary>
        public List<T> Data { get; set; }
    }

    /// <summary>
    /// 合成的sql查询
    /// </summary>
    public class ComplexKeyValue
    {
        public ComplexKeyValue()
        {
            WhereType = KeyValueWhereType.And;
            QueryKeyValues = new List<KeyValue>();
        }
        public ComplexKeyValue(KeyValueWhereType whereType, List<KeyValue> queryKeyValues)
        {
            WhereType = whereType;
            QueryKeyValues = queryKeyValues;
        }
        /// <summary>
        /// 类型 and 或者or 组
        /// </summary>
        public KeyValueWhereType WhereType { get; set; }

        /// <summary>
        /// 条件组合
        /// </summary>
        public List<KeyValue> QueryKeyValues { get; set; }
    }

    /// <summary>
    /// 请求条件
    /// </summary>
    public class KeyValue
    {
        public KeyValue()
        {
            //默认为and条件
            WhereType = KeyValueWhereType.And;
            //默认为等于
            OperatorType = KeyValueOperatorType.Equal;
        }
        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
            //默认为and条件
            WhereType = KeyValueWhereType.And;
            //默认为等于
            OperatorType = KeyValueOperatorType.Equal;
        }
        public KeyValue(string key, string value, KeyValueWhereType whereType)
        {
            Key = key;
            Value = value;
            //默认为and条件
            WhereType = whereType;
            //默认为等于
            OperatorType = KeyValueOperatorType.Equal;
        }
        public KeyValue(string key, string value, KeyValueOperatorType operatorType)
        {
            Key = key;
            Value = value;
            //默认为and条件
            WhereType = KeyValueWhereType.And;
            //默认为等于
            OperatorType = operatorType;
        }
        public KeyValue(string key, string value, KeyValueWhereType whereType, KeyValueOperatorType operatorType)
        {
            Key = key;
            Value = value;
            //默认为and条件
            WhereType = whereType;
            //默认为等于
            OperatorType = operatorType;
        }
        /// <summary>
        /// 条件类型
        /// </summary>
        public KeyValueWhereType WhereType { get; set; }
        /// <summary>
        /// 条件字段名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 条件字段值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 操作符号类型比如 绝对相等或者like
        /// </summary>
        public KeyValueOperatorType OperatorType { get; set; }
    }
    /// <summary>
    /// 条件类型
    /// </summary>
    public enum KeyValueWhereType
    {
        //and连接
        And,
        //or连接
        Or
    }

    /// <summary>
    /// 键值判断类型
    /// </summary>
    public enum KeyValueOperatorType
    {
        /// <summary>
        /// 相等
        /// </summary>
        Equal,
        /// <summary>
        /// 全字匹配
        /// </summary>
        LikeAll,
        /// <summary>
        /// 匹配右侧，比如  value%
        /// </summary>
        LikeRight,
        /// <summary>
        /// 匹配左侧 比如 %value
        /// </summary>
        LikeLeft,

        /// <summary>
        /// 不相等
        /// </summary>
        Inequality,

        /// <summary>
        /// 大于
        /// </summary>
        GT,
        /// <summary>
        /// 大于等于
        /// </summary>
        GTE,
        /// <summary>
        /// 小于
        /// </summary>
        LT,
        /// <summary>
        /// 小于等于
        /// </summary>
        LTE,
        /// <summary>
        /// in 条件,多个值用英文,分割
        /// </summary>
        In
    }
}
