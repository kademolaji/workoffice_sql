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


const routes: Routes = [
  {
    path: "all-appointment",
    component: AllAppointmentComponent,
  },
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
    path: "add-pathway",
    component: AddPathwayComponent,
  },
  {
    path: "validate-now",
    component: ValidateNowComponent,
  },

  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NHSRoutingModule {}
