import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { WaitinglistModel } from '../../../watinglist.model';
import { WaitinglistService } from '../../../waitinglist.service';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteWaitinglistDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteWaitinglistDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WaitinglistModel,
    public waitinglistService: WaitinglistService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.waitinglistService.deleteWaitinglist(this.data.waitinglistId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
