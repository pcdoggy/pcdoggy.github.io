using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static string RemoveEnterAndWrap(string str)
        {
            return str.Replace("\r", "").Replace("\n", "");
        }

        public static bool CanToDateTime(object obj)
        {
            DateTime dtimels;
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }

            return DateTime.TryParse(obj.ToString(), out dtimels);
        }


        #endregion

    }
}