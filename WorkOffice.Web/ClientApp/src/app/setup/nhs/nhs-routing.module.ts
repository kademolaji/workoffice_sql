import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { Page404Component } from "./../../authentication/page404/page404.component";
import { AddAppTypeComponent } from "./apptype/add-apptype/add-apptype.component";
import { AllAppTypeComponent } from "./apptype/all-apptype/all-apptype.component";
import { AddConsultantComponent } from "./consultant/add-consultant/add-consultant.component";
import { AllConsultantComponent } from "./consultant/all-consultant/all-consultant.component";
import { AddHospitalComponent } from "./hospital/add-hospital/add-hospital.component";
import { AllHospitalComponent } from "./hospital/all-hospital/all-hospital.component";
import { AddNHSActivityComponent } from "./nhsactivity/add-nhsactivity/add-nhsactivity.component";
import { AllNHSActivityComponent } from "./nhsactivity/all-nhsactivity/all-nhsactivity.component";
import { AddPathwayStatusComponent } from "./pathwaystatus/add-pathwaystatus/add-pathwaystatus.component";
import { AllPathwayStatusComponent } from "./pathwaystatus/all-pathwaystatus/all-pathwaystatus.component";
import { AddRTTComponent } from "./rtt/add-rtt/add-rtt.component";
import { AllRTTComponent } from "./rtt/all-rtt/all-rtt.component";
import { AddSpecialtyComponent } from "./specialty/add-specialty/add-specialty.component";
import { AllSpecialtyComponent } from "./specialty/all-specialty/all-specialty.component";
import { AddWaitingTypeComponent } from "./waitingtype/add-waitingtype/add-waitingtype.component";
import { AllWaitingTypeComponent } from "./waitingtype/all-waitingtype/all-waitingtype.component";
import { AddWardComponent } from "./ward/add-ward/add-ward.component";
import { AllWardComponent } from "./ward/all-ward/all-ward.component";



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
  {
    path: "all-hospital",
    component: AllHospitalComponent,
  },
  {
    path: "add-hospital",
    component: AddHospitalComponent,
  },
  {
     path: "edit-hospital/:id",
    component: AddHospitalComponent,
  },

  {
    path: "all-consultant",
    component: AllConsultantComponent,
  },
  {
    path: "add-consultant",
    component: AddConsultantComponent,
  },
  {
     path: "edit-consultant/:id",
    component: AddConsultantComponent,
  },

  {
    path: "all-nhsactivity",
    component: AllNHSActivityComponent,
  },
  {
    path: "add-nhsactivity",
    component: AddNHSActivityComponent,
  },
  {
     path: "edit-nhsactivity/:id",
    component: AddNHSActivityComponent,
  },

  {
    path: "all-pathwaystatus",
    component: AllPathwayStatusComponent,
  },
  {
    path: "add-pathwaystatus",
    component: AddPathwayStatusComponent,
  },
  {
     path: "edit-pathwaystatus/:id",
    component: AddPathwayStatusComponent,
  },

  {
    path: "all-rtt",
    component: AllRTTComponent,
  },
  {
    path: "add-rtt",
    component: AddRTTComponent,
  },
  {
     path: "edit-rtt/:id",
    component: AddRTTComponent,
  },

  {
    path: "all-specialty",
    component: AllSpecialtyComponent,
  },
  {
    path: "add-specialty",
    component: AddSpecialtyComponent,
  },
  {
     path: "edit-specialty/:id",
    component: AddSpecialtyComponent,
  },

  {
    path: "all-waitingtype",
    component: AllWaitingTypeComponent,
  },
  {
    path: "add-waitingtype",
    component: AddWaitingTypeComponent,
  },
  {
     path: "edit-waitingtype/:id",
    component: AddWaitingTypeComponent,
  },

  {
    path: "all-ward",
    component: AllWardComponent,
  },
  {
    path: "add-ward",
    component: AddWardComponent,
  },
  {
     path: "edit-ward/:id",
    component: AddWardComponent,
  },
  { path: "**", component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NHSRoutingModule {}
