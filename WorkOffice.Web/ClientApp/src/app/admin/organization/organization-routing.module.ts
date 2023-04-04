import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { Page404Component } from "./../../authentication/page404/page404.component";
import { AddLocationComponent } from "./location/add-location/add-location.component";
import { AddStructureDefinitionComponent } from "./structure-definition/add-structure-definition/add-structure-definition.component";
import { AllLocationComponent } from "./location/all-location/all-location.component";
import { AllStructureDefinitionComponent } from "./structure-definition/all-structure-definition/all-structure-definition.component";
import { AddCompanyStructureComponent } from "./company-structure/add-company-structure/add-company-structure.component";
import { AllCompanyStructureComponent } from "./company-structure/all-company-structure/all-company-structure.component";
import { AllGeneralInformationComponent } from "./general-information/all-general-information/all-general-information.component";
import { AddGeneralInformationComponent } from "./general-information/add-general-information/add-general-information.component";
import { AddCustomIdentitySettingsComponent } from "./custom-identity-format/add-custom-identity-settings/add-custom-identity-settings.component";
import { AllCustomIdentitySettingsComponent } from "./custom-identity-format/all-custom-identity-settings/all-custom-identity-settings.component";

const routes: Routes = [
  {
    path: "all-general-information",
    component: AllGeneralInformationComponent,
  },
  {
    path: "add-general-information",
    component: AddGeneralInformationComponent,
  },
  {
    path: "edit-general-information/:id",
    component: AddGeneralInformationComponent,
  },
  {
    path: "all-custom-identity-settings",
    component: AllCustomIdentitySettingsComponent,
  },
  {
    path: "add-custom-identity-settings",
    component: AddCustomIdentitySettingsComponent,
  },
  {
    path: "edit-custom-identity-settings/:id",
    component: AddCustomIdentitySettingsComponent,
  },
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
  {
    path: "all-company-structure",
    component: AllCompanyStructureComponent,
  },
  {
    path: "add-company-structure",
    component: AddCompanyStructureComponent,
  },
  {
    path: "edit-company-structure/:id",
    component: AddCompanyStructureComponent,
  },
  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationRoutingModule {}
