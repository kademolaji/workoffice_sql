import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { AppointmentService } from '../../../appointment.service';
import {  CreateAppointmentModel } from '../../../appointment.model';



@Component({
  selector: 'app-cancel',
  templateUrl: './cancel.component.html',
  styleUrls: ['./cancel.component.sass'],
})
export class CancelAppointmentDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<CancelAppointmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateAppointmentModel,
    public appointmentService: AppointmentService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmCancel(): void {
    this.appointmentService.cancelAppointment(this.data.appointmentId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
