/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllRTTComponent } from './all-rtt.component';

// import { AllRTTComponent } from '../../../all-apptype/all-apptype.component';

describe('AllRTTComponent', () => {
  let component: AllRTTComponent;
  let fixture: ComponentFixture<AllRTTComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllRTTComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllRTTComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
