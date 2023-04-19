/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AddRTTComponent } from './add-rtt.component';

describe('AddRTTComponent', () => {
  let component: AddRTTComponent;
  let fixture: ComponentFixture<AddRTTComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddRTTComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddRTTComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
