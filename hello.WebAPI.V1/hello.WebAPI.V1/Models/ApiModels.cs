using hello.WebAPI.V1.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Models
{
    // API入口参数检查相关
    public class ApiModels
    {
        /// <summary>
        /// 校验时间戳的合法性
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public bool CheckTimestamp(string timestamp)
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
        public int CheckAppkeyAndSign(string appkey, string timestamp, string data, string sign)
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

        public JObject CallInterFace(string type, JObject data, string ver)
        {
            JObject ReturnJson = new JObject();
            switch (type)
            {
                case "UserLogin":
                    //Enums.ApiTypes.UserLogin.ToString()
                    //ReturnJson = JKRFInterFaceModel.GetToken(ver, data);
                    break;

                default:
                    ReturnJson.Add("Success", false);
                    ReturnJson.Add("Code", ((int)Enums.ApiCodes.不支持的接口类型).ToString());
                    ReturnJson.Add("Message", Enums.ApiCodes.不支持的接口类型.ToString());
                    ReturnJson.Add("Result", null);
                    break;
            }
            return ReturnJson;
        }

    }
}