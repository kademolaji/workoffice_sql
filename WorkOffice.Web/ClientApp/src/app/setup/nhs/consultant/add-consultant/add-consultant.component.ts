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
import { ConsultantModel } from '../consultant.model';
// import { ConsultantModel } from '../consultant.model';
import { ConsultantService } from '../consultant.service';

@Component({
  selector: 'app-add-consultant',
  templateUrl: './add-consultant.component.html',
  styleUrls: ['./add-consultant.component.css'],
})
export class AddConsultantComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  consultantForm!: UntypedFormGroup;
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
    private ConsultantService: ConsultantService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.consultantForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;

    if(!this.isAddMode){
      this.subs.sink = this.ConsultantService
      .getConsultantById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.consultantForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/consultant/all-consultant']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.consultantForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new Consultant failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const consultant: ConsultantModel = {
        //consultantId: this.id,
        consultantId: this.id ? + this.id : 0,
        code: this.consultantForm.value.code,
        name: this.consultantForm.value.name,

      };
      this.subs.sink = this.ConsultantService
        .addConsultant(consultant)
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
              this.router.navigate(['/setup/consultant/all-consultant']);
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
