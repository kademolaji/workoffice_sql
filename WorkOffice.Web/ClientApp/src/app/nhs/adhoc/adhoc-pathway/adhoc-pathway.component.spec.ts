/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdhocPathwayComponent } from './adhoc-pathway.component';

describe('AdhocPathwayComponent', () => {
  let component: AdhocPathwayComponent;
  let fixture: ComponentFixture<AdhocPathwayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdhocPathwayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdhocPathwayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
