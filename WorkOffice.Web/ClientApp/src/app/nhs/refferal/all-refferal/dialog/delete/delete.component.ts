import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ReferralModel } from '../../../referral.model';
import { ReferralService } from '../../../referral.service';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteReferralDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteReferralDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ReferralModel,
    public referralService: ReferralService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.referralService.deleteReferral(this.data.referralId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
