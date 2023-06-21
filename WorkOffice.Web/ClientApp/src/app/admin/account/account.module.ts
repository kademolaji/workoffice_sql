import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSortModule } from '@angular/material/sort';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDatepickerModule } from '@angular/material/datepicker';

import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';
// import { MatTableExporterModule } from "mat-table-exporter";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { MatTabsModule } from '@angular/material/tabs';
import { ComponentsModule } from 'src/app/shared/components/components.module';
import { SharedModule } from './../../shared/shared.module';
import { AddUserComponent } from './add-user/add-user.component';
import { AllUsersComponent } from './all-users/all-users.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { DeleteDialogComponent } from './all-users/dialog/delete/delete.component';
import { AccountRoutingModule } from './account-routing.module';
import { UsersService } from './all-users/users.service';
import { UserRoleService } from './user-roles/user-role.service';
import { AddUserRoleComponent } from './user-roles/add-user-role/add-user-role.component';
import { AllUserRolesComponent } from './user-roles/all-user-roles/all-user-roles.component';
import { DeleteUserRoleDialogComponent } from './user-roles/all-user-roles/dialog/delete/delete.component';
import { EditUserComponent } from './edit-user/edit-user.component';

@NgModule({
  declarations: [
    AllUsersComponent,
    DeleteDialogComponent,
    AddUserComponent,
    EditUserComponent,

    AddUserRoleComponent,
    AllUserRolesComponent,
    DeleteUserRoleDialogComponent,

    UserProfileComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSortModule,
    MatToolbarModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatTabsModule,
    MatTooltipModule,
    // MatTableExporterModule,
    MatProgressSpinnerModule,
    AccountRoutingModule,
    ComponentsModule,
    SharedModule,
  ],
  providers: [UsersService, UserRoleService],
})
export class AccountModule {}
