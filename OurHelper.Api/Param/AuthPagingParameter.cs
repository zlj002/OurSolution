using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Param
{
    /// <summary>
    /// 带身份验证的分页参数
    /// </summary>
    [Serializable]
    public class AuthPagingParameter : AuthParameter
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageSize { get; set; }

        public AuthPagingParameter()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }
    }
}
