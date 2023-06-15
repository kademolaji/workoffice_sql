/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PathwayAdhocComponent } from './pathway-adhoc.component';

describe('PathwayAdhocComponent', () => {
  let component: PathwayAdhocComponent;
  let fixture: ComponentFixture<PathwayAdhocComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PathwayAdhocComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PathwayAdhocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
