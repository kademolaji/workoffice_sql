import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { Page404Component } from "./../../authentication/page404/page404.component";
import { AddAppTypeComponent } from "./apptype/add-apptype.component";
import { AllAppTypeComponent } from "./apptype/all-apptype/all-apptype.component";


const routes: Routes = [
  {
    path: "all-apptype",
    component: AllAppTypeComponent,
  },
  {
    path: "add-apptype",
    component: AddAppTypeComponent,
  },
  {
    path: "edit-apptype/:id",
    component: AddAppTypeComponent,
  },
  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NHSRoutingModule {}
