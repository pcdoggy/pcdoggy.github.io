using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Models
{
    // 获取书名列表，入参信息
    public class GetBooksModel
    {
        // 用户编号
        public string userno { get; set; }
        // 令牌
        public string token { get; set; }
        // 客户端系统时间
        public string clientsystime { get; set; }
    }

    // 获取书名列表，结果信息
    public class GetBooksResultModel
    {
        // 用户编号
        public string userno { get; set; }
        // 书籍基本信息列表
        public List<BookInfoModel> infolist { get; set; }

    }

    // 书籍基本信息
    public class BookInfoModel
    {
        // 书籍编号
        public string bno { get; set; }
        // 书籍名称
        public string bname { get; set; }
        // 书籍链接
        public string burl { get; set; }
        // 作者
        public string author { get; set; }
        // 简介
        public string briefintro { get; set; }
        // 章节总数
        public string chapcount { get; set; }
        // 记录人
        public string insuser { get; set; }
        // 记录时间
        public string instime { get; set; }
        // 更新人
        public string upduser { get; set; }
        // 更新时间
        public string updtime { get; set; }
    }

}