import { Component, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { PatientModel } from '../patient.model';
import { PatientService } from '../patient.service';

@Component({
  selector: 'app-add-patient',
  templateUrl: './add-patient.component.html',
  styleUrls: ['./add-patient.component.css']
})
export class AddPatientComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  patientInformationForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private patientService: PatientService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.patientInformationForm = this.fb.group({
      districtNumber: ['', [Validators.required]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      middleName: ['', [Validators.required]],
      dOB: ['', [Validators.required]],
      age: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phoneNo: ['', [Validators.required]],
      email: ['', [Validators.required]],
      sex: ['', [Validators.required]],
      postalCode: ['', [Validators.required]],
      nHSNumber: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.patientService.getPatientById(this.id).subscribe({
        next: (res) => {
          if (res.status) {
            this.patientInformationForm.setValue({
              districtNumber: res.entity.districtNumber,
              firstName: res.entity.firstName,
              lastName: res.entity.lastName,
              middleName: res.entity.middleName,
              dOB: res.entity.dOB,
              age: res.entity.age,
              address: res.entity.address,
              phoneNo: res.entity.phoneNo,
              email: res.entity.email,
              sex: res.entity.sex,
              postalCode: res.entity.postalCode,
              nHSNumber: res.entity.nHSNumber,
            });
          }
        },
      });
    }
  }
  cancelForm() {
    this.router.navigate(['/nhs/all-patient']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.patientInformationForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new patient information failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const patient: PatientModel = {
        patientId: this.id ? this.id : 0,
        districtNumber: this.patientInformationForm.value.definition,
        firstName: this.patientInformationForm.value.definition,
        lastName: this.patientInformationForm.value.definition,
        middleName: this.patientInformationForm.value.definition,
        dOB: this.patientInformationForm.value.definition,
        age: this.patientInformationForm.value.definition,
        address: this.patientInformationForm.value.definition,
        phoneNo: this.patientInformationForm.value.definition,
        email: this.patientInformationForm.value.definition,
        sex: this.patientInformationForm.value.definition,
        postalCode: this.patientInformationForm.value.definition,
        nHSNumber: this.patientInformationForm.value.definition,
        active: true,
        fullName: 'null',
      };
      this.subs.sink = this.patientService.addPatient(patient).subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.showNotification(
              'snackbar-success',
              res.message,
              'top',
              'right'
            );
            this.router.navigate(['/nhs/all-patient']);
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
