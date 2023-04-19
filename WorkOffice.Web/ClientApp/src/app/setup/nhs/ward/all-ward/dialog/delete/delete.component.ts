import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { WardModel } from '../../../ward.model';
import { WardService } from '../../../ward.service';
// import { WardService } from '../../../../apptype.service';
// import { WardModel } from '../../../../apptype.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteWardDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteWardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WardModel,
    public WardService: WardService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.WardService.deleteWard(this.data.wardId).subscribe();
  }
}
