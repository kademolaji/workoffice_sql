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
import { Router, ActivatedRoute } from '@angular/router';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { UsersService } from '../all-users/users.service';
import { UpdateUserModel } from '../all-users/users.model';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css'],
})
export class EditUserComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  editUserForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  countryList: GeneralSettingsModel[] = [];
  userRoleList: GeneralSettingsModel[] = [];
  id = 0;

  constructor(
    private fb: UntypedFormBuilder,
    private router: Router,
    private route: ActivatedRoute,
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
    this.editUserForm = this.fb.group({
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
    });
    this.id = +this.route.snapshot.params['id'];
    this.subs.sink = this.usersService.getUserById(this.id).subscribe({
      next: (res) => {
        if (res.status) {
          this.editUserForm.setValue({
            firstName: res.entity.firstName,
            lastName: res.entity.lastName,
            email: res.entity.email,
            country: res.entity.country,
            userRoleId: res.entity.userRoleId,
            phoneNumber: res.entity.phoneNumber || '',
            securityQuestion: res.entity.securityQuestion || '',
            securityAnswer: res.entity.securityAnswer || '',
            lastLogin:  res.entity.lastLogin ? new Date(res.entity.lastLogin) : '',
          });
        }
      },
    });
  }

  cancelForm() {
    this.router.navigate(['/admin/users/all-users']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.editUserForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new user failed...!!!',
        'bottom',
        'center'
      );
      return;
    } else {
      const userRoles: number[] = [];
      userRoles.push(+this.editUserForm.value.userRoleId);
      const user: UpdateUserModel = {
        userId: this.id,
        firstName: this.editUserForm.value.firstName,
        lastName: this.editUserForm.value.lastName,
        email: this.editUserForm.value.email,
        country: this.editUserForm.value.country,
        lastLogin: this.editUserForm.value.lastLogin,
        phoneNumber: this.editUserForm.value.phoneNumber,
        securityQuestion: this.editUserForm.value.securityQuestion,
        securityAnswer: this.editUserForm.value.securityAnswer,
        userRoleIds: userRoles,
      };
      this.subs.sink = this.usersService.updateUser(user).subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.showNotification(
              'snackbar-success',
              'User updated successfully...!!!',
              'top',
              'right'
            );
            this.router.navigate(['/admin/users/all-users']);
          } else {
            this.showNotification(
              'snackbar-danger',
              'Edit user failed...!!!',
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
