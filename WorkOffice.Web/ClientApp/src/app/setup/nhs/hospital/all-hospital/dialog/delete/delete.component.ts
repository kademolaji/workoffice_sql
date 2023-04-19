import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { HospitalModel } from '../../../hospital.model';
import { HospitalService } from '../../../hospital.service';
// import { HospitalService } from '../../../../apptype.service';
// import { HospitalModel } from '../../../../apptype.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteHospitalDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteHospitalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: HospitalModel,
    public HospitalService: HospitalService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.HospitalService.deleteHospital(this.data.hospitalId).subscribe();
  }
}
