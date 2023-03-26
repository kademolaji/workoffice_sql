import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { UserRoleAndActivityModel } from '../../../user-role.model';
import { UserRoleService } from '../../../user-role.service';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteUserRoleDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteUserRoleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserRoleAndActivityModel,
    public userRoleService: UserRoleService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.userRoleService.deleteUserRole(this.data.userRoleAndActivityId).subscribe();
  }
}
