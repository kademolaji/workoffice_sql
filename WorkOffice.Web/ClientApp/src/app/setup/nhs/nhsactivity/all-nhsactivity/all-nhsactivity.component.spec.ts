/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllNHSActivityComponent } from './all-nhsactivity.component';

// import { AllNHSActivityComponent } from '../../../all-nhsactivity/all-nhsactivity.component';

describe('AllNHSActivityComponent', () => {
  let component: AllNHSActivityComponent;
  let fixture: ComponentFixture<AllNHSActivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllNHSActivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllNHSActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
