import { TestBed } from '@angular/core/testing';

import { CompetitorService } from './competitor.service';

describe('CompetitorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CompetitorService = TestBed.get(CompetitorService);
    expect(service).toBeTruthy();
  });
});
