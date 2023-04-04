import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { AppTypeModel } from '../../../apptype.model';
import { AppTypeService } from '../../../apptype.service';
// import { AppTypeService } from '../../../../apptype.service';
// import { AppTypeModel } from '../../../../apptype.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteAppTypeDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteAppTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AppTypeModel,
    public AppTypeService: AppTypeService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.AppTypeService.deleteAppType(this.data.appTypeId).subscribe();
  }
}
