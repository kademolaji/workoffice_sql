import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { AppointmentService } from '../../../appointment.service';
import {  CreateAppointmentModel } from '../../../appointment.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteAppointmentDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteAppointmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateAppointmentModel,
    public appointmentService: AppointmentService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.appointmentService.deleteAppointment(this.data.appointmentId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
