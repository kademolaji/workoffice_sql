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
import { NHSActivityModel } from '../nhsactivity.model';
import { NHSActivityService } from '../nhsactivity.service';

@Component({
  selector: 'app-add-nhsactivity',
  templateUrl: './add-nhsactivity.component.html',
  styleUrls: ['./add-nhsactivity.component.css'],
})
export class AddNHSActivityComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  nhsActivityForm!: UntypedFormGroup;
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
    private NHSActivityService: NHSActivityService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.nhsActivityForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.NHSActivityService
      .getNHSActivityById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.nhsActivityForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/nhsactivity/all-nhsactivity']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.nhsActivityForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new App Type failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const nhsActivity: NHSActivityModel = {
        //nhsActivityId: this.id,
        nhsActivityId: this.id ? + this.id : 0,
        code: this.nhsActivityForm.value.code,
        name: this.nhsActivityForm.value.name,

      };
      this.subs.sink = this.NHSActivityService
        .addNHSActivity(nhsActivity)
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
              this.router.navigate(['/setup/nhsactivity/all-nhsactivity']);
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
