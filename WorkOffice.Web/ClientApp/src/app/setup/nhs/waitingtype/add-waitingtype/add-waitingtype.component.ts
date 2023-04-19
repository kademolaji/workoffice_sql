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
import { WaitingTypeModel } from '../waitingtype.model';
// import { WaitingTypeModel } from '../waitingtype.model';
import { WaitingTypeService } from '../waitingtype.service';

@Component({
  selector: 'app-add-waitingtype',
  templateUrl: './add-waitingtype.component.html',
  styleUrls: ['./add-waitingtype.component.css'],
})
export class AddWaitingTypeComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  waitingTypeForm!: UntypedFormGroup;
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
    private WaitingTypeService: WaitingTypeService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.waitingTypeForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.WaitingTypeService
      .getWaitingTypeById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.waitingTypeForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/waitingtype/all-waitingtype']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.waitingTypeForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new Waiting Type failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const waitingType: WaitingTypeModel = {
        waitingTypeId: this.id,
        code: this.waitingTypeForm.value.code,
        name: this.waitingTypeForm.value.name,

      };
      this.subs.sink = this.WaitingTypeService
        .addWaitingType(waitingType)
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
              this.router.navigate(['/setup/waitingtype/all-waitingtype']);
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
