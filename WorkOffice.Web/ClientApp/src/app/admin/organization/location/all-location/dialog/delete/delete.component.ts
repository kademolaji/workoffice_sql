import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { LocationService } from '../../../location.service';

export interface DialogData {
  id: number;
  name: string;
  designation: string;
  mobile: string;
}

@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteLocationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteLocationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public locationService: LocationService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.locationService.deleteLocation(this.data.id);
  }
}
