import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { Page404Component } from "./../../authentication/page404/page404.component";
import { AllUsersComponent } from "./all-users/all-users.component";
import { AddUserComponent } from "./add-user/add-user.component";
import { UserProfileComponent } from "./user-profile/user-profile.component";
import { AllUserRolesComponent } from "./user-roles/all-user-roles/all-user-roles.component";
import { AddUserRoleComponent } from "./user-roles/add-user-role/add-user-role.component";
import { EditUserComponent } from "./edit-user/edit-user.component";
const routes: Routes = [
  {
    path: "all-users",
    component: AllUsersComponent,
  },
  {
    path: "add-user",
    component: AddUserComponent,
  },
  {
    path: "edit-user/:id",
    component: EditUserComponent,
  },

  {
    path: "user-profile",
    component: UserProfileComponent,
  },
  {
    path: "all-user-roles",
    component: AllUserRolesComponent,
  },
  {
    path: "add-user-role",
    component: AddUserRoleComponent,
  },
  {
    path: "edit-user-role/:id",
    component: AddUserRoleComponent,
  },
  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {}
