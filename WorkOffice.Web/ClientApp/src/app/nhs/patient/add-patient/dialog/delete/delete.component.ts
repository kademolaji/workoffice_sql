import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { PatientService } from '../../../patient.service';
import { PatientDocumentModel } from '../../../patient.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeletePatientDocumentDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeletePatientDocumentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PatientDocumentModel,
    public patientService: PatientService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.patientService.deletePatientDocument(this.data.patientId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
