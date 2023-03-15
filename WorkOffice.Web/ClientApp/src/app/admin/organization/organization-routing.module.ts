import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { Page404Component } from "./../../authentication/page404/page404.component";
import { AddLocationComponent } from "./location/add-location/add-location.component";
import { AddStructureDefinitionComponent } from "./structure-definition/add-structure-definition/add-structure-definition.component";
import { AllLocationComponent } from "./location/all-location/all-location.component";
import { AllStructureDefinitionComponent } from "./structure-definition/all-structure-definition/all-structure-definition.component";

const routes: Routes = [
  {
    path: "all-location",
    component: AllLocationComponent,
  },
  {
    path: "add-location",
    component: AddLocationComponent,
  },
  {
    path: "edit-location/:id",
    component: AddLocationComponent,
  },
  {
    path: "all-structure-definition",
    component: AllStructureDefinitionComponent,
  },
  {
    path: "add-structure-definition",
    component: AddStructureDefinitionComponent,
  },
  {
    path: "edit-structure-definition/:id",
    component: AddStructureDefinitionComponent,
  },
  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationRoutingModule {}
