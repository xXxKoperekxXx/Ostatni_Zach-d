import { TestBed } from '@angular/core/testing';

import { ActiveUsersServiceService } from './active-users-service.service';

describe('ActiveUsersServiceService', () => {
  let service: ActiveUsersServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ActiveUsersServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
