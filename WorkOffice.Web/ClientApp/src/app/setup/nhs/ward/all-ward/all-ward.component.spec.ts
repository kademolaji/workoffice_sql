/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllWardComponent } from './all-ward.component';

// import { AllWardComponent } from '../../../all-apptype/all-apptype.component';

describe('AllWardComponent', () => {
  let component: AllWardComponent;
  let fixture: ComponentFixture<AllWardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllWardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllWardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
