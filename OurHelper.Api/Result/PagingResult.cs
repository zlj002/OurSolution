using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Result
{
    [Serializable]
    public class PagingResult<T>
    {
        /// <summary>
        /// 数据库总记录数
        /// </summary>
        public long RecordCount { get; set; }
        /// <summary>
        /// 接口返回的 结果集
        /// </summary>
        public T[] ResultSet { get; set; }
    }
}
