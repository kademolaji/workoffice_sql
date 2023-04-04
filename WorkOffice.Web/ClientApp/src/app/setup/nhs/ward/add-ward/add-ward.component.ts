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
import { WardModel } from '../ward.model';
// import { WardModel } from '../ward.model';
import { WardService } from '../ward.service';

@Component({
  selector: 'app-add-ward',
  templateUrl: './add-ward.component.html',
  styleUrls: ['./add-ward.component.css'],
})
export class AddWardComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  wardForm!: UntypedFormGroup;
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
    private WardService: WardService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.wardForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.WardService
      .getWardById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.wardForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/ward/all-ward']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.wardForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new Ward failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const ward: WardModel = {
        wardId: this.id,
        code: this.wardForm.value.code,
        name: this.wardForm.value.name,

      };
      this.subs.sink = this.WardService
        .addWard(ward)
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
              this.router.navigate(['/setup/ward/all-ward']);
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
