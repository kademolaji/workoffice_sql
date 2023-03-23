import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { CustomIdentityFormatService } from '../../../custom-identity-format.service';
import { CustomIdentityFormatModel } from '../../../custom-identity-format.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteCustomIdentitySettingsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteCustomIdentitySettingsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CustomIdentityFormatModel,
    public customIdentityFormatService: CustomIdentityFormatService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.customIdentityFormatService.deleteCustomIdentityFormat(this.data.customIdentityFormatSettingId).subscribe();
  }
}
