import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NHSRoutingModule } from './nhs-routing.module';
import { AddAppointmentComponent } from './appointment/add-appointment/add-appointment.component';
import { AllAppointmentComponent } from './appointment/all-appointment/all-appointment.component';
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
import { ComponentsModule } from '../shared/components/components.module';
import { SharedModule } from '../shared/shared.module';
import { AddPatientComponent } from './patient/add-patient/add-patient.component';
import { AllPatientComponent } from './patient/all-patient/all-patient.component';
import { AddWaitinglistComponent } from './waitinglist/add-waitinglist/add-waitinglist.component';
import { AllWaitinglistComponent } from './waitinglist/all-waitinglist/all-waitinglist.component';
import { PatientService } from './patient/patient.service';
import { AppointmentService } from './appointment/appointment.service';
import { WaitinglistService } from './waitinglist/waitinglist.service';
import { DeleteAppointmentDialogComponent } from './appointment/all-appointment/dialog/delete/delete.component';
import { DeletePatientDialogComponent } from './patient/all-patient/dialog/delete/delete.component';
import { DeleteWaitinglistDialogComponent } from './waitinglist/all-waitinglist/dialog/delete/delete.component';

@NgModule({
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
    MatProgressSpinnerModule,
    NHSRoutingModule,
    ComponentsModule,
    SharedModule,
  ],
  declarations: [
    AllAppointmentComponent,
    AddAppointmentComponent,
    DeleteAppointmentDialogComponent,

    AllPatientComponent,
    AddPatientComponent,
    DeletePatientDialogComponent,

    AllWaitinglistComponent,
    AddWaitinglistComponent,
    DeleteWaitinglistDialogComponent,
  ],
  providers: [PatientService, AppointmentService, WaitinglistService],
})
export class NhsModule {}
