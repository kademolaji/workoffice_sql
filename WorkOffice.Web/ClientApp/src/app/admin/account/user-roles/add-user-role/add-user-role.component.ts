import { SelectionModel } from '@angular/cdk/collections';
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
import {
  UserRoleActivitiesModel,
  UserRoleAndActivityModel,
} from '../user-role.model';
import { UserRoleService } from '../user-role.service';

@Component({
  selector: 'app-add-user-role',
  templateUrl: './add-user-role.component.html',
  styleUrls: ['./add-user-role.component.css'],
})
export class AddUserRoleComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  userRoleForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;

  displayedColumns: string[] = [
    'select',
    'userActivityParentName',
    'userActivityName',
    // 'canAdd',
    // 'canEdit',
    // 'canView',
    // 'canDelete',
    // 'canApprove',
  ];
 activitiesList: UserRoleActivitiesModel[] = [];
  isTblLoading = false;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userRoleService: UserRoleService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.userRoleForm = this.fb.group({
      userRoleDefinition: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    this.isTblLoading = true;
    const userRoleId = this.id ? this.id : 0;
    this.subs.sink = this.userRoleService
      .getUserRoleAndActivities(userRoleId)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.activitiesList = res.entity;
            this.isTblLoading = false;
          }
        },
      });

    if (!this.isAddMode) {
      this.subs.sink = this.userRoleService
        .getUserRoleById(userRoleId)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.userRoleForm.setValue({
                userRoleDefinition: res.entity.userRoleDefinition,
              });
            }
          },
        });
    }
  }
  cancelForm() {
    this.router.navigate(['/admin/users/all-user-roles']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.userRoleForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new user role failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const userRole: UserRoleAndActivityModel = {
        userRoleAndActivityId: this.id ? this.id : 0,
        userRoleDefinition: this.userRoleForm.value.definition,
        activities: [],
      };
      this.subs.sink = this.userRoleService.addUserRole(userRole).subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.showNotification(
              'snackbar-success',
              res.message,
              'top',
              'right'
            );
            this.router.navigate(['/admin/users/all-user-roles']);
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

  checkUncheckAll(evt: Event) {
    this.activitiesList.forEach((c) => c.isSelected = (evt.target as HTMLInputElement).checked)
  }


}
