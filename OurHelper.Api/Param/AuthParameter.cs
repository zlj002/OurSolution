using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Param
{
    /// <summary>
    /// 带身份验证的参数
    /// </summary>
    [Serializable]
    public class AuthParameter : ApiParameter
    { 
        /// <summary>
        /// Token,防止篡改重复提交
        /// </summary>
        public Guid Token { get; set; }

        /// <summary>
        /// 当前用户唯一标识
        /// </summary> 
        public Guid PassportID { get; set; }
    }
}
