import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ValidateNowService } from 'src/app/nhs/Validate/validate.service';
import { PatientValidationDetailsModel } from 'src/app/nhs/Validate/validate.model';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeletePatientValidationDetailsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeletePatientValidationDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PatientValidationDetailsModel,
    public validateNowService: ValidateNowService,
    private snackBar: MatSnackBar,
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.validateNowService.deletePatientValidationDetails(this.data.patientValidationDetailsId).subscribe(
      {
        next: (data)=>{
          if(data.status){
            this.dialogRef.close();
            this.showNotification(
              'snackbar-success',
              'Delete Record Successfully...!!!',
              'top',
              'right'
            );
          }
        },
        error: (error) => {
          this.showNotification('snackbar-danger', error, 'top', 'right');
        },
      });
  }

  showNotification(
    colorName: string,
    text: string,
    placementFrom: MatSnackBarVerticalPosition,
    placementAlign: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }
}
