// 网站首页，书籍列表信息
import { Component, OnInit } from '@angular/core';
import { Book } from '../book';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})

export class BooksComponent implements OnInit {

  // 所有书籍列表信息
  books: Book[] = [
    {
      bno: '20180620001',
      btitle: 'hello world',
      briefintro: 'hello hello hello hello',
      bcover: '',
      btype: '',
      btypename: '分类1',
      bstatus: '',
      bstatusname: '连载',
    },
    {
      bno: '20180620002',
      btitle: 'hello hello',
      briefintro: 'world world world world',
      bcover: '',
      btype: '',
      btypename: '分类2',
      bstatus: '',
      bstatusname: '完结',
    }
  ];

  constructor() { }

  ngOnInit() {
  }

}
