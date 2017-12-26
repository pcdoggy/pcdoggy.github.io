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
    // API入口相关：1.登录入口，获取token信息；2.其他数据操作入口，获取具体业务信息；3.校验token信息
    public class ApiHelper
    {
        // 用户登录
        public static JObject GetUser(string uname, string upass)
        {
            JObject ReturnJson = new JObject();
            try
            {
                SqlConnection sqlc = SqlHelper.GetConnection();
                if (sqlc == null)
                {
                    return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接失败).ToString(), "数据连接失败");
                }
                // 查询用户信息，用户名或电话号码登录
                StringBuilder sbsql = new StringBuilder();
                sbsql.AppendFormat(@"SELECT *
  FROM users
  WHERE username = {0} or telno = {1}", uname, uname);

                DataSet ds = SqlHelper.SqlQuery(sqlc, sbsql.ToString());
                if (ds == null)
                {
                    return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接异常).ToString(), "数据连接异常!获取用户信息失败!");
                }

                DataTable dtls = ds.Tables[0];
                if (dtls.Rows.Count == 0)
                {
                    return CommFn.ErrorReturn(((int)Enums.ApiCodes.用户信息不存在).ToString(), "用户不存在!");
                }
                if (dtls.Rows[0]["userpass"].ToString() != upass)
                {
                    return CommFn.ErrorReturn(((int)Enums.ApiCodes.用户名或密码错误).ToString(), "登录失败," + Enums.ApiCodes.用户名或密码错误.ToString());
                }

                // 用户名密码存在。设置当前登录成功后的Token信息。有效期为1天。
                string usertoken = Guid.NewGuid().ToString().Replace("-", ""); sbsql = new StringBuilder();
                sbsql.AppendFormat(@"update users set token = {0}, tokentime = {1}, updtime = {2} WHERE userno = {3}", usertoken, DateTime.Now.AddDays(1), DateTime.Now, dtls.Rows[0]["userno"].ToString());
                int updcnt = SqlHelper.SqlNonQuery(sqlc, sbsql.ToString());
                if (updcnt <= 0)
                {
                    return CommFn.ErrorReturn(((int)Enums.ApiCodes.tokenerror).ToString(), GetDescription.getdescription(Enums.ApiCodes.tokenerror));
                }
                // 更新token成功
                JObject resualjson = new JObject();
                resualjson.Add("userno", dtls.Rows[0]["userno"].ToString());
                resualjson.Add("username", dtls.Rows[0]["username"].ToString());
                resualjson.Add("telno", dtls.Rows[0]["telno"].ToString());
                resualjson.Add("token", usertoken);

                return CommFn.SuccessReturn("0000", "获取令牌成功!", resualjson);
            }
            catch (Exception ex)
            {
                Log.WriteLog("登录入口出错，原因为：" + ex.Message);
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.系统错误).ToString(), "发生错误!详细信息:" + ex.Message);
            }
        }

        // 接口入口
        public static JObject CallInterFace(string type, JObject data, string ver)
        {
            JObject ReturnJson = new JObject();
            switch (type)
            {
                case "GetBooks":
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



        /// <summary>
        /// 校验令牌
        /// </summary>
        /// <param name="sqlc"></param>
        /// <param name="userno"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckToken(SqlConnection sqlc, string userno, string token, ref JObject errjson)
        {
            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@userno", userno);
            pars.Add("@token", token);

            // 获取用户令牌信息
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(@"SELECT *
  FROM users
  WHERE userno = {0} ", userno);

            DataSet ds = SqlHelper.SqlQuery(sqlc, sbsql.ToString());
            if (ds == null)
            {
                errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接异常).ToString(), Enums.ApiCodes.数据连接异常.ToString() + ",获取令牌信息失败!");
                return false;
            }
            DataTable dtls = ds.Tables[0];
            if (dtls.Rows.Count == 0)//用户信息不存在
            {
                errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.用户信息不存在).ToString(), Enums.ApiCodes.用户信息不存在.ToString());
                return false;
            }

            DateTime tokentime = DateTime.Now;
            DateTime.TryParse(dtls.Rows[0]["tokentime"].ToString(), out tokentime);
            if (tokentime > DateTime.Now)//令牌已过期
            {
                errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.令牌已过期).ToString(), Enums.ApiCodes.令牌已过期.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 控制同一个令牌对同一业务类型接口进行调用的时间必须大于上次调用时间
        /// </summary>
        /// <param name="sqlc"></param>
        /// <param name="userno"></param>
        /// <param name="token"></param>
        /// <param name="interfacetype"></param>
        /// <param name="cliensystemtime"></param>
        /// <param name="errjson"></param>
        /// <returns></returns>
        public static bool CheckInterFaceCall(SqlConnection sqlc, string userno, string token, string interfacetype, string cliensystemtime, ref JObject errjson)
        {
            // 获取用户令牌信息
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(@"SELECT [iid]
      ,[userno]
      ,[intertype]
      ,[token]
      ,[clientsystime]
  FROM [dbo].[interface]
  WHERE userno = {0} and intertype = {1} ", userno, interfacetype);

            DataSet ds = SqlHelper.SqlQuery(sqlc, sbsql.ToString());
            if (ds == null)
            {
                errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接异常).ToString(), Enums.ApiCodes.数据连接异常.ToString() + ",获取接口调用状态失败!");
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                string lasttime = ds.Tables[0].Rows[0]["clientsystime"].ToString();

                DateTime lastclinetime;
                if (DateTime.TryParse(lasttime.Substring(0, 4) + "-" + lasttime.Substring(4, 2) + "-" + lasttime.Substring(6, 2) + " " + lasttime.Substring(8, 2) + ":" + lasttime.Substring(10, 2) + ":" + lasttime.Substring(12, 2) + "." + lasttime.Substring(14, 3), out lastclinetime) == false)
                {
                    errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.自定义错误描述).ToString(), "clientsystime属性格式错误!");
                    return false;
                }

                DateTime nowclientime;
                if (DateTime.TryParse(cliensystemtime.Substring(0, 4) + "-" + cliensystemtime.Substring(4, 2) + "-" + cliensystemtime.Substring(6, 2) + " " + cliensystemtime.Substring(8, 2) + ":" + cliensystemtime.Substring(10, 2) + ":" + cliensystemtime.Substring(12, 2) + "." + cliensystemtime.Substring(14, 3), out nowclientime) == false)
                {
                    errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.自定义错误描述).ToString(), "clientsystime属性格式错误!");
                    return false;
                }

                if (nowclientime <= lastclinetime)
                {
                    errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.自定义错误描述).ToString(), "接口调用异常,调用方clientsystime错误,应大于该接口上次调用值!");
                    return false;
                }
            }

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@userno", userno);
            pars.Add("@token", token);
            pars.Add("@intertype", interfacetype);
            pars.Add("@cliensystime", cliensystemtime);
            if (SqlHelper.SqlNonQuery(sqlc, "InterfaceCallState_Update", pars) == -99)
            {
                errjson = CommFn.ErrorReturn(((int)Enums.ApiCodes.自定义错误描述).ToString(), "数据连接异常,更新接口调用状态失败!");
                return false;
            }

            return true;
        }

    }
}