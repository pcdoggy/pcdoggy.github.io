using hello.WebAPI.V1.Common;
using hello.WebAPI.V1.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace hello.WebAPI.V1.Controllers
{
    public class MainController : ApiController
    {
        [HttpPost]
        public async Task<JsonResult> UserLogin()
        {
            string code = "";
            string errormsg = "";
            string username = string.Empty, userpass = string.Empty, timestamp = string.Empty;

            // 入参校验
            Dictionary<string, string> listpar = new Dictionary<string, string>();
            listpar.Add("username", "");
            listpar.Add("userpass", "");
            listpar.Add("timestamp", "");

            JObject returnJson = new JObject();

            try
            {
                // multipart/formdata
                var vstream = await Request.Content.ReadAsMultipartAsync();
                var vcontents = vstream.Contents;
                foreach (var vcontent in vcontents)
                {
                    switch (vcontent.Headers.ContentDisposition.Name)
                    {
                        case "username":
                            username = await vcontent.ReadAsStringAsync();
                            if (username == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供username属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供username属性值.ToString();// "未提供appkey属性值";
                            }
                            else
                            {
                                listpar.Remove("username");
                            }
                            username = CommFn.RemoveEnterAndWrap(username);
                            break;
                        case "userpass":
                            userpass = await vcontent.ReadAsStringAsync();
                            if (userpass == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供userpass属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供userpass属性值.ToString();// "未提供appkey属性值";
                            }
                            else
                            {
                                listpar.Remove("userpass");
                            }
                            userpass = CommFn.RemoveEnterAndWrap(userpass);
                            break;
                        case "timestamp":
                            timestamp = await vcontent.ReadAsStringAsync();
                            if (timestamp == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供timestamp属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供timestamp属性值.ToString();// "未提供appkey属性值";
                            }
                            else
                            {
                                listpar.Remove("timestamp");
                            }
                            timestamp = CommFn.RemoveEnterAndWrap(timestamp);
                            break;
                    }
                    if (code != "")
                    {
                        break;
                    }
                }

                if (code != "")
                {
                    returnJson.Add("Success", false);
                    returnJson.Add("Code", code);
                    returnJson.Add("Message", errormsg);
                    returnJson.Add("Result", null);
                }
                else
                {
                    if (listpar.Count > 0)//参数不完整
                    {
                        string strs = "";
                        foreach (var par in listpar)
                        {
                            strs = strs + "[" + par.Key + "]";
                        }

                        code = ((int)Enums.ApiCodes.未提供属性值).ToString();// "1000";
                        errormsg = "参数" + strs + "未提供属性值";

                        returnJson.Add("Success", false);
                        returnJson.Add("Code", code);
                        returnJson.Add("Message", errormsg);
                        returnJson.Add("Result", null);
                    }
                    else//参数完整则进入正式处理流程
                    {
                        ApiModels rfm = new ApiModels();

                        if (rfm.CheckTimestamp(timestamp) == false)//校验timestamp
                        {
                            code = ((int)Enums.ApiCodes.非法调用timestamp).ToString();
                            errormsg = GetDescription.getdescription(Enums.ApiCodes.tokenout);// "非法调用,时间戳格式不正确或时间戳异常!";
                            returnJson.Add("Success", false);
                            returnJson.Add("Code", code);
                            returnJson.Add("Message", errormsg);
                            returnJson.Add("Result", null);
                            return new JsonResult(returnJson.ToString(Newtonsoft.Json.Formatting.Indented), this.Request);
                        }

                        //byte[] datas = Convert.FromBase64String(data);
                        //string datastr = Encoding.UTF8.GetString(datas);

                        //JObject datajson = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(datastr);
                        //string type = CommFn.GetJsonItemValue<string>(datajson, "Type");
                        //JObject pardata = CommFn.GetJsonItemValue<JObject>(datajson, "Data");
                        //if (type == null || pardata == null)
                        //{
                        //    code = ((int)Enums.ApiCodes.未指定调用的接口类型或未传入接口参数).ToString();// "1005";
                        //    errormsg = Enums.ApiCodes.未指定调用的接口类型或未传入接口参数.ToString(); // "未指定调用的接口类型或未传入接口参数!";
                        //    returnJson.Add("Success", false);
                        //    returnJson.Add("Code", code);
                        //    returnJson.Add("Message", errormsg);
                        //    returnJson.Add("Result", null);
                        //}
                        //else
                        //{
                        //    returnJson = rfm.CallInterFace(type, pardata, ver);//调用接口业务处理
                        //}
                        return new JsonResult(returnJson.ToString(Newtonsoft.Json.Formatting.Indented), this.Request);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("登录验证出错，原因为：" + ex.Message);

                code = ((int)Enums.ApiCodes.系统错误).ToString();
                errormsg = "发生错误!详细信息:" + ex.Message;

                returnJson.Add("Success", false);
                returnJson.Add("Code", code);
                returnJson.Add("Message", errormsg);
                returnJson.Add("Result", null);
            }

            return new JsonResult(returnJson.ToString(Newtonsoft.Json.Formatting.Indented), this.Request);
        }

        // API请求入口
        [HttpPost]
        public async Task<JsonResult> DatasInterface()
        {
            string code = "";
            string errormsg = "";
            string appkey = "";
            string timestamp = "";
            string data = "";
            string sign = "";
            string ver = "";

            // 入参校验
            Dictionary<string, string> Listpar = new Dictionary<string, string>();
            Listpar.Add("appkey", "");
            Listpar.Add("timestamp", "");
            Listpar.Add("data", "");
            Listpar.Add("sign", "");
            Listpar.Add("ver", "");

            JObject ReturnJson = new JObject();

            try
            {
                // multipart/formdata
                var streamProvider = await Request.Content.ReadAsMultipartAsync();
                var contents = streamProvider.Contents;
                foreach (HttpContent content in contents)
                {
                    switch (content.Headers.ContentDisposition.Name)
                    {
                        case "appkey":
                            appkey = await content.ReadAsStringAsync();
                            if (appkey == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供appkey属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供appkey属性值.ToString();// "未提供appkey属性值";
                            }
                            else
                            {
                                Listpar.Remove("appkey");
                            }
                            appkey = CommFn.RemoveEnterAndWrap(appkey);
                            break;
                        case "timestamp":
                            timestamp = await content.ReadAsStringAsync();
                            if (timestamp == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供timestamp属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供timestamp属性值.ToString();// "未提供timestamp属性值";
                            }
                            else
                            {
                                Listpar.Remove("timestamp");
                            }
                            timestamp = CommFn.RemoveEnterAndWrap(timestamp);
                            break;
                        case "data":
                            data = await content.ReadAsStringAsync();
                            if (data == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供data属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供data属性值.ToString();// "未提供data属性值";
                            }
                            else
                            {
                                Listpar.Remove("data");
                            }
                            data = CommFn.RemoveEnterAndWrap(data);
                            break;
                        case "sign":
                            sign = await content.ReadAsStringAsync();
                            if (sign == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供sign属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供sign属性值.ToString();// "未提供sign属性值";
                            }
                            else
                            {
                                Listpar.Remove("sign");
                            }
                            sign = CommFn.RemoveEnterAndWrap(sign);
                            break;
                        case "ver":
                            ver = await content.ReadAsStringAsync();
                            if (ver == null)
                            {
                                code = ((int)Enums.ApiCodes.未提供ver属性值).ToString();// "1005";
                                errormsg = Enums.ApiCodes.未提供ver属性值.ToString();// "未提供ver属性值";
                            }
                            else
                            {
                                Listpar.Remove("ver");
                            }
                            ver = CommFn.RemoveEnterAndWrap(ver);
                            break;
                    }
                    if (code != "")
                    {
                        break;
                    }
                }
                if (code != "")
                {
                    ReturnJson.Add("Success", false);
                    ReturnJson.Add("Code", code);
                    ReturnJson.Add("Message", errormsg);
                    ReturnJson.Add("Result", null);
                }
                else
                {
                    if (Listpar.Count > 0)//参数不完整
                    {
                        string strs = "";
                        foreach (var par in Listpar)
                        {
                            strs = strs + "[" + par.Key + "]";
                        }

                        code = ((int)Enums.ApiCodes.未提供属性值).ToString();// "1000";
                        errormsg = "参数" + strs + "未提供属性值";

                        ReturnJson.Add("Success", false);
                        ReturnJson.Add("Code", code);
                        ReturnJson.Add("Message", errormsg);
                        ReturnJson.Add("Result", null);
                    }
                    else//参数完整则进入正式处理流程
                    {
                        ApiModels rfm = new ApiModels();

                        if (rfm.CheckTimestamp(timestamp) == false)//校验timestamp
                        {
                            code = ((int)Enums.ApiCodes.非法调用timestamp).ToString();
                            errormsg = GetDescription.getdescription(Enums.ApiCodes.tokenout);// "非法调用,时间戳格式不正确或时间戳异常!";
                            ReturnJson.Add("Success", false);
                            ReturnJson.Add("Code", code);
                            ReturnJson.Add("Message", errormsg);
                            ReturnJson.Add("Result", null);
                            return new JsonResult(ReturnJson.ToString(Newtonsoft.Json.Formatting.Indented), this.Request);
                        }

                        int checkappkey = rfm.CheckAppkeyAndSign(appkey, timestamp, data, sign);
                        if (checkappkey < 0)//校验appkey和sign
                        {
                            if (checkappkey == -1)
                            {
                                code = ((int)Enums.ApiCodes.自定义错误描述).ToString();// "9999";
                                errormsg = "数据连接失败!";
                                ReturnJson.Add("Success", false);
                                ReturnJson.Add("Code", code);
                                ReturnJson.Add("Message", errormsg);
                                ReturnJson.Add("Result", null);
                            }
                            else if (checkappkey == -2)
                            {
                                code = ((int)Enums.ApiCodes.非法调用appkey).ToString();// "1001";
                                errormsg = Enums.ApiCodes.非法调用appkey.ToString(); // "非法调用,AppKey不合法!";
                                ReturnJson.Add("Success", false);
                                ReturnJson.Add("Code", code);
                                ReturnJson.Add("Message", errormsg);
                                ReturnJson.Add("Result", null);
                            }
                            else
                            {
                                code = ((int)Enums.ApiCodes.非法调用sign).ToString();// "1001";
                                errormsg = Enums.ApiCodes.非法调用sign.ToString(); // "非法调用,数据签名sign不合法!";
                                ReturnJson.Add("Success", false);
                                ReturnJson.Add("Code", code);
                                ReturnJson.Add("Message", errormsg);
                                ReturnJson.Add("Result", null);
                            }
                            return new JsonResult(ReturnJson.ToString(Newtonsoft.Json.Formatting.Indented), this.Request);
                        }

                        byte[] datas = Convert.FromBase64String(data);
                        string datastr = Encoding.UTF8.GetString(datas);

                        JObject datajson = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(datastr);
                        string type = CommFn.GetJsonItemValue<string>(datajson, "Type");
                        JObject pardata = CommFn.GetJsonItemValue<JObject>(datajson, "Data");
                        if (type == null || pardata == null)
                        {
                            code = ((int)Enums.ApiCodes.未指定调用的接口类型或未传入接口参数).ToString();// "1005";
                            errormsg = Enums.ApiCodes.未指定调用的接口类型或未传入接口参数.ToString(); // "未指定调用的接口类型或未传入接口参数!";
                            ReturnJson.Add("Success", false);
                            ReturnJson.Add("Code", code);
                            ReturnJson.Add("Message", errormsg);
                            ReturnJson.Add("Result", null);
                        }
                        else
                        {
                            ReturnJson = rfm.CallInterFace(type, pardata, ver);//调用接口业务处理
                        }
                        return new JsonResult(ReturnJson.ToString(Newtonsoft.Json.Formatting.Indented), this.Request);
                    }
                }
            }
            catch (Exception ex)
            {
                //调用日志类的写日志的方法，将错误信息传入  
                Log.WriteLog("接口入口验证出错，原因为：" + ex.Message);

                code = ((int)Enums.ApiCodes.系统错误).ToString();
                errormsg = "发生错误!详细信息:" + ex.Message;

                ReturnJson.Add("Success", false);
                ReturnJson.Add("Code", code);
                ReturnJson.Add("Message", errormsg);
                ReturnJson.Add("Result", null);
            }

            return new JsonResult(ReturnJson.ToString(Newtonsoft.Json.Formatting.Indented), this.Request);
        }
    }
}
