import { Component, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { PathwayService } from '../pathway.service';
import { CreatePathwayModel } from '../pathway.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { catchError, debounceTime, distinctUntilChanged, filter, finalize, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-add-pathway',
  templateUrl: './add-pathway.component.html',
  styleUrls: ['./add-pathway.component.css'],
})
export class AddPathwayComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  pathwayForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;

  departmentList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];
  appTypeList: GeneralSettingsModel[] = [];
  hospitalList: GeneralSettingsModel[] = [];
  specialityList: GeneralSettingsModel[] = [];
  pathwayStatusList: GeneralSettingsModel[] = [];
  patientList: GeneralSettingsModel[] = [];
  rttList: GeneralSettingsModel[] = [];
  minLengthTerm=3;
  isLoading = false;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private pathwayService: PathwayService,
    private snackBar: MatSnackBar,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.pathwayForm = this.fb.group({
      pathWayNumber: [''],
      pathWayCondition: ['', [Validators.required]],
      specialtyId: [0, [Validators.required]],
      pathWayStartDate: [new Date(), [Validators.required]],
      pathWayEndDate: [new Date()],
      nhsNumber: [''],
      pathWayStatusId: ['', [Validators.required]],
      rttId: [],
      patientId: ['', [Validators.required]],
      rttWait: [''],
      specialityName:[''],

    });

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
      .getRTT()
      .subscribe((response) => {
        this.rttList = response.entity;
      });

    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.pathwayService
        .getPathwayById(this.id)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.pathwayForm.setValue({
                pathWayNumber: res.entity.pathWayNumber,
                pathWayCondition: res.entity.pathWayCondition,
                specialtyId: res.entity.specialtyId,
                pathWayStartDate: res.entity.pathWayStartDate,
                pathWayEndDate: res.entity.pathWayEndDate,
                nhsNumber: res.entity.nhsNumber,
                pathWayStatusId: res.entity.pathWayStatusId,
                rttId: res.entity.rttId,
                patientId: { label: res.entity.patientName, value: res.entity.patientId},
                rttWait: res.entity.rttWait,
                specialityName: res.entity.specialityName,
              });
            }
          },
        });
    }

    this.pathwayForm.get('patientId')?.valueChanges
    .pipe(
      filter(res => {
        return res !== null && res.length >= this.minLengthTerm
      }),
      distinctUntilChanged(),
      debounceTime(1000),
      tap(() => {
        this.patientList = [];
        this.isLoading = true;
      }),
      switchMap(value => this.generalSettingsService.getPatientList(value as string).pipe(catchError((err) => this.router.navigateByUrl('/')))
        .pipe(
          finalize(() => {
            this.isLoading = false
          }),
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
  }

  displayWith(value: any) {
    return value?.label;
  }

  cancelForm() {
    this.router.navigate(['/nhs/all-pathway']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.pathwayForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new pathway failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const patient: CreatePathwayModel = {
        patientValidationId: this.id ? +this.id : 0,
        pathWayNumber: this.pathwayForm.value.pathWayNumber,
        pathWayCondition: this.pathwayForm.value.pathWayCondition,
        specialtyId: +this.pathwayForm.value.specialtyId,
        rttId: +this.pathwayForm.value.rttId,
        pathWayStartDate: this.pathwayForm.value.pathWayStartDate,
        pathWayEndDate: this.pathwayForm.value.pathWayEndDate,
        pathWayStatusId: +this.pathwayForm.value.pathWayStatusId,
        patientId: +this.pathwayForm.value.patientId.value,
        rttWait: this.pathwayForm.value.rttWait,
        districtNumber: '',
        nhsNumber: '',
        patientNumber: '',
        specialityName: ''
      };
      this.subs.sink = this.pathwayService
        .addPathway(patient)
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
              this.router.navigate(['/nhs/all-pathway']);
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
