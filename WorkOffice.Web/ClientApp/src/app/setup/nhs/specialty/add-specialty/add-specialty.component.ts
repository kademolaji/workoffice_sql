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
import { SpecialtyModel } from '../specialty.model';
// import { SpecialtyModel } from '../specialty.model';
import { SpecialtyService } from '../specialty.service';

@Component({
  selector: 'app-add-specialty',
  templateUrl: './add-specialty.component.html',
  styleUrls: ['./add-specialty.component.css'],
})
export class AddSpecialtyComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  specialtyForm!: UntypedFormGroup;
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
    private SpecialtyService: SpecialtyService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.specialtyForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
    });
    this.id = this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.SpecialtyService
      .getSpecialtyById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.specialtyForm.setValue({
              code: res.entity.code,
              name: res.entity.name,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/setup/specialty/all-specialty']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.specialtyForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new Specialty failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const specialty: SpecialtyModel = {
        specialtyId: this.id,
        code: this.specialtyForm.value.code,
        name: this.specialtyForm.value.name,

      };
      this.subs.sink = this.SpecialtyService
        .addSpecialty(specialty)
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
              this.router.navigate(['/setup/specialty/all-specialty']);
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
