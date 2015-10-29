using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Param
{
    [Serializable]
    public class AuthParameter : ApiParameter
    {
        /// <summary>
        /// SessionID,登录成功后 sessionid 记录，后期访问需要
        /// </summary>
        public Guid SessionID { get; set; }

        /// <summary>
        /// Token,防止重复提交
        /// </summary>
        public Guid Token { get; set; }

        /// <summary>
        /// 当前用户唯一标识
        /// </summary> 
        public Guid PassportID { get; set; }
    }
}
