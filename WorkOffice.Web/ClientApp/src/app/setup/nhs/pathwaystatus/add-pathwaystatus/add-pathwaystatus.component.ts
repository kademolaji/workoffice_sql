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
import { PathwayStatusModel } from '../pathwaystatus.model';
import { PathwayStatusService } from '../pathwaystatus.service';

@Component({
  selector: 'app-add-pathwaystatus',
  templateUrl: './add-pathwaystatus.component.html',
  styleUrls: ['./add-pathwaystatus.component.css'],
})
export class AddPathwayStatusComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  pathwayStatusForm!: UntypedFormGroup;
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
    private PathwayStatusService: PathwayStatusService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.pathwayStatusForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      allowClosed: [false],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.PathwayStatusService
      .getPathwayStatusById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.pathwayStatusForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
              allowClosed: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/pathwaystatus/all-pathwaystatus']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.pathwayStatusForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new App Type failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const pathwayStatus: PathwayStatusModel = {
        //pathwayStatusId: this.id,
        pathwayStatusId: this.id ? + this.id : 0,
        code: this.pathwayStatusForm.value.code,
        name: this.pathwayStatusForm.value.name,
        allowClosed: this.pathwayStatusForm.value.allowClosed
      };
      this.subs.sink = this.PathwayStatusService
        .addPathwayStatus(pathwayStatus)
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
              this.router.navigate(['/setup/pathwaystatus/all-pathwaystatus']);
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
