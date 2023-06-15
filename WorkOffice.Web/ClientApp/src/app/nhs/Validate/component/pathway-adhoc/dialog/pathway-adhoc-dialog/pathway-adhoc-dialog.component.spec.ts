/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PathwayAdhocDialogComponent } from './pathway-adhoc-dialog.component';

describe('PathwayAdhocDialogComponent', () => {
  let component: PathwayAdhocDialogComponent;
  let fixture: ComponentFixture<PathwayAdhocDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PathwayAdhocDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PathwayAdhocDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
