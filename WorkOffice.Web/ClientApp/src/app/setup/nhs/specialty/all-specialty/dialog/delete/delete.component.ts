import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { SpecialtyModel } from '../../../specialty.model';
import { SpecialtyService } from '../../../specialty.service';
// import { SpecialtyModel } from '../../../../apptype.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteSpecialtyDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteSpecialtyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SpecialtyModel,
    public SpecialtyService: SpecialtyService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.SpecialtyService.deleteSpecialty(this.data.specialtyId).subscribe();
  }
}
