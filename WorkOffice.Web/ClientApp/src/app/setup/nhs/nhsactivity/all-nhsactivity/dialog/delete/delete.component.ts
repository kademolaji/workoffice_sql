import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { NHSActivityModel } from '../../../nhsactivity.model';
import { NHSActivityService } from '../../../nhsactivity.service';
// import { NHSActivityService } from '../../../../nhsactivity.service';
// import { NHSActivityModel } from '../../../../nhsactivity.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteNHSActivityDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteNHSActivityDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NHSActivityModel,
    public NHSActivityService: NHSActivityService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.NHSActivityService.deleteNHSActivity(this.data.nhsActivityId).subscribe();
  }
}
