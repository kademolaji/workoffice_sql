/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllPathwayStatusComponent } from './all-pathwaystatus.component';

// import { AllPathwayStatusComponent } from '../../../all-pathwaystatus/all-pathwaystatus.component';

describe('AllPathwayStatusComponent', () => {
  let component: AllPathwayStatusComponent;
  let fixture: ComponentFixture<AllPathwayStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllPathwayStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllPathwayStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
