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
import { AppTypeModel } from '../apptype.model';
// import { AppTypeModel } from '../apptype.model';
import { AppTypeService } from '../apptype.service';

@Component({
  selector: 'app-add-apptype',
  templateUrl: './add-apptype.component.html',
  styleUrls: ['./add-apptype.component.css'],
})
export class AddAppTypeComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  appTypeForm!: UntypedFormGroup;
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
    private AppTypeService: AppTypeService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.appTypeForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.AppTypeService
      .getAppTypeById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.appTypeForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/apptype/all-apptype']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.appTypeForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new App Type failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const appType: AppTypeModel = {
        // appTypeId: this.id,
        appTypeId: this.id ? + this.id : 0,
        code: this.appTypeForm.value.code,
        name: this.appTypeForm.value.name,

      };
      this.subs.sink = this.AppTypeService
        .addAppType(appType)
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
              this.router.navigate(['/setup/apptype/all-apptype']);
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
