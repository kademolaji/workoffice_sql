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
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
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
  countryList: GeneralSettingsModel[] = [];
  userRoleList: GeneralSettingsModel[] = [];

  constructor(
    private fb: UntypedFormBuilder,
    private router: Router,
    private usersService: UsersService,
    private snackBar: MatSnackBar,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.subs.sink = this.generalSettingsService
      .getCountryList()
      .subscribe((response) => {
        this.countryList = response.entity;
      });
    this.subs.sink = this.generalSettingsService
      .getUserRoleList()
      .subscribe((response) => {
        this.userRoleList = response.entity;
      });
    this.addUserForm = this.fb.group(
      {
        firstName: ['', [Validators.required, Validators.pattern('[a-zA-Z]+')]],
        lastName: ['', [Validators.required, Validators.pattern('[a-zA-Z]+')]],
        email: [
          '',
          [Validators.required, Validators.email, Validators.minLength(5)],
        ],
        country: ['', [Validators.required]],
        userRoleId: ['', [Validators.required]],
        phoneNumber: ['', [Validators.required]],
        securityQuestion: ['', [Validators.required]],
        securityAnswer: ['', [Validators.required]],
        lastLogin: [''],
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
      const userRoles: number[] = [];
      userRoles.push(+this.addUserForm.value.userRoleId);
      const user: AddEditUserModel = {
        firstName: this.addUserForm.value.firstName,
        lastName: this.addUserForm.value.lastName,
        email: this.addUserForm.value.email,
        country: this.addUserForm.value.country,
        lastLogin: this.addUserForm.value.lastLogin,
        userRoleId: +this.addUserForm.value.userRoleId,
        acceptTerms: true,
        phoneNumber: this.addUserForm.value.phoneNumber,
        accesslevel: 1,
        securityQuestion: this.addUserForm.value.securityQuestion,
        securityAnswer: this.addUserForm.value.securityAnswer,
        userAccessIds: [],
        userRoleIds: userRoles,
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
