using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Models
{
    // 业务接口入参信息
    public class DatasInterfaceModel
    {
        // 时间戳,格式为(年月日时分秒),例如:20160901110304
        public string timestamp { get; set; }
        // 签名，数据签名,签名算法为 MD5(timestamp +data)后Base64编码
        public string sign { get; set; }
        // 版本号
        public string ver { get; set; }
        // 数据详情
        public string data { get; set; }
    }

    // 接口返回结果信息
    public class ResultModel
    {
        // 成功或失败标志,true表示成功,false表示失败
        public bool Success { get; set; }
        // 消息代码(详细信息见文档附件的消息代码列表)
        public string Code { get; set; }
        // 返回消息,失败时返回失败描述
        public string Message { get; set; }
        // 返回结果, UTF-8格式的JSON字符串(内容因接口类型不同而有差异,具体见各接口类型调用说明) 经过Base64位编码后得到的字符串,Success为false时,Result为null
        public string Result { get; set; }
    }
}