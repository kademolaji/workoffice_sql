/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllWaitingTypeComponent } from './all-waitingtype.component';

// import { AllWaitingTypeComponent } from '../../../all-waitingtype/all-waitingtype.component';

describe('AllWaitingTypeComponent', () => {
  let component: AllWaitingTypeComponent;
  let fixture: ComponentFixture<AllWaitingTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllWaitingTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllWaitingTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
