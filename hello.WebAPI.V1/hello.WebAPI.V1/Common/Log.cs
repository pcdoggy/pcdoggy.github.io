using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace hello.WebAPI.V1.Common
{

    ////调用日志类的写日志的方法，将错误信息传入  
    //Log.WriteLog(ex.Message);  
    ////调用日志类的写日志的方法，将错误堆栈跟踪信息传入  
    //Log.WriteLog(ex.StackTrace);  
    public class Log
    {
        /// <summary>  
        /// 写入日志.  
        /// </summary>  
        /// <param name="strList"></param>  
        /// <remarks>  </remarks>  
        /// <Description>将错误信息写入日志文件（*.txt）</Description>  
        public static void WriteLog(params object[] strList)
        {
            //如果传过strList无内容，直接返回，不写日志  
            if (strList.Count() == 0) return;
            //获取本地服务器路径  
            string strDicPath = HttpContext.Current.Server.MapPath("~/temp/log/");
            //创建日志路径  
            string strPath = strDicPath + string.Format("{0:yyyy年-MM月-dd日}", DateTime.Now) + "日志记录.txt";
            //如果服务器路径不存在，就创建一个  
            if (!Directory.Exists(strDicPath)) Directory.CreateDirectory(strDicPath);
            //如果日志文件不存在，创建一个  
            if (!File.Exists(strPath))
            {
                using (FileStream fs = File.Create(strPath)) { }
            }
            //读取日志文件中的信息  
            string str = File.ReadAllText(strPath);
            StringBuilder sb = new StringBuilder();
            //将错误信息写入sb  
            foreach (var item in strList)
            {
                sb.Append("\r\n" + DateTime.Now.ToString() + "-----" + item + "");
            }
            //将错误信息写入txt  
            File.WriteAllText(strPath, sb.ToString() + "\r\n-----z-----\r\n" + str);
        }


        /// <summary>  
        /// 写入日志.  
        /// </summary>  
        /// <param name="strList">The STR list.</param>  
        /// <remarks></remarks>  
        /// <Description></Description>  
        public static void WriteLog(Action DefFunc, Func<string> ErrorFunc = null)
        {
            try
            {
                DefFunc();
            }
            catch (Exception ex)
            {
                string strDicPath = System.Web.HttpContext.Current.Server.MapPath("~/temp/log/");
                string strPath = strDicPath + string.Format("{0:yyyy年-MM月-dd日}", DateTime.Now) + "日志记录.txt";
                if (!Directory.Exists(strDicPath)) Directory.CreateDirectory(strDicPath);
                if (!File.Exists(strPath)) using (FileStream fs = File.Create(strPath)) { }
                string str = File.ReadAllText(strPath);
                StringBuilder sb = new StringBuilder();
                if (ErrorFunc != null)
                {
                    sb.Append("\r\n" + DateTime.Now.ToString() + "-----" + ErrorFunc());
                }
                sb.Append("\r\n" + DateTime.Now.ToString() + "-----" + ex.Message);
                sb.Append("\r\n" + DateTime.Now.ToString() + "-----" + ex.StackTrace);
                File.WriteAllText(strPath, sb.ToString() + "\r\n--z--------\r\n" + str);

            }
        }
    }
}