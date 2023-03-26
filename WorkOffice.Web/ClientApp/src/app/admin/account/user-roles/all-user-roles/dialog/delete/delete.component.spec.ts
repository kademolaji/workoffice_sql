import { ComponentFixture, TestBed, waitForAsync } from "@angular/core/testing";
import { DeleteUserRoleDialogComponent } from "./delete.component";
describe("DeleteComponent", () => {
  let component: DeleteUserRoleDialogComponent;
  let fixture: ComponentFixture<DeleteUserRoleDialogComponent>;
  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [DeleteUserRoleDialogComponent],
      }).compileComponents();
    })
  );
  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteUserRoleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
