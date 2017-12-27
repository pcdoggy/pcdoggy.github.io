using hello.WebAPI.V1.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Util
{
    // 书籍相关
    public class EBooks
    {
        /// <summary>
        /// 2.获取书名列表
        /// </summary>
        /// <param name="ver"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JObject GetBooks(string ver, JObject data)
        {
            #region 校验参数是否完整

            string userno = CommFn.GetJsonItemValue<string>(data, "userno");
            if (userno == null || userno.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供userno参数).ToString(), Enums.ApiCodes.未提供userno参数.ToString());
            }

            string usertoken = CommFn.GetJsonItemValue<string>(data, "token");
            if (usertoken == null || usertoken.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供token参数).ToString(), Enums.ApiCodes.未提供token参数.ToString());
            }

            string cliensystemtime = CommFn.GetJsonItemValue<string>(data, "clientsystime");
            if (cliensystemtime == null || cliensystemtime.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供clientsystime参数).ToString(), Enums.ApiCodes.未提供clientsystime参数.ToString());
            }

            #endregion

            SqlConnection sqlc = SqlHelper.GetConnection();
            if (sqlc == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接失败).ToString(), Enums.ApiCodes.数据连接失败.ToString());
            }

            JObject errjson = new JObject();
            if (ApiHelper.CheckToken(sqlc, userno, usertoken, ref errjson) == false)//校验令牌
            {
                return errjson;
            }

            errjson = new JObject();
            if (ApiHelper.CheckInterFaceCall(sqlc, userno, usertoken, "GetBooks", cliensystemtime, ref errjson) == false)//校验接口调用的客户端时间,防止同一请求被重复调用(预防重放式攻击)
            {
                return errjson;
            }

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@userno", userno);

            DataSet ds = SqlHelper.SqlQuery(sqlc, "App_GetSJArea", pars);
            if (ds == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接异常).ToString(), Enums.ApiCodes.数据连接异常.ToString() + "!获取书名列表信息失败!");
            }

            JObject resualjson = new JObject();
            resualjson.Add("userno", userno);
            JArray formarray = new JArray();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                formarray.Add(dr["bno"].ToString());
            }
            resualjson.Add("infolist", formarray);

            return CommFn.SuccessReturn(((int)Enums.ApiCodes.成功).ToString(), "获取书名列表信息成功!", resualjson);
        }

        /// <summary>
        /// 3.获取书籍所有章节列表
        /// </summary>
        /// <param name="ver"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JObject GetBookChaps(string ver, JObject data)
        {
            #region 校验参数是否完整

            string userno = CommFn.GetJsonItemValue<string>(data, "userno");
            if (userno == null || userno.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供userno参数).ToString(), Enums.ApiCodes.未提供userno参数.ToString());
            }

            string usertoken = CommFn.GetJsonItemValue<string>(data, "token");
            if (usertoken == null || usertoken.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供token参数).ToString(), Enums.ApiCodes.未提供token参数.ToString());
            }

            string cliensystemtime = CommFn.GetJsonItemValue<string>(data, "clientsystime");
            if (cliensystemtime == null || cliensystemtime.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供clientsystime参数).ToString(), Enums.ApiCodes.未提供clientsystime参数.ToString());
            }

            #endregion

            SqlConnection sqlc = SqlHelper.GetConnection();
            if (sqlc == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接失败).ToString(), Enums.ApiCodes.数据连接失败.ToString());
            }

            JObject errjson = new JObject();
            if (ApiHelper.CheckToken(sqlc, userno, usertoken, ref errjson) == false)//校验令牌
            {
                return errjson;
            }

            errjson = new JObject();
            if (ApiHelper.CheckInterFaceCall(sqlc, userno, usertoken, "GetBookChaps", cliensystemtime, ref errjson) == false)//校验接口调用的客户端时间,防止同一请求被重复调用(预防重放式攻击)
            {
                return errjson;
            }

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@userno", userno);

            DataSet ds = SqlHelper.SqlQuery(sqlc, "App_GetSJArea", pars);
            if (ds == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接异常).ToString(), Enums.ApiCodes.数据连接异常.ToString() + "!获取书籍章节列表信息失败!");
            }

            JObject resualjson = new JObject();
            resualjson.Add("userno", userno);
            JArray formarray = new JArray();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                formarray.Add(dr["bno"].ToString());
            }
            resualjson.Add("infolist", formarray);

            return CommFn.SuccessReturn(((int)Enums.ApiCodes.成功).ToString(), "获取书籍章节列表信息成功!", resualjson);
        }

        /// <summary>
        /// 4.获取书籍每章内容详情
        /// </summary>
        /// <param name="ver"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JObject GetBookContent(string ver, JObject data)
        {
            #region 校验参数是否完整

            string userno = CommFn.GetJsonItemValue<string>(data, "userno");
            if (userno == null || userno.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供userno参数).ToString(), Enums.ApiCodes.未提供userno参数.ToString());
            }

            string usertoken = CommFn.GetJsonItemValue<string>(data, "token");
            if (usertoken == null || usertoken.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供token参数).ToString(), Enums.ApiCodes.未提供token参数.ToString());
            }

            string cliensystemtime = CommFn.GetJsonItemValue<string>(data, "clientsystime");
            if (cliensystemtime == null || cliensystemtime.Trim() == "")
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.未提供clientsystime参数).ToString(), Enums.ApiCodes.未提供clientsystime参数.ToString());
            }

            #endregion

            SqlConnection sqlc = SqlHelper.GetConnection();
            if (sqlc == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接失败).ToString(), Enums.ApiCodes.数据连接失败.ToString());
            }

            JObject errjson = new JObject();
            if (ApiHelper.CheckToken(sqlc, userno, usertoken, ref errjson) == false)//校验令牌
            {
                return errjson;
            }

            errjson = new JObject();
            if (ApiHelper.CheckInterFaceCall(sqlc, userno, usertoken, "GetBookContent", cliensystemtime, ref errjson) == false)//校验接口调用的客户端时间,防止同一请求被重复调用(预防重放式攻击)
            {
                return errjson;
            }

            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@userno", userno);

            DataSet ds = SqlHelper.SqlQuery(sqlc, "App_GetSJArea", pars);
            if (ds == null)
            {
                return CommFn.ErrorReturn(((int)Enums.ApiCodes.数据连接异常).ToString(), Enums.ApiCodes.数据连接异常.ToString() + "!获取章节内容详情失败!");
            }

            JObject resualjson = new JObject();
            resualjson.Add("userno", userno);
            JArray formarray = new JArray();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                formarray.Add(dr["bno"].ToString());
            }
            resualjson.Add("infolist", formarray);

            return CommFn.SuccessReturn(((int)Enums.ApiCodes.成功).ToString(), "获取章节内容详情成功!", resualjson);
        }

    }
}