import { TestBed, inject } from '@angular/core/testing';

import { BookchapsService } from './bookchaps.service';

describe('BookchapsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BookchapsService]
    });
  });

  it('should be created', inject([BookchapsService], (service: BookchapsService) => {
    expect(service).toBeTruthy();
  }));
});
