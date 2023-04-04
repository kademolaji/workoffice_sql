import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { RTTModel } from '../../../rtt.model';
import { RTTService } from '../../../rtt.service';
// import { RTTService } from '../../../../apptype.service';
// import { RTTModel } from '../../../../apptype.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteRTTDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteRTTDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RTTModel,
    public RTTService: RTTService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.RTTService.deleteRTT(this.data.rttId).subscribe();
  }
}
