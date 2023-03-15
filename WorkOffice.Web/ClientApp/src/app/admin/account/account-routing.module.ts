import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { Page404Component } from "./../../authentication/page404/page404.component";
import { AllUsersComponent } from "./all-users/all-users.component";
import { AddUserComponent } from "./add-user/add-user.component";
import { UserProfileComponent } from "./user-profile/user-profile.component";
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
    path: "user-profile",
    component: UserProfileComponent,
  },
  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {}
