using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace hello.WebAPI.V1.Common
{
    public class Enums
    {
        // 接口调用，结果中code对应信息
        public enum ApiCodes
        {
            // 成功
            成功 = 0000,

            // 参数规则验证
            非法调用 = 1000,
            [Description("非法调用,时间戳格式不正确或时间戳异常!")]
            非法调用timestamp = 1001,// Appkey不合法或签名不合法,时间戳与当前服务器时间差异过大
            [Description("非法调用,Appkey不合法")]
            非法调用appkey = 1002,
            [Description("非法调用,数据签名sign不合法!")]
            非法调用sign = 1003,

            // 参数值存在与否验证
            未提供属性值 = 1010,
            未提供appkey属性值 = 1011,
            未提供timestamp属性值 = 1012,
            未提供data属性值 = 1013,
            未提供sign属性值 = 1014,
            未提供ver属性值 = 1015,
            未提供username属性值 = 1016,
            未提供userpass属性值 = 1017,

            // 授权相关验证
            [Description("未经授权的非法使用,需申请授权")]
            authoration = 1020,
            授权已过期 = 1021,
            连接站点已超出授权 = 1022,

            // 用户信息验证
            用户名或密码错误 = 1030,
            [Description("令牌已失效,请重新登录")]
            tokenout = 1031,
            用户信息失效 = 1032,
            用户信息不存在 = 1033,

            // 接口系统基本信息相关
            未指定调用的接口类型或未传入接口参数 = 1040,
            不支持的接口类型 = 1041,
            未知的接收系统 = 1042,
            数据接收失败 = 1043,

            系统错误 = 8000,

            自定义错误描述 = 9999,
        }

        public enum ApiTypes
        {
            // 用户登录
            UserLogin = 1000,

            // 获取书名列表
            GetBooks = 1010,
            // 获取书籍所有章节列表
            GetBookChaps = 1011,
            // 获取书籍每章内容详情
            GetBookContent = 1012,

        }
    }

    public static class GetDescription
    {
        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="en">枚举值</param>
        /// <param name="nameInstend">当枚举没有定义DescriptionAttribute,是否用枚举名代替，默认使用</param>
        /// <returns></returns>
        public static string getdescription(this Enum en, bool nameInstend = true)
        {
            Type type = en.GetType();
            string name = Enum.GetName(type, en);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }
    }
}