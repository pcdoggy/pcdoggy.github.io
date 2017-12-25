using hello.WebAPI.V1.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace hello.WebAPI.V1.Util
{
    // API入口相关
    public class ApiHelper
    {
        // 用户登录
        public static JObject GetUser(string uname, string upass)
        {
            JObject ReturnJson = new JObject();

            SqlConnection sqlc = SqlHelper.GetConnection();
            if (sqlc == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.自定义错误描述).ToString(), "数据连接失败");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT *
  FROM users
  WHERE username = {0} or telno = {1}", uname, uname);

            DataSet ds = SqlHelper.SqlQuery(sqlc, sb.ToString());
            if (ds == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.自定义错误描述).ToString(), "数据连接异常!获取用户信息失败!");
            }
            DataTable dtls = ds.Tables[0];
            if (dtls.Rows.Count == 0)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.用户信息不存在).ToString(), "用户不存在!");
            }

            if (dtls.Rows[0]["password"].ToString() != upass)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.用户名或密码错误).ToString(), "登录失败," + Enums.ApiCodes.用户名或密码错误.ToString());
            }

            //JObject errjson = new JObject();
            //if (CheckUser(sqlc, userno, ref errjson) == false)//校验用户
            //{
            //    return errjson;
            //}

            ReturnJson.Add("Success", false);
            ReturnJson.Add("Code", ((int)Enums.ApiCodes.不支持的接口类型).ToString());
            ReturnJson.Add("Message", Enums.ApiCodes.不支持的接口类型.ToString());
            ReturnJson.Add("Result", null);
            return ReturnJson;
        }

        // 接口入口
        public static JObject CallInterFace(string type, JObject data, string ver)
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