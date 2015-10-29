using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Param
{
    [Serializable]
    public abstract class ApiParameter
    { 
        /// <summary>
        /// 参数签名，防止篡改
        /// </summary>
        public string  Sign { get; set; } 
    }
}
