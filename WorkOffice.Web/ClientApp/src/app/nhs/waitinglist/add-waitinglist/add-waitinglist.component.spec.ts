/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AddWaitinglistComponent } from './add-waitinglist.component';

describe('AddWaitinglistComponent', () => {
  let component: AddWaitinglistComponent;
  let fixture: ComponentFixture<AddWaitinglistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddWaitinglistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddWaitinglistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
