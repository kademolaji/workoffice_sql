import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ConsultantModel } from '../../../consultant.model';
import { ConsultantService } from '../../../consultant.service';
// import { ConsultantService } from '../../../../apptype.service';
// import { ConsultantModel } from '../../../../apptype.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteConsultantDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteConsultantDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConsultantModel,
    public ConsultantService: ConsultantService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.ConsultantService.deleteConsultant(this.data.consultantId).subscribe();
  }
}
