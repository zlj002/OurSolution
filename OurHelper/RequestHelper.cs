using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace OurHelper
{
    public class RequestHelper : Dictionary<string, string>
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
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }

       

        public RequestHelper()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        /// <summary>
        /// 处理页面传递的参数，返回 Dictionary类型的 参数名和值的 键值对
        /// 注意 返回的 Dictionary 中的key值区分大小写
        /// </summary>
        /// <param name="param"></param>
        public void GenerateSearchTerm(NameValueCollection param)
        {
            if (param != null)
            {
                foreach (var formKey in param.AllKeys)
                {
                    if (string.IsNullOrEmpty(formKey)
                        || param[formKey] == null
                        || string.IsNullOrEmpty(param[formKey].Trim()))
                        continue;

                    var key = formKey;
                    var val = param[formKey];



                    int i;
                    switch (key)
                    {

                        case "page":
                        case "pageindex":
                            if (int.TryParse(val, out i) && i > 0)
                            {
                                PageIndex = i;
                                Fillup("pageIndex", val);
                            }
                            break;
                        case "iDisplayStart":
                            if (int.TryParse(val, out i) && i >= 0)
                            {
                                i++;
                                int pageSizeTemp = 10;
                                if (int.TryParse(param["iDisplayLength"].ToString(), out pageSizeTemp) && pageSizeTemp > 0)
                                {
                                    PageIndex = (i % pageSizeTemp == 0) ? i / pageSizeTemp : ((i / pageSizeTemp) + 1);
                                    if (PageIndex == 0)
                                    {
                                        PageIndex++;
                                    }
                                }


                                Fillup("pageIndex", PageIndex.ToString());
                            }
                            break;
                        case "rows":
                        case "iDisplayLength":
                        case "pagesize":
                            if (int.TryParse(val, out i) && i > 0)
                            {
                                PageSize = i;
                                Fillup("pageSize", val);
                            }
                            break;

                        case "orderby":
                            OrderBy = val;
                            break;
                        case "iColumns":
                            int iColumns = 0;
                            if (int.TryParse(param["iColumns"].ToString(), out iColumns) && iColumns > 0)
                            {
                                for (int k = 0; k < iColumns; k++)
                                {
                                    if (param["mDataProp_" + k] != null && param["sSearch_" + k] != null)
                                    {
                                        Fillup(param["mDataProp_" + k], param["sSearch_" + k]);
                                    }
                                }

                            }
                            break;
                        default:
                            if (!ContainsKey(key))
                                Add(key, val);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏父类的ContainsKey，为避免大小写匹配 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool ContainsKey(string key)
        {
            bool result = false;
            if (base.ContainsKey(key) || base.ContainsKey(key.ToLower()) || base.ContainsKey(key.ToUpper()))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 隐藏父类的索引，为避免大小写匹配
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new object this[string key]
        {
            get
            {
                foreach (var t_key in base.Keys)
                {
                    if (t_key.ToLower() == key.ToLower())
                    {
                        return base[t_key];
                    }
                }
                return null;
            }
            set
            {
                base[key] = value.ToString();
            }
        }


        /// <summary>
        /// 若存在，则覆盖；不存在，则添加 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Fillup(string key, string value)
        {
            if (ContainsKey(key)) this[key] = value;
            else Add(key, value);
        }

        public override string ToString()
        {
            var ret = new StringBuilder();

            foreach (var key in Keys)
            {
                if (key != "_")
                    ret.AppendFormat("{0}={1}&", key, this[key]);
            }

            return ret.ToString().TrimEnd('&');
        }
    }
}
