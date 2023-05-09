/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AllRefferalComponent } from './all-refferal.component';

describe('AllRefferalComponent', () => {
  let component: AllRefferalComponent;
  let fixture: ComponentFixture<AllRefferalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllRefferalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllRefferalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
