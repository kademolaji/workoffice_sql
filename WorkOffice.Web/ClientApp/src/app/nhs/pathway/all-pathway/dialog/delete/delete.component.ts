import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { PathwayService } from '../../../pathway.service';
import {  CreatePathwayModel } from '../../../pathway.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeletePathwayDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeletePathwayDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreatePathwayModel,
    public pathwayService: PathwayService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.pathwayService.deletePathway(this.data.patientValidationId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
