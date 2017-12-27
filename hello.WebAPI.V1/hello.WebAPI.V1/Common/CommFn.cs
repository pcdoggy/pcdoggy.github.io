using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace hello.WebAPI.V1.Common
{
    public class CommFn
    {
        #region 数据返回
        // 错误结果信息
        public static JObject ErrorReturn(string code, string errormsg)
        {
            JObject ReturnJson = new JObject();
            ReturnJson.Add("Success", false);
            ReturnJson.Add("Code", code);
            ReturnJson.Add("Message", errormsg);
            ReturnJson.Add("Result", null);
            return ReturnJson;
        }

        // 每个接口对应正确接口信息
        public static JObject SuccessReturn(string code, string msg, JObject resjson)
        {
            JObject ReturnJson = new JObject();
            ReturnJson.Add("Success", true);
            ReturnJson.Add("Code", code);
            ReturnJson.Add("Message", msg);
            ReturnJson.Add("Result", resjson);
            return ReturnJson;
        }
        #endregion

        #region 校验

        /// <summary>
        /// 校验时间戳的合法性
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static bool CheckTimestamp(string timestamp)
        {
            if (timestamp.Length != 14) return false;
            try
            {
                string timestr = timestamp.Substring(0, 4) + "-" + timestamp.Substring(4, 2) + "-" + timestamp.Substring(6, 2) + " " + timestamp.Substring(8, 2) + ":" + timestamp.Substring(10, 2) + ":" + timestamp.Substring(12, 2);
                DateTime times = Convert.ToDateTime(timestr);
                TimeSpan ts = DateTime.Now - times;
                if (Math.Abs(ts.TotalMinutes) > 5)//时间戳和服务器时间相差5分钟以上
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验Appkey和Sign
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="timestamp"></param>
        /// <param name="data"></param>
        /// <param name="sign"></param>
        ///<returns>-1数据连接异常,-2非法AppKey,-3签名不合法</returns>
        public static int CheckAppkeyAndSign(string appkey, string timestamp, string data, string sign)
        {
            SqlConnection sqlc = SqlHelper.GetConnection();
            if (sqlc == null)
            {
                return -1;
            }

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@appkey", appkey);
            DataSet ds = SqlHelper.SqlQuery(sqlc, "App_GetAppKey", pars);
            if (ds == null)
            {
                return -1;
            }

            DataTable dtls = ds.Tables[0];
            if (dtls.Rows.Count == 0)
            {
                return -2;
            }
            string signs = EncryptHelper.MD5Encrypt(timestamp + data + dtls.Rows[0]["AppPassword"].ToString());

            if (EncryptHelper.MD5Encrypt(timestamp + data + dtls.Rows[0]["AppPassword"].ToString()) != sign)
            {
                return -3;
            }

            return 1;
        }

        /// <summary>
        /// 根据已有类，检查参数是否完整
        /// </summary>
        /// <typeparam name="T">类 泛型</typeparam>
        /// <param name="model">具体类内容</param>
        /// <param name="data">接口入参信息</param>
        /// <returns></returns>        
        public static bool CheckCommand<T>(T model, JObject data)
        {
            bool res = true;
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                // 类对象成员名称
                string name = item.Name;

                string userno = CommFn.GetJsonItemValue<string>(data, name);

                if (string.IsNullOrEmpty(userno) || string.IsNullOrEmpty(userno.Trim()))
                {
                    res = false;
                    break;
                }
                item.SetValue(model, userno);
            }
            return res;
        }

        #endregion

        #region JSON对象操作

        public static T GetJsonItemValue<T>(JObject job, string item)
        {
            JToken itemvalue = null;
            if (job.TryGetValue(item, out itemvalue) == false)
            {
                return default(T);
            }
            else
            {
                return itemvalue.Value<T>();
            }
        }

        #endregion
        #region 其他
        // 移除回车空格
        public static string RemoveEnterAndWrap(string str)
        {
            return str.Replace("\r", "").Replace("\n", "");
        }

        // 校验是否可以转换成时间
        public static bool CanToDateTime(object obj)
        {
            DateTime dtimels;
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }

            return DateTime.TryParse(obj.ToString(), out dtimels);
        }

        /// <summary>
        /// C#反射遍历对象属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        public static void ForeachClassProperties<T>(T model)
        {
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                string name = item.Name;
                object value = item.GetValue(model, null);
            }
        }
        #endregion

    }
}