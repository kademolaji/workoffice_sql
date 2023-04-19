import { Component, OnInit } from '@angular/core';
import { WaitinglistService } from '../waitinglist.service';
import { UntypedFormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { WaitinglistModel } from '../watinglist.model';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';

@Component({
  selector: 'app-add-waitinglist',
  templateUrl: './add-waitinglist.component.html',
  styleUrls: ['./add-waitinglist.component.css']
})
export class AddWaitinglistComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
waitinglistForm!: UntypedFormGroup;
submitted = false;
loading = false;
isAddMode = true;
id = 0;

specialityList: GeneralSettingsModel[] = [];
waitingListStatusList: GeneralSettingsModel[] = [];
patientList: GeneralSettingsModel[] = [];

constructor(
  private fb: UntypedFormBuilder,
  private route: ActivatedRoute,
  private router: Router,
  private waitinglistService: WaitinglistService,
  private snackBar: MatSnackBar,
  private generalSettingsService: GeneralSettingsService
) {
  super();
}
ngOnInit() {
  this.waitinglistForm = this.fb.group({
    waitTypeId: ['', [Validators.required]],
    specialityId:   ['', [Validators.required]],
    tciDate :   ['', [Validators.required]],
    waitinglistDate :   ['', [Validators.required]],
    waitinglistTime:   ['', [Validators.required]],
    condition :   ['', [Validators.required]],
    waitinglistStatus:   ['', [Validators.required]],
    pathwayUniqueNumber:   [''],
    patientId:  ['', [Validators.required]],

  });
  this.id = +this.route.snapshot.params['id'];
  this.isAddMode = !this.id;
  if (!this.isAddMode) {
    this.subs.sink = this.waitinglistService.getWaitinglistById(this.id).subscribe({
      next: (res) => {
        if (res.status) {
          this.waitinglistForm.setValue({
            waitTypeId: res.entity.waitTypeId,
            specialityId:   res.entity.specialityId,
            tciDate :   res.entity.tciDate,
            waitinglistDate :   res.entity.waitinglistDate,
            waitinglistTime:   res.entity.waitinglistTime,
            condition :   res.entity.condition,
            waitinglistStatus:   res.entity.waitinglistStatus,
            pathwayUniqueNumber:   res.entity.pathwayUniqueNumber,
          });
        }
      },
    });
  }

  this.subs.sink = this.generalSettingsService
  .getSpecialty()
  .subscribe((response) => {
    this.specialityList = response.entity;
  });
this.subs.sink = this.generalSettingsService
  .getWaitingType()
  .subscribe((response) => {
    this.waitingListStatusList = response.entity;
  });
this.subs.sink = this.generalSettingsService
  .getPatientList()
  .subscribe((response) => {
    this.patientList = response.entity;
  });
}
cancelForm() {
  this.router.navigate(['/nhs/all-waitinglist']);
}

onSubmit() {
  this.submitted = true;
  this.loading = true;
  // stop here if form is invalid
  if (this.waitinglistForm.invalid) {
    this.showNotification(
      'snackbar-danger',
      'Add new waiting list failed...!!!',
      'top',
      'right'
    );
    return;
  } else {
    const watintlist: WaitinglistModel = {
      waitinglistId : this.id ? +this.id : 0,
      waitTypeId: this.waitinglistForm.value.waitTypeId,
      specialityId :this.waitinglistForm.value.specialityId,
      tciDate : this.waitinglistForm.value.tciDate,
      waitinglistDate : this.waitinglistForm.value.waitinglistDate,
      waitinglistTime :this.waitinglistForm.value.waitinglistTime,
      patientId : this.waitinglistForm.value.patientId,
      patientValidationId:0,
      condition : this.waitinglistForm.value.condition,
      waitinglistStatus: this.waitinglistForm.value.waitinglistStatus,
      districtUniqueNumber: "",
      pathwayUniqueNumber: this.waitinglistForm.value.pathwayUniqueNumber,
    };
    this.subs.sink = this.waitinglistService.addWaitinglist(watintlist).subscribe({
      next: (res) => {
        if (res.status) {
          this.loading = false;
          this.showNotification(
            'snackbar-success',
            res.message,
            'top',
            'right'
          );
          this.router.navigate(['/nhs/all-waitinglist']);
        } else {
          this.showNotification(
            'snackbar-danger',
            res.message,
            'top',
            'right'
          );
        }
      },
      error: (error) => {
        this.showNotification('snackbar-danger', error, 'top', 'right');
        this.submitted = false;
        this.loading = false;
      },
    });
  }
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
