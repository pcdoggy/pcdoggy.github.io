import { Chapinfo } from './chapinfo';

// 书籍章节列表信息
export class Bookchaps {
    // 用户编号
    userno: string;
    // 书籍编号
    bno: string;
    // 书籍名称
    bname: string;
    // 书籍链接
    burl: string;
    // 作者
    author: string;
    // 简介
    briefintro: string;
    // 章节总数
    chapcount: number;
    // 记录时间
    instime: string;
    // 更新时间
    updtime: string;
    // 章节内容详情
    infolist: Chapinfo[];
}
