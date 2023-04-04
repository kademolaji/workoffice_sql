/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AddConsultantComponent } from './add-consultant.component';

describe('AddConsultantComponent', () => {
  let component: AddConsultantComponent;
  let fixture: ComponentFixture<AddConsultantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddConsultantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddConsultantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
