import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ComponentsModule } from 'src/app/shared/components/components.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { NHSRoutingModule } from './nhs-routing.module';
import { HospitalService } from './hospital/hospital.service';
import { NHSActivityService } from './nhsactivity/nhsactivity.service';
import { AddNHSActivityComponent } from './nhsactivity/add-nhsactivity/add-nhsactivity.component';
import { AllNHSActivityComponent } from './nhsactivity/all-nhsactivity/all-nhsactivity.component';
import { DeleteNHSActivityDialogComponent } from './nhsactivity/all-nhsactivity/dialog/delete/delete.component';
import { AddAppTypeComponent } from './apptype/add-apptype/add-apptype.component';
import { AllAppTypeComponent } from './apptype/all-apptype/all-apptype.component';
import { DeleteAppTypeDialogComponent } from './apptype/all-apptype/dialog/delete/delete.component';
import { AddHospitalComponent } from './hospital/add-hospital/add-hospital.component';
import { AllHospitalComponent } from './hospital/all-hospital/all-hospital.component';
import { DeleteHospitalDialogComponent } from './hospital/all-hospital/dialog/delete/delete.component';
import { AppTypeService } from './apptype/apptype.service';
import { ConsultantService } from './consultant/consultant.service';
import { PathwayStatusService } from './pathwaystatus/pathwaystatus.service';
import { RTTService } from './rtt/rtt.service';
import { SpecialtyService } from './specialty/specialty.service';
import { WaitingTypeService } from './waitingtype/waitingtype.service';
import { WardService } from './ward/ward.service';
import { AddConsultantComponent } from './consultant/add-consultant/add-consultant.component';
import { AllConsultantComponent } from './consultant/all-consultant/all-consultant.component';
import { DeleteConsultantDialogComponent } from './consultant/all-consultant/dialog/delete/delete.component';
import { AddPathwayStatusComponent } from './pathwaystatus/add-pathwaystatus/add-pathwaystatus.component';
import { AllPathwayStatusComponent } from './pathwaystatus/all-pathwaystatus/all-pathwaystatus.component';
import { DeletePathwayStatusDialogComponent } from './pathwaystatus/all-pathwaystatus/dialog/delete/delete.component';
import { AddRTTComponent } from './rtt/add-rtt/add-rtt.component';
import { AllRTTComponent } from './rtt/all-rtt/all-rtt.component';
import { DeleteRTTDialogComponent } from './rtt/all-rtt/dialog/delete/delete.component';
import { AddSpecialtyComponent } from './specialty/add-specialty/add-specialty.component';
import { AllSpecialtyComponent } from './specialty/all-specialty/all-specialty.component';
import { DeleteSpecialtyDialogComponent } from './specialty/all-specialty/dialog/delete/delete.component';
import { AddWaitingTypeComponent } from './waitingtype/add-waitingtype/add-waitingtype.component';
import { AllWaitingTypeComponent } from './waitingtype/all-waitingtype/all-waitingtype.component';
import { DeleteWaitingTypeDialogComponent } from './waitingtype/all-waitingtype/dialog/delete/delete.component';
import { AddWardComponent } from './ward/add-ward/add-ward.component';
import { AllWardComponent } from './ward/all-ward/all-ward.component';
import { DeleteWardDialogComponent } from './ward/all-ward/dialog/delete/delete.component';

@NgModule({
  declarations: [
    AddAppTypeComponent,AllAppTypeComponent,DeleteAppTypeDialogComponent,
    AddHospitalComponent,AllHospitalComponent,DeleteHospitalDialogComponent,
    AddNHSActivityComponent,AllNHSActivityComponent,DeleteNHSActivityDialogComponent,
    AddConsultantComponent,AllConsultantComponent,DeleteConsultantDialogComponent,
    AddPathwayStatusComponent, AllPathwayStatusComponent, DeletePathwayStatusDialogComponent,
    AddRTTComponent,AllRTTComponent,DeleteRTTDialogComponent,
    AddSpecialtyComponent,AllSpecialtyComponent,DeleteSpecialtyDialogComponent,
    AddWaitingTypeComponent,AllWaitingTypeComponent,DeleteWaitingTypeDialogComponent,
    AddWardComponent,AllWardComponent,DeleteWardDialogComponent,

   
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
    NHSRoutingModule,
    ComponentsModule,
    SharedModule,
  ],
  providers: [ AppTypeService,HospitalService, NHSActivityService, ConsultantService,PathwayStatusService,
  RTTService,SpecialtyService,WaitingTypeService,WardService],
})
export class NHSModule { }
