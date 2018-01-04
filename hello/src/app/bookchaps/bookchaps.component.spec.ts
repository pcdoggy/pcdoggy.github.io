import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookchapsComponent } from './bookchaps.component';

describe('BookchapsComponent', () => {
  let component: BookchapsComponent;
  let fixture: ComponentFixture<BookchapsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookchapsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookchapsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
