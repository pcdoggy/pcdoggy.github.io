import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookchaptersComponent } from './bookchapters.component';

describe('BookchaptersComponent', () => {
  let component: BookchaptersComponent;
  let fixture: ComponentFixture<BookchaptersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookchaptersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookchaptersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
