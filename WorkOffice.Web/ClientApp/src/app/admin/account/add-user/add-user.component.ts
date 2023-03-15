import { Component, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { MustMatch } from 'src/app/core/utilities/must-match.validator';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { AddEditUserModel } from '../all-users/users.model';
import { UsersService } from '../all-users/users.service';
@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.sass'],
})
export class AddUserComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  addUserForm!: UntypedFormGroup;
  hide3 = true;
  agree3 = false;
  submitted = false;
  loading = false;
  constructor(
    private fb: UntypedFormBuilder,
    private router: Router,
    private usersService: UsersService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.addUserForm = this.fb.group(
      {
        firstName: ['', [Validators.required, Validators.pattern('[a-zA-Z]+')]],
        lastName: ['', [Validators.required, Validators.pattern('[a-zA-Z]+')]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
        email: [
          '',
          [Validators.required, Validators.email, Validators.minLength(5)],
        ],
        country: ['', [Validators.required]],
        userRole: [''],
      },
      {
        validator: MustMatch('password', 'confirmPassword'),
      }
    );
  }
  cancelForm() {
    this.router.navigate(['/admin/users/all-users']);
  }


  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.addUserForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new user failed...!!!',
        'bottom',
        'center'
      );
      return;
    } else {
      console.log("form Value", this.addUserForm.value)
      const user: AddEditUserModel = {
        firstName: this.addUserForm.value.firstName,
        lastName: this.addUserForm.value.lastName,
        password: this.addUserForm.value.password,
        confirmPassword: this.addUserForm.value.confirmPassword,
        email: this.addUserForm.value.email,
        country: this.addUserForm.value.country,
        userRole: this.addUserForm.value.userRole,
        acceptTerms: true,
      };
      this.subs.sink = this.usersService.addUser(user).subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.showNotification(
              'snackbar-success',
              'User added successfully...!!!',
              'top',
              'right'
            );
            this.router.navigate(['/admin/users/all-users']);
          } else {
            this.showNotification(
              'snackbar-danger',
              'Add new user failed...!!!',
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
