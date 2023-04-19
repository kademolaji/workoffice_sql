import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { PatientService } from '../../../patient.service';
import { PatientModel } from '../../../patient.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeletePatientDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeletePatientDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PatientModel,
    public patientService: PatientService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.patientService.deletePatient(this.data.patientId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
