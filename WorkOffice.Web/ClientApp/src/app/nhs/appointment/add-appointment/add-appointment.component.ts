import { Component, OnInit } from '@angular/core';
import {
  FormControl,
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
import { AppointmentService } from '../appointment.service';
import { CreateAppointmentModel } from '../appointment.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import {
  catchError,
  debounceTime,
  distinctUntilChanged,
  filter,
  finalize,
  switchMap,
  tap,
} from 'rxjs';

@Component({
  selector: 'app-add-appointment',
  templateUrl: './add-appointment.component.html',
  styleUrls: ['./add-appointment.component.css'],
})
export class AddAppointmentComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  appointmentForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  disableDepartment = true;
  departmentList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];
  appTypeList: GeneralSettingsModel[] = [];
  hospitalList: GeneralSettingsModel[] = [];
  specialityList: GeneralSettingsModel[] = [];
  pathwayStatusList: GeneralSettingsModel[] = [];
  patientPathWayList: GeneralSettingsModel[] = [];
  wardList: GeneralSettingsModel[] = [];

  patientList: GeneralSettingsModel[] = [];
  pathwayList: GeneralSettingsModel[] = [];
  isLoading = false;
  minLengthTerm = 3;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private appointmentService: AppointmentService,
    private snackBar: MatSnackBar,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.appointmentForm = this.fb.group({
      appTypeId: ['', [Validators.required]],
      specialityId: ['', [Validators.required]],
      patientId: ['', [Validators.required]],
      bookDate: [''],
      appDate: [''],
      appTime: [''],
      consultantId: [''],
      hospitalId: [''],
      wardId: [''],
      comments: [''],
      statusId: [''],
      departmentId: [1],
      patientValidationId: [''],
    });

    this.subs.sink = this.generalSettingsService
      .getSpecialty()
      .subscribe((response) => {
        this.specialityList = response.entity;
      });
    this.subs.sink = this.generalSettingsService
      .getConsultant()
      .subscribe((response) => {
        this.consultantList = response.entity;
      });
    this.subs.sink = this.generalSettingsService
      .getAppType()
      .subscribe((response) => {
        this.appTypeList = response.entity;
      });

    this.subs.sink = this.generalSettingsService
      .getHospital()
      .subscribe((response) => {
        this.hospitalList = response.entity;
      });

    this.subs.sink = this.generalSettingsService
      .getPathwayStatus()
      .subscribe((response) => {
        this.pathwayStatusList = response.entity;
      });
    this.subs.sink = this.generalSettingsService
      .getDepartmentList()
      .subscribe((response) => {
        this.departmentList = response.entity;
      });
    this.subs.sink = this.generalSettingsService
      .getWard()
      .subscribe((response) => {
        this.wardList = response.entity;
      });

    this.subs.sink = this.generalSettingsService
      .getPatientPathWayList()
      .subscribe((response) => {
        this.patientPathWayList = response.entity;
      });

    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.appointmentService
        .getAppointmentById(this.id)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.appointmentForm.setValue({
                appTypeId: res.entity.appTypeId,
                statusId: res.entity.statusId,
                specialityId: res.entity.specialityId,
                bookDate: res.entity.bookDate
                  ? new Date(res.entity.bookDate)
                  : '',
                appDate: res.entity.appDate ? new Date(res.entity.appDate) : '',
                appTime: res.entity.appTime,
                consultantId: res.entity.consultantId,
                hospitalId: res.entity.hospitalId,
                wardId: res.entity.wardId,
                departmentId: res.entity.departmentId,
                patientId: {
                  label: res.entity.patientName,
                  value: res.entity.patientId,
                },
                patientValidationId: {
                  label: res.entity.patientPathNumber,
                  value: res.entity.patientValidationId,
                },
                comments: res.entity.comments,
              });
            }
          },
        });
    }
    this.appointmentForm
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

    this.appointmentForm
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

    this.appointmentForm.get('appTypeId')?.valueChanges.subscribe((value) => {
      if (value !== 3) {
        this.appointmentForm
          .get('consultantId')
          ?.setValidators(Validators.required);
        this.appointmentForm
          .get('hospitalId')
          ?.setValidators(Validators.required);
        this.appointmentForm.get('wardId')?.setValidators(Validators.required);
        this.appointmentForm
          .get('comments')
          ?.setValidators(Validators.required);
        this.appointmentForm
          .get('bookDate')
          ?.setValidators(Validators.required);
        this.appointmentForm.get('appDate')?.setValidators(Validators.required);
        this.appointmentForm.get('appTime')?.setValidators(Validators.required);
        this.appointmentForm
          .get('statusId')
          ?.setValidators(Validators.required);
        this.appointmentForm
          .get('departmentId')
          ?.setValidators(Validators.required);
      } else {
        this.appointmentForm.get('consultantId')?.clearValidators();
        this.appointmentForm.get('hospitalId')?.clearValidators();
        this.appointmentForm.get('wardId')?.clearValidators();
        this.appointmentForm.get('comments')?.clearValidators();
        this.appointmentForm.get('bookDate')?.clearValidators();
        this.appointmentForm.get('appDate')?.clearValidators();
        this.appointmentForm.get('appTime')?.clearValidators();
        this.appointmentForm.get('statusId')?.clearValidators();
        this.appointmentForm.get('departmentId')?.clearValidators();
      }
      this.appointmentForm.get('consultantId')?.updateValueAndValidity();
      this.appointmentForm.get('hospitalId')?.updateValueAndValidity();
      this.appointmentForm.get('wardId')?.updateValueAndValidity();
      this.appointmentForm.get('comments')?.updateValueAndValidity();
      this.appointmentForm.get('bookDate')?.updateValueAndValidity();
      this.appointmentForm.get('appDate')?.updateValueAndValidity();
      this.appointmentForm.get('appTime')?.updateValueAndValidity();
      this.appointmentForm.get('statusId')?.updateValueAndValidity();
      this.appointmentForm.get('departmentId')?.updateValueAndValidity();
    });
  }

  displayWithPatient(value: any) {
    return value?.label;
  }

  displayWithPathway(value: any) {
    return value?.label;
  }

  cancelForm() {
    this.router.navigate(['/nhs/all-appointment']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.appointmentForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new appointment failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const patient: CreateAppointmentModel = {
        appointmentId: this.id ? +this.id : 0,
        appTypeId: +this.appointmentForm.value.appTypeId,
        statusId: +this.appointmentForm.value.statusId,
        specialityId: +this.appointmentForm.value.specialityId,
        bookDate: this.appointmentForm.value.bookDate,
        appDate: this.appointmentForm.value.appDate,
        appTime: this.appointmentForm.value.appTime,
        consultantId: +this.appointmentForm.value.consultantId,
        hospitalId: +this.appointmentForm.value.hospitalId,
        wardId: +this.appointmentForm.value.wardId,
        departmentId: +this.appointmentForm.value.departmentId,
        patientId: +this.appointmentForm.value.patientId.value,
        patientValidationId:
          +this.appointmentForm.value.patientValidationId.value,
        comments: this.appointmentForm.value.comments,
        appointmentStatus: '',
        cancellationReason: '',
        speciality: '',
        patientNumber: '',
        patientName: '',
        patientPathWayNumber: '',
      };
      this.subs.sink = this.appointmentService
        .addAppointment(patient)
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
              this.router.navigate(['/nhs/all-appointment']);
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
