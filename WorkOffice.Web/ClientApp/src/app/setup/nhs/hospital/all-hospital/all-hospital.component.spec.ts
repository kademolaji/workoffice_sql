/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { AllHospitalComponent } from './all-hospital.component';

// import { AllHospitalComponent } from '../../../all-apptype/all-apptype.component';

describe('AllHospitalComponent', () => {
  let component: AllHospitalComponent;
  let fixture: ComponentFixture<AllHospitalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllHospitalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllHospitalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
