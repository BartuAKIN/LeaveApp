import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveType } from './leave-type';

describe('LeaveType', () => {
  let component: LeaveType;
  let fixture: ComponentFixture<LeaveType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeaveType],
    }).compileComponents();

    fixture = TestBed.createComponent(LeaveType);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
