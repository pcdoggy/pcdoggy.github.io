using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Models
{
    public class UserLoginModel
    {
        // 用户账号。用户名或手机号码
        public string username { get; set; }
        // 用户密码。加密传输。
        public string userpass { get; set; }
        // 时间戳,格式为(年月日时分秒),例如:20160901110304
        public string timestamp { get; set; }
    }

    public class UserLoginResultModel
    {
        // 用户编号
        public string userno { get; set; }
        // 用户账号。用户名或手机号码
        public string username { get; set; }
        // 手机号
        public string telno { get; set; }
        // 令牌
        public string token { get; set; }
    }
}