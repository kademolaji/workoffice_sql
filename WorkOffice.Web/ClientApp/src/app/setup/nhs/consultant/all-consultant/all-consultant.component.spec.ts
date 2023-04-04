/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllConsultantComponent } from './all-consultant.component';

// import { AllConsultantComponent } from '../../../all-apptype/all-apptype.component';

describe('AllConsultantComponent', () => {
  let component: AllConsultantComponent;
  let fixture: ComponentFixture<AllConsultantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllConsultantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllConsultantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
