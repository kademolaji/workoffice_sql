import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { PathwayStatusModel } from '../../../pathwaystatus.model';
import { PathwayStatusService } from '../../../pathwaystatus.service';
// import { PathwayStatusService } from '../../../../pathwaystatus.service';
// import { PathwayStatusModel } from '../../../../pathwaystatus.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeletePathwayStatusDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeletePathwayStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PathwayStatusModel,
    public PathwayStatusService: PathwayStatusService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.PathwayStatusService.deletePathwayStatus(this.data.pathwayStatusId).subscribe();
  }
}
