using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurHelper
{

    /// <summary>
    /// 客户端与服务器端交互的信息
    /// </summary>
    [Serializable]
    public class InteractiveData
    {
        /// <summary>
        /// 客户端与服务器端交互的信息
        /// </summary>
        public InteractiveData()
        {
        }
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
        /// <summary>
        /// 交互完成后返回的界面状态信息
        /// Json格式的
        /// </summary>
        public string DisplayUI { get; set; }
    }
}
