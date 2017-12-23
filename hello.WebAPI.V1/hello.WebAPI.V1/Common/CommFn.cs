using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Common
{
    public class CommFn
    {

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