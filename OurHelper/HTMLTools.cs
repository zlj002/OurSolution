using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OurHelper
{
    public class HTMLTools
    {
        static public string HTMLTextClearStyle(string htmlText)
        {

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"<script[^>]*?>.*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"<(.[^>]*)>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"([\r\n])[\s]+", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"-->", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"<!--.*", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(quot|#34);", "\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(amp|#38);", "&", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(lt|#60);", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(gt|#62);", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(nbsp|#160);", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(iexcl|#161);", "\xa1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(cent|#162);", "\xa2", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(pound|#163);", "\xa3", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&(copy|#169);", "\xa9", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            htmlText = System.Text.RegularExpressions.Regex.Replace(htmlText, @"&#(\d+);", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            htmlText.Replace("<", "");

            htmlText.Replace(">", "");

            htmlText.Replace("\r\n", "");
            return htmlText;
        }



    }
}
