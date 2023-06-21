import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { UsersService } from '../../users.service';
import { UserListModel } from '../../users.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserListModel,
    public usersService: UsersService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.usersService.activateDeactivateUser(this.data.userId).subscribe((data)=>{
        if(data.status){
          this.dialogRef.close();
        }
      });
    }
}
