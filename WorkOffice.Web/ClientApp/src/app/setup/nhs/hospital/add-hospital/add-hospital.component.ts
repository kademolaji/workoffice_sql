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
import { HospitalModel } from '../hospital.model';
import { HospitalService } from '../hospital.service';

@Component({
  selector: 'app-add-hospital',
  templateUrl: './add-hospital.component.html',
  styleUrls: ['./add-hospital.component.css'],
})
export class AddHospitalComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  hospitalForm!: UntypedFormGroup;
  hide3 = true;
  agree3 = false;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private HospitalService: HospitalService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.hospitalForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.HospitalService
      .getHospitalById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.hospitalForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/hospital/all-hospital']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.hospitalForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new Hospital failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const hospital: HospitalModel = {
        // hospitalId: this.id,
        hospitalId: this.id ? + this.id : 0,
        code: this.hospitalForm.value.code,
        name: this.hospitalForm.value.name,

      };
      this.subs.sink = this.HospitalService
        .addHospital(hospital)
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
              this.router.navigate(['/setup/hospital/all-hospital']);
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
