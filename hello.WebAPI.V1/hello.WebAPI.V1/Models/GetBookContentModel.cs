using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hello.WebAPI.V1.Models
{
    // 获取书籍每章内容详情，入参信息
    public class GetBookContentModel : GetBooksModel
    {
    }
    public class GetBookContentResultModel
    {
        // 用户编号
        public string userno { get; set; }
        // 书籍章节内容详情
        public BookChapInfoModel chapinfo { get; set; }
    }

}