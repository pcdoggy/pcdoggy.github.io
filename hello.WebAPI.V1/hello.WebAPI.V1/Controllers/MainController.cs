using hello.WebAPI.V1.Common;
using hello.WebAPI.V1.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace hello.WebAPI.V1.Controllers
{
    public class MainController : ApiController
    {
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
                                code = "1005";
                                errormsg = "未提供appkey属性值";
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
                                code = "1005";
                                errormsg = "未提供timestamp属性值";
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
                                code = "1005";
                                errormsg = "未提供data属性值";
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
                                code = "1005";
                                errormsg = "未提供sign属性值";
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
                                code = "1005";
                                errormsg = "未提供ver属性值";
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

                        code = "1005";
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
                            code = "1001";
                            errormsg = "非法调用,时间戳格式不正确或时间戳异常!";
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
                                code = "9999";
                                errormsg = "数据连接失败!";
                                ReturnJson.Add("Success", false);
                                ReturnJson.Add("Code", code);
                                ReturnJson.Add("Message", errormsg);
                                ReturnJson.Add("Result", null);
                            }
                            else if (checkappkey == -2)
                            {
                                code = "1001";
                                errormsg = "非法调用,AppKey不合法!";
                                ReturnJson.Add("Success", false);
                                ReturnJson.Add("Code", code);
                                ReturnJson.Add("Message", errormsg);
                                ReturnJson.Add("Result", null);
                            }
                            else
                            {
                                code = "1001";
                                errormsg = "非法调用,数据签名sign不合法!";
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
                            code = "1005";
                            errormsg = "未指定调用的接口类型或未传入接口参数!";
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
                Log.WriteLog(ex.Message);

                code = "1004";
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
