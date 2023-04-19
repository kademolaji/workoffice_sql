/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AllWaitinglistComponent } from './all-waitinglist.component';

describe('AllWaitinglistComponent', () => {
  let component: AllWaitinglistComponent;
  let fixture: ComponentFixture<AllWaitinglistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllWaitinglistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllWaitinglistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
