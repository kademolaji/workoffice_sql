import { Component, Inject, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { PatientValidationDetailsModel } from 'src/app/nhs/Validate/validate.model';
import { ValidateNowService } from 'src/app/nhs/Validate/validate.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';

@Component({
  selector: 'app-pathway-adhoc-dialog',
  templateUrl: './pathway-adhoc-dialog.component.html',
  styleUrls: ['./pathway-adhoc-dialog.component.css'],
})
export class PathwayAdhocDialogComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  patientValidationDetailsForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  specialityList: GeneralSettingsModel[] = [];
  pathwayStatusList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];

  constructor(
    public dialogRef: MatDialogRef<PathwayAdhocDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PatientValidationDetailsModel,
    private fb: UntypedFormBuilder,
    private validateNowService: ValidateNowService,
    private generalSettingsService: GeneralSettingsService,
    private snackBar: MatSnackBar,
  ) {
    super();
  }
  ngOnInit() {
    this.patientValidationDetailsForm = this.fb.group({
      pathWayStatusId: ['', [Validators.required]],
      specialityId: ['', [Validators.required]],
      date: ['', [Validators.required]],
      consultantId: ['', [Validators.required]],
      endDate: [''],
      activity: ['', [Validators.required]],
    });
    if (this.data.patientValidationDetailsId > 0) {
      this.patientValidationDetailsForm.setValue({
        pathWayStatusId: this.data.pathWayStatusId,
        specialityId: this.data.specialityId,
        date: this.data.date,
        consultantId: this.data.consultantId,
        endDate: this.data.endDate  ? new Date(this.data.endDate) : '',
        activity: this.data.activity,
      });
    }
    this.subs.sink = this.generalSettingsService
    .getSpecialty()
    .subscribe((response) => {
      this.specialityList = response.entity;
    });
    this.subs.sink = this.generalSettingsService
    .getPathwayStatus()
    .subscribe((response) => {
      this.pathwayStatusList = response.entity;
    });
    this.subs.sink = this.generalSettingsService
      .getConsultant()
      .subscribe((response) => {
        this.consultantList = response.entity;
      });
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.patientValidationDetailsForm.invalid) {
      return;
    } else {
      const validationDetails: PatientValidationDetailsModel = {
        patientValidationDetailsId: this.data.patientValidationDetailsId,
        patientValidationId: this.data.patientValidationId,
        pathWayStatusId:
          this.patientValidationDetailsForm.value.pathWayStatusId,
        specialityId: this.patientValidationDetailsForm.value.specialityId,
        date: this.patientValidationDetailsForm.value.date,
        consultantId: this.patientValidationDetailsForm.value.consultantId,
        endDate: this.patientValidationDetailsForm.value.endDate,
        patientId: this.data.patientId,
        activity: this.patientValidationDetailsForm.value.activity,
        specialityCode: '',
        specialityName: '',
        pathWayStatusCode: '',
        pathWayStatusName: '',
        consultantCode: '',
        consultantName: '',
      };

      this.subs.sink = this.validateNowService
        .addPatientValidationDetails(validationDetails)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.loading = false;
              this.dialogRef.close();
              this.showNotification(
                'snackbar-success',
                'Added Record Successfully...!!!',
                'top',
                'right'
              );
            }
          },
          error: (error) => {
            this.submitted = false;
            this.loading = false;
            this.showNotification('snackbar-danger', error, 'top', 'right');
          },
        });
    }
  }

  onNoClick() {
    this.dialogRef.close();
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
