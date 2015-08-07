using Aliyun.OpenServices.OpenStorageService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Plugins.Aliyun
{
    public class OssHelper
    {
        private string _aliyunkeyid;
        private string _aliyunkeysecret;
        private string _bucketname;
        private string _customDomain;
        public OssHelper(string aliyunKeyID, string aliyunKeySecret,string bucketName, string customDomain)
        {
            this._aliyunkeyid = aliyunKeyID;
            this._aliyunkeysecret = aliyunKeySecret;
            this._bucketname = bucketName;
            this._customDomain = customDomain;
        }
        /// <summary>
        /// 本项目中文件上云
        /// </summary>
        /// <param name="fileName">文件名，可以带路径，存在则覆盖</param>
        /// <param name="imageContent">内容</param>
        /// <param name="meta">原数据描述</param>
        /// <returns></returns>
        public  string FileToOss(string fileName, Stream imageContent, ObjectMetadata meta)
        {
            try
            { 
                OssClient client = new OssClient(this._aliyunkeyid, this._aliyunkeysecret); 
                PutObjectResult putResult = client.PutObject(this._bucketname, fileName, imageContent, meta);
                var resulturl = this._customDomain.TrimEnd('/') + "/" + fileName;
                return resulturl;
            }
            catch (Exception ex)
            {
                throw new Exception("更新至阿里云出错" + ex.Message);
            }
        }
    }
}
