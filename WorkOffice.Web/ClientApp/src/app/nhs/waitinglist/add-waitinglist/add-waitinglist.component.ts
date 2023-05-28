import { Component, OnInit } from '@angular/core';
import { WaitinglistService } from '../waitinglist.service';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
} from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { WaitinglistModel } from '../watinglist.model';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { catchError, debounceTime, distinctUntilChanged, filter, finalize, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-add-waitinglist',
  templateUrl: './add-waitinglist.component.html',
  styleUrls: ['./add-waitinglist.component.css'],
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
  waitTypeList: GeneralSettingsModel[] = [];
  patientList: GeneralSettingsModel[] = [];
  pathwayList: GeneralSettingsModel[] = [];
  isLoading = false;
  minLengthTerm = 3;

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
      specialityId: ['', [Validators.required]],
      tciDate: [new Date(), [Validators.required]],
      waitinglistDate: [new Date(), [Validators.required]],
      waitinglistTime: ['', [Validators.required]],
      condition: ['', [Validators.required]],
      waitinglistStatus: ['', [Validators.required]],
      patientValidationId: [''],
      patientId: ['', [Validators.required]],
    });

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
    this.subs.sink = this.generalSettingsService
      .getWaitingType()
      .subscribe((response) => {
        this.waitTypeList = response.entity;
      });

    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.waitinglistService
        .getWaitinglistById(this.id)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.waitinglistForm.setValue({
                waitTypeId: res.entity.waitTypeId,
                specialityId: res.entity.specialityId,
                tciDate: res.entity.tciDate,
                waitinglistDate: res.entity.waitinglistDate,
                waitinglistTime: res.entity.waitinglistTime,
                condition: res.entity.condition,
                waitinglistStatus: res.entity.waitinglistStatus,
                patientId: { label: res.entity.districtNumber, value: res.entity.patientId},
                patientValidationId: { label: res.entity.pathWayNumber, value: res.entity.patientValidationId},
              });
            }
          },
        });
    }

    this.waitinglistForm
      .get('patientId')
      ?.valueChanges.pipe(
        filter((res) => {
          return res !== null && res.length >= this.minLengthTerm;
        }),
        distinctUntilChanged(),
        debounceTime(1000),
        tap(() => {
          this.patientList = [];
          this.isLoading = true;
        }),
        switchMap((value) =>
          this.generalSettingsService
            .getPatientList(value as string)
            .pipe(catchError((err) => this.router.navigateByUrl('/')))
            .pipe(
              finalize(() => {
                this.isLoading = false;
              })
            )
        )
      )
      .subscribe((data: any) => {
        if (data['entity'] == undefined) {
          this.patientList = [];
        } else {
          this.patientList = data['entity'];
        }
      });

    this.waitinglistForm
      .get('patientValidationId')
      ?.valueChanges.pipe(
        filter((res) => {
          return res !== null && res.length >= this.minLengthTerm;
        }),
        distinctUntilChanged(),
        debounceTime(1000),
        tap(() => {
          this.pathwayList = [];
          this.isLoading = true;
        }),
        switchMap((value) =>
          this.generalSettingsService
            .getPatientPathWayList(value as string)
            .pipe(catchError((err) => this.router.navigateByUrl('/')))
            .pipe(
              finalize(() => {
                this.isLoading = false;
              })
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

  displayWithPatient(value: any) {
    return value?.label;
  }

  displayWithPathway(value: any) {
    return value?.label;
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
        waitinglistId: this.id ? +this.id : 0,
        waitTypeId: +this.waitinglistForm.value.waitTypeId,
        specialityId: +this.waitinglistForm.value.specialityId,
        tciDate: this.waitinglistForm.value.tciDate,
        waitinglistDate: this.waitinglistForm.value.waitinglistDate,
        waitinglistTime: this.waitinglistForm.value.waitinglistTime,
        patientId: +this.waitinglistForm.value.patientId.value,
        patientValidationId:
          +this.waitinglistForm.value.patientValidationId.value,
        condition: this.waitinglistForm.value.condition,
        waitinglistStatus: this.waitinglistForm.value.waitinglistStatus,
        districtNumber: '',
        pathWayNumber: '',
      };
      this.subs.sink = this.waitinglistService
        .addWaitinglist(watintlist)
        .subscribe({
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
