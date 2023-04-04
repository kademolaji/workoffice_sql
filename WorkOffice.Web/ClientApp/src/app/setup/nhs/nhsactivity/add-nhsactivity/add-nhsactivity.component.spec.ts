/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AddNHSActivityComponent } from './add-nhsactivity.component';

describe('AddNHSActivityComponent', () => {
  let component: AddNHSActivityComponent;
  let fixture: ComponentFixture<AddNHSActivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddNHSActivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddNHSActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
