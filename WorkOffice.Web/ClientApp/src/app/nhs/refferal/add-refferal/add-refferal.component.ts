import { Component, OnInit } from '@angular/core';
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
import { ReferralService } from '../referral.service';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { catchError, debounceTime, distinctUntilChanged, filter, finalize, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-add-refferal',
  templateUrl: './add-refferal.component.html',
  styleUrls: ['./add-refferal.component.css'],
})
export class AddRefferalComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  referralForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  minLengthTerm=3;
  isLoading = false;

  patientList: GeneralSettingsModel[] = [];
  specialityList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private referralService: ReferralService,
    private snackBar: MatSnackBar,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.referralForm = this.fb.group({
      patientId: ['', [Validators.required]],
      specialtyId: ['', [Validators.required]],
      consultantId: ['', [Validators.required]],
      consultantName: ['', [Validators.required]],
      documentName: ['', [Validators.required]],
      referralDate: [new Date(), [Validators.required]],
      uploadFile: [''],
    });

  this.subs.sink = this.generalSettingsService
    .getConsultant()
    .subscribe((response) => {
      this.consultantList = response.entity;
    });
  this.subs.sink = this.generalSettingsService
    .getSpecialty()
    .subscribe((response) => {
      this.specialityList = response.entity;
    });

    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.referralService.getReferralById(this.id).subscribe({
        next: (res) => {
          if (res.status) {
            this.referralForm.setValue({
              patientId: { label: res.entity.patientName, value: res.entity.patientId},
              specialtyId: res.entity.specialtyId,
              consultantId: res.entity.consultantId,
              documentName: res.entity.documentName,
              consultantName:res.entity.consultantName,
              referralDate: res.entity.referralDate,
              uploadFile: ""
            });
          }
        },
      });
    }
    this.referralForm.get('patientId')?.valueChanges
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
    this.router.navigate(['/nhs/all-referral']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.referralForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new referral failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const formData = new FormData();

      formData.append('referralId', this.id ? this.id.toString() : '0');
      formData.append('patientId', this.referralForm.value.patientId.value);
      formData.append('specialtyId', this.referralForm.value.specialtyId);
      formData.append('consultantId', this.referralForm.value.consultantId);
      formData.append('documentName', this.referralForm.value.documentName);
      formData.append('consultantName', this.referralForm.value.consultantName);
      formData.append('referralDate', new Date(this.referralForm.value.referralDate).toISOString());
      formData.append('file', this.referralForm.value.uploadFile);

      this.subs.sink = this.referralService.addReferral(formData).subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.showNotification(
              'snackbar-success',
              res.message,
              'top',
              'right'
            );
            this.router.navigate(['/nhs/all-referral']);
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
