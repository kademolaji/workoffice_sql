/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllSpecialtyComponent } from './all-specialty.component';

// import { AllSpecialtyComponent } from '../../../all-apptype/all-apptype.component';

describe('AllSpecialtyComponent', () => {
  let component: AllSpecialtyComponent;
  let fixture: ComponentFixture<AllSpecialtyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllSpecialtyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllSpecialtyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
