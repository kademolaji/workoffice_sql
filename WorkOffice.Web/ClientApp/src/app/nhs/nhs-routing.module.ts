import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { Page404Component } from "../authentication/page404/page404.component";
import { AddAppointmentComponent } from "./appointment/add-appointment/add-appointment.component";
import { AllAppointmentComponent } from "./appointment/all-appointment/all-appointment.component";
import { AddPatientComponent } from "./patient/add-patient/add-patient.component";
import { AllPatientComponent } from "./patient/all-patient/all-patient.component";
import { AddWaitinglistComponent } from "./waitinglist/add-waitinglist/add-waitinglist.component";
import { AllWaitinglistComponent } from "./waitinglist/all-waitinglist/all-waitinglist.component";
import { ValidateNowComponent } from "./Validate/validate-now/validate-now.component";
import { AddPathwayComponent } from "./pathway/add-pathway/add-pathway.component";
import { AddRefferalComponent } from "./refferal/add-refferal/add-refferal.component";
import { AllRefferalComponent } from "./refferal/all-refferal/all-refferal.component";
import { AllDiagnosticComponent } from "./diagnostic/all-diagnostic/all-diagnostic.component";
import { AddDiagnosticComponent } from "./diagnostic/add-diagnostic/add-diagnostic.component";
import { AllPathwayComponent } from "./pathway/all-pathway/all-pathway.component";
import { ValidateNowPathwaysComponent } from "./Validate/validate-now-pathways/validate-now-pathways.component";
import { ValidatePatientComponent } from "./Validate/validate-patient/validate-patient.component";


const routes: Routes = [
  {
    path: "all-appointment/:status",
    component: AllAppointmentComponent,
  },

  // {
  //   path: "all-cancelappointment",
  //   component: AllCancelAppointmentComponent,
  // },
  {
    path: "add-appointment",
    component: AddAppointmentComponent,
  },
  {
    path: "edit-appointment/:id",
    component: AddAppointmentComponent,
  },
  {
    path: "all-patient",
    component: AllPatientComponent,
  },
  {
    path: "add-patient",
    component: AddPatientComponent,
  },
  {
    path: "edit-patient/:id",
    component: AddPatientComponent,
  },
  {
    path: "all-waitinglist",
    component: AllWaitinglistComponent,
  },
  {
    path: "add-waitinglist",
    component: AddWaitinglistComponent,
  },
  {
    path: "edit-waitinglist/:id",
    component: AddWaitinglistComponent,
  },
  {
    path: "all-pathway",
    component: AllPathwayComponent,
  },
  {
    path: "add-pathway",
    component: AddPathwayComponent,
  },
  {
    path: "edit-pathway/:id",
    component: AddPathwayComponent,
  },
  {
    path: "validate-now",
    component: ValidateNowComponent,
  },
  {
    path: "validate-now/:id",
    component: ValidateNowPathwaysComponent,
  },
  {
    path: ":patientId/validate-now/:id",
    component: ValidatePatientComponent,
  },


  {
    path: "all-diagnostic",
    component: AllDiagnosticComponent,
  },
  {
    path: "add-diagnostic",
    component: AddDiagnosticComponent,
  },
  {
    path: "edit-diagnostic/:id",
    component: AddDiagnosticComponent,
  },

  {
    path: "all-referral",
    component: AllRefferalComponent,
  },
  {
    path: "add-referral",
    component: AddRefferalComponent,
  },
  {
    path: "edit-referral/:id",
    component: AddRefferalComponent,
  },

  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NHSRoutingModule {}
