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
import { RTTModel } from '../rtt.model';
// import { RTTModel } from '../rtt.model';
import { RTTService } from '../rtt.service';

@Component({
  selector: 'app-add-rtt',
  templateUrl: './add-rtt.component.html',
  styleUrls: ['./add-rtt.component.css'],
})
export class AddRTTComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  rttForm!: UntypedFormGroup;
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
    private RTTService: RTTService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.rttForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.RTTService
      .getRTTById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.rttForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/rtt/all-rtt']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.rttForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new RTT failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const rtt: RTTModel = {
        //rttId: this.id,
        rttId: this.id ? + this.id : 0,
        code: this.rttForm.value.code,
        name: this.rttForm.value.name,

      };
      this.subs.sink = this.RTTService
        .addRTT(rtt)
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
              this.router.navigate(['/setup/rtt/all-rtt']);
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
