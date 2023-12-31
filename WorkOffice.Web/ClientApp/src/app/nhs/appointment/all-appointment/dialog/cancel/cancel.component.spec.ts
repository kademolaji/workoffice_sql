import { ComponentFixture, TestBed, waitForAsync } from "@angular/core/testing";
import { CancelDialogComponent } from "./cancel.component";
describe("CancelComponent", () => {
  let component: CancelDialogComponent;
  let fixture: ComponentFixture<CancelDialogComponent>;
  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [CancelDialogComponent],
      }).compileComponents();
    })
  );
  beforeEach(() => {
    fixture = TestBed.createComponent(CancelDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
