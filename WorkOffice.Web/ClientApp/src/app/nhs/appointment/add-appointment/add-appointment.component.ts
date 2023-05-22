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
import { catchError, debounceTime, distinctUntilChanged, filter, finalize, switchMap, tap } from 'rxjs';

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

  departmentList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];
  appTypeList: GeneralSettingsModel[] = [];
  hospitalList: GeneralSettingsModel[] = [];
  specialityList: GeneralSettingsModel[] = [];
  pathwayStatusList: GeneralSettingsModel[] = [];
  wardList: GeneralSettingsModel[] = [];

  patientList: GeneralSettingsModel[] = [];
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
      statusId: ['', [Validators.required]],
      specialityId: ['', [Validators.required]],
      bookDate: ['', [Validators.required]],
      appDate: ['', [Validators.required]],
      appTime: ['', [Validators.required]],
      consultantId: ['', [Validators.required]],
      hospitalId: ['', [Validators.required]],
      wardId: ['', [Validators.required]],
      departmentId: [''],
      patientId: ['', [Validators.required]],
      comments: ['', [Validators.required]],
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
                bookDate: res.entity.bookDate,
                appDate: res.entity.appDate,
                appTime: res.entity.appTime,
                consultantId: res.entity.consultantId,
                hospitalId: res.entity.hospitalId,
                wardId: res.entity.wardId,
                departmentId: res.entity.departmentId,
                patientId: { label: "N/A", value: res.entity.patientId},
                comments: res.entity.comments,
              });
            }
          },
        });
    }
    this.appointmentForm.get('patientId')?.valueChanges
    .pipe(
      filter(res => {
        console.log("res", res)
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
    this.router.navigate(['/nhs/all-appointment/PartialBooked']);
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
        patientId: this.appointmentForm.value.patientId.value,
        patientValidationId: +this.appointmentForm.value.patientValidationId,
        comments: this.appointmentForm.value.comments,
        appointmentStatus: '',
        cancellationReason: '',
        speciality: '',
        patientNumber:'',
        patientName:'',
        patientPathNumber: '',
      };
      console.log("patient", patient)
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
              this.router.navigate(['/nhs/all-appointment/PartialBooked']);
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
