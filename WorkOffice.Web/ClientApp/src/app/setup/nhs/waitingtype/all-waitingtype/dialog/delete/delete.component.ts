import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { WaitingTypeModel } from '../../../waitingtype.model';
import { WaitingTypeService } from '../../../waitingtype.service';
// import { WaitingTypeService } from '../../../../waitingtype.service';
// import { WaitingTypeModel } from '../../../../waitingtype.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteWaitingTypeDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteWaitingTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WaitingTypeModel,
    public WaitingTypeService: WaitingTypeService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.WaitingTypeService.deleteWaitingType(this.data.waitingTypeId).subscribe();
  }
}
