import { Component, OnInit } from '@angular/core';
import { BOOKS } from '../mock-books';
import { Bookinfo } from '../bookinfo';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit {
  books = BOOKS;
  selectedBook: Bookinfo;

  constructor() { }

  ngOnInit() {
  }

  onSelect(book: Bookinfo): void {
    this.selectedBook = book;
  }
}
