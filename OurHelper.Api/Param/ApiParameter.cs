using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Param
{
    /// <summary>
    /// 参数父类
    /// </summary>
    [Serializable]
    public abstract class ApiParameter
    {
        /// <summary>
        /// 参数签名，防止篡改
        /// </summary>
        private string _Sign { get; set; }
        /// <summary>
        /// 参数签名防止篡改
        /// </summary>
        /// <param name="signKey"></param>
        public ApiParameter Sign(string signKey)
        {
            var json = JObject.FromObject(this);
            json.Remove("_Sign");
            this._Sign = EncryptHelper.MD5Encrypt(json.ToString(Newtonsoft.Json.Formatting.None) + signKey);
            return this;
        }
        /// <summary>
        /// 验证参数签名
        /// </summary>
        /// <param name="signKey"></param>
        /// <returns></returns>
        public bool ValidateSign(string signKey)
        {
            var customerSign = this._Sign;
            return customerSign.Equals(this.Sign(signKey)._Sign,StringComparison.OrdinalIgnoreCase);
        }
    }
}
