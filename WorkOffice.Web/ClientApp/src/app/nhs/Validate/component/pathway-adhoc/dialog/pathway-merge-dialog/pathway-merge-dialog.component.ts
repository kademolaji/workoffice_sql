import { Component, Inject, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { filter, distinctUntilChanged, debounceTime, tap, switchMap, catchError, finalize } from 'rxjs';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { MergePathwayModel, PatientValidationDetailsModel } from 'src/app/nhs/Validate/validate.model';
import { ValidateNowService } from 'src/app/nhs/Validate/validate.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';

@Component({
  selector: 'app-pathway-merge-dialog',
  templateUrl: './pathway-merge-dialog.component.html',
  styleUrls: ['./pathway-merge-dialog.component.css']
})
export class PathwayMergeDialogComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
mergePatwayForm!: UntypedFormGroup;
submitted = false;
loading = false;
isAddMode = true;
id = 0;
minLengthTerm=3;
isLoading = false;
pathwayList: GeneralSettingsModel[] = [];

constructor(
  public dialogRef: MatDialogRef<PathwayMergeDialogComponent>,
  @Inject(MAT_DIALOG_DATA) public data: PatientValidationDetailsModel,
  private fb: UntypedFormBuilder,
  private validateNowService: ValidateNowService,
  private generalSettingsService: GeneralSettingsService,
  private snackBar: MatSnackBar,
  private router: Router,

) {
  super();
}
ngOnInit() {
  this.mergePatwayForm = this.fb.group({
    patientValidationId: ['', [Validators.required]],
  });

  this.subs.sink = this.generalSettingsService
  .getPathWayListByPatientId(this.data.patientId)
  .subscribe((response) => {
    this.pathwayList = response.entity;
  });

  this.mergePatwayForm.get('patientValidationId')?.valueChanges
  .pipe(
    filter(res => {
      return res !== null && res.length >= this.minLengthTerm
    }),
    distinctUntilChanged(),
    debounceTime(1000),
    tap(() => {
      this.pathwayList = [];
      this.isLoading = true;
    }),
    switchMap(value => this.generalSettingsService.getPathWayListByPatientId(this.data.patientId, value as string).pipe(catchError((err) => this.router.navigateByUrl('/')))
      .pipe(
        finalize(() => {
          this.isLoading = false
        }),
      )
    )
  )
  .subscribe((data: any) => {
    if (data['entity'] == undefined) {
      this.pathwayList = [];
    } else {
      this.pathwayList = data['entity'];
    }
  });
}

displayWith(value: any) {
  return value?.label;
}

onSubmit() {
  this.submitted = true;
  this.loading = true;
  // stop here if form is invalid
  if (this.mergePatwayForm.invalid) {
    return;
  } else {
    const mergeData: MergePathwayModel = {
      patientValidationDetailsId: this.data.patientValidationDetailsId,
      patientValidationId: this.mergePatwayForm.value.patientValidationId.value,
    };

    this.subs.sink = this.validateNowService
      .mergePatientValidationDetails(mergeData)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.dialogRef.close();
            this.showNotification(
              'snackbar-success',
              'Pathway merged Successfully...!!!',
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
