using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using System.IO;

namespace OurHelper
{
    public class ConvertToHtmlHelper
    {
        private static void FileStringChange(string phyFilePath, string oldString, string newString)
        {
            string[] allLines = System.IO.File.ReadAllLines(phyFilePath, Encoding.Default);

            bool isNeedCheck = true;//只检查head中，超过放弃
            for (int i = 0; i < allLines.Length; i++)
            {
                if (allLines[i].IndexOf(oldString) > -1)
                {
                    allLines[i] = allLines[i].Replace(oldString, newString);
                    System.IO.File.WriteAllLines(phyFilePath, allLines);
                    isNeedCheck = false;
                }
                else if (allLines[i].IndexOf("</head>") > -1)
                {
                    isNeedCheck = false;
                }
                if (!isNeedCheck)
                {
                    break;
                }
            }


            StringBuilder sb = new StringBuilder();
            sb.Append("<style type=\"text/css\">");
            sb.Append("BODY");
            sb.Append("{");
            sb.Append("scrollbar-face-color: #e8e7e7;");
            sb.Append("scrollbar-highlight-color: #ffffff;");
            sb.Append("scrollbar-shadow-color: #ffffff;");
            sb.Append("scrollbar-3dlight-color: #cccccc;");
            sb.Append("scrollbar-arrow-color: #03B7EC;");
            sb.Append("scrollbar-track-color: #EFEFEF;");
            sb.Append("scrollbar-darkshadow-color: #b2b2b2;");
            sb.Append("scrollbar-base-color: #000000;");
            sb.Append("}");
            sb.Append("</style>");
            StreamWriter sw = File.AppendText(phyFilePath);
            sw.Write("{0}", sb.ToString());
            sw.Flush();
            sw.Close();
            sw.Dispose();

        }


        public static string WordToHtml(string path, string savePath, string wordFileName)
        {

            //在此处放置用户代码以初始化页面
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();

            Type wordType = word.GetType();

            Microsoft.Office.Interop.Word.Documents docs = word.Documents;

            //打开文件
            Type docsType = docs.GetType();
            Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { path, true, true });

            //转换格式，另存为
            Type docType = doc.GetType();

            string wordSaveFileName = savePath;

            string strSaveFileName = savePath.TrimEnd('/') + "/" + (wordFileName.IndexOf(".html") > -1 ? wordFileName : (wordFileName + ".html"));

            object saveFileName = (object)strSaveFileName;

            docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });

            docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);

            //退出 Word
            wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);

            //修改编码
            FileStringChange(strSaveFileName, "charset=x-cp20936", "charset=uft-8");

            return saveFileName.ToString();
        }
        public static void ExcelToHtml(string path, string savePath, string wordFileName)
        {
            string str = string.Empty;
            Microsoft.Office.Interop.Excel.Application repExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            workbook = repExcel.Application.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            object htmlFile = savePath.TrimEnd('/') + "/" + (wordFileName.IndexOf(".html") > -1 ? wordFileName : (wordFileName + ".html"));
            object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            workbook.SaveAs(htmlFile, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            object osave = false;
            workbook.Close(osave, Type.Missing, Type.Missing);
            repExcel.Quit();

        }
        public static void PPTToHtml(string path, string savePath, string wordFileName)
        {

            Microsoft.Office.Interop.PowerPoint.Application ppApp = new Microsoft.Office.Interop.PowerPoint.Application();

            string strSourceFile = path;

            string strDestinationFile = savePath.TrimEnd('/') + "/" + (wordFileName.IndexOf(".html") > -1 ? wordFileName : (wordFileName + ".html"));

            Microsoft.Office.Interop.PowerPoint.Presentation prsPres = ppApp.Presentations.Open(strSourceFile, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);

            prsPres.SaveAs(strDestinationFile, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsHTML, MsoTriState.msoTrue);

            prsPres.Close();

            ppApp.Quit();
        }

        public static void TXTToHtml(string path, string savePath, string wordFileName) 
        {
            string[] allLines = System.IO.File.ReadAllLines(path, Encoding.Default);
            object htmlFile = savePath.TrimEnd('/') + "/" + (wordFileName.IndexOf(".html") > -1 ? wordFileName : (wordFileName + ".html"));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < allLines.Length; i++)
            {
                sb.Append(allLines[i]+"<br>");
            } 
            StreamWriter sw = File.AppendText(htmlFile.ToString());
            sw.Write("{0}", sb.ToString());
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }

}
