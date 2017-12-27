using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Models
{
    // 获取书籍所有章节列表，入参信息
    public class GetBookChapsModel : GetBooksModel
    {
    }

    public class GetBookChapsResultModel
    {
        // 用户编号
        public string userno { get; set; }
        // 书籍基本信息
        public BookInfoModel bookinfo { get; set; }
        // 书籍各章节基本信息列表
        public List<BookChapInfoModel> infolist { get; set; }
    }

    public class BookChapInfoModel
    {
        // 书籍编号
        public string bno { get; set; }
        // 书籍名称
        public string bname { get; set; }
        // 章节序号
        public string chapindex { get; set; }
        // 章节名称
        public string chapname { get; set; }
        // 章节内容
        public string chapcontent { get; set; }
        // 录入时间
        public string instime { get; set; }
    }
}