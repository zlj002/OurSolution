using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OurHelper
{
    public class LogHelper
    {
        /// <summary>
        /// 辅助在线调试日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteErrorlog(string message)
        {
            string desc = HttpContext.Current.Server.MapPath("~/ErrorBlog/");
            if (!System.IO.Directory.Exists(desc))
            {
                System.IO.Directory.CreateDirectory(desc);
            }
            string fullFileName = desc + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fullFileName, true);
            sw.Write("【" + DateTime.Now.ToString() + "】" + message+Environment.NewLine);
            sw.Close();

        }
    }
}
