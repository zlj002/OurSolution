using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Result
{
    [Serializable]
    public class ApiResult
    {
        /// <summary>
        /// 交互的完成标志
        /// 0:失败
        /// 1:成功
        /// </summary>
        public int FinishFlag { get; set; }
        /// <summary>
        /// 交互的完成信息
        /// 文本格式
        /// </summary>
        public string FinishMessage { get; set; }
    }
}
