using Microsoft.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace OurHelper
{
    public class HttpClientHelper
    {
        /// <summary>
        /// 验证远程URL 是否有效
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public static bool VerifyURL(string url,Dictionary<string, object> parameter)
        {
            HttpClient client = new HttpClient();
            client.DefaultHeaders.Add("ContentType", "application/json");
            client.DefaultHeaders.Add("Accept", "application/json");
            HttpContent content = HttpContent.Create(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json"); 
            HttpResponseMessage responseMessage = client.Post(url, content);
            if (responseMessage.StatusCode== HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetData(string url, Dictionary<string, object> parameter)
        {
            string result = string.Empty;
            HttpClient client = new HttpClient(url);
            client.DefaultHeaders.Add("ContentType", "application/json");
            client.DefaultHeaders.Add("Accept", "application/json");
            HttpContent content = HttpContent.Create(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json");
            HttpQueryString qStr = new HttpQueryString();
            foreach(var item in parameter){
                qStr.Add(item.Key,item.Value.ToString());
            }
            HttpResponseMessage responseMessage = client.Get(new Uri(url),qStr);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                result=responseMessage.Content.ReadAsString();
            } 
            return result;
        }

        public static string PostData(string url, Dictionary<string, object> parameter)
        {
            string result = string.Empty;
            HttpClient client = new HttpClient();
            client.DefaultHeaders.Add("ContentType", "application/json");
            client.DefaultHeaders.Add("Accept", "application/json");
            HttpContent content = HttpContent.Create(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = client.Post(url, content);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                result = responseMessage.Content.ReadAsString();
            } 
            return result;
        }
    }
}