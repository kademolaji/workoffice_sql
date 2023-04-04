import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { LocationService } from '../../../location.service';
import { LocationModel } from '../../../location.model';


@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteLocationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteLocationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: LocationModel,
    public locationService: LocationService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.locationService.deleteLocation(this.data.locationId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
