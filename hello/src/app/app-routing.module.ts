import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BooksComponent } from 'app/books/books.component';
import { BookchaptersComponent } from 'app/bookchapters/bookchapters.component';

const routes: Routes = [
  { path: 'books', component: BooksComponent },
  { path: 'bookchapters', component: BookchaptersComponent }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }