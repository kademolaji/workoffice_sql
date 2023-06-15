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
import { ValidateNowComponent } from './Validate/validate-now/validate-now.component';
import { AddPathwayComponent } from './pathway/add-pathway/add-pathway.component';
import { MatStepperModule } from '@angular/material/stepper';
import { AddPatientDocumentDialogComponent } from './patient/add-patient/dialog/add-patient-document/add-patient-document.component';
import { DeletePatientDocumentDialogComponent } from './patient/add-patient/dialog/delete/delete.component';
import { AddDiagnosticComponent } from './diagnostic/add-diagnostic/add-diagnostic.component';
import { AddDiagnosticResultDialogComponent } from './diagnostic/add-diagnostic/dialog/add-diagnostic-result/add-diagnostic-result.component';
import { DeleteDiagnosticResultDialogComponent } from './diagnostic/add-diagnostic/dialog/delete/delete.component';
import { AllDiagnosticComponent } from './diagnostic/all-diagnostic/all-diagnostic.component';
import { DeleteDiagnosticDialogComponent } from './diagnostic/all-diagnostic/dialog/delete/delete.component';
import { DiagnosticService } from './diagnostic/diagnostic.service';
import { AddRefferalComponent } from './refferal/add-refferal/add-refferal.component';
import { DeleteReferralDialogComponent } from './refferal/all-refferal/dialog/delete/delete.component';
import { AllRefferalComponent } from './refferal/all-refferal/all-refferal.component';
import { ReferralService } from './refferal/referral.service';
import { DeletePathwayDialogComponent } from './pathway/all-pathway/dialog/delete/delete.component';
import { AllPathwayComponent } from './pathway/all-pathway/all-pathway.component';
import { PathwayService } from './pathway/pathway.service';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CancelAppointmentDialogComponent } from './appointment/all-appointment/dialog/cancel/cancel.component';
import { ValidateNowPathwaysComponent } from './Validate/validate-now-pathways/validate-now-pathways.component';
import { ValidatePatientComponent } from './Validate/validate-patient/validate-patient.component';
import { PatientAppointmentsComponent } from './Validate/component/patient-appointments/patient-appointments.component';
import { PatientDiagnosticComponent } from './Validate/component/patient-diagnostic/patient-diagnostic.component';
import { PatientReferralComponent } from './Validate/component/patient-referral/patient-referral.component';
import { PatientWaitinglistComponent } from './Validate/component/patient-waitinglist/patient-waitinglist.component';
import { PatientDocumentsComponent } from './Validate/component/patient-documents/patient-documents.component';
import { PathwayAdhocComponent } from './Validate/component/pathway-adhoc/pathway-adhoc.component';
import { PathwayAdhocDialogComponent } from './Validate/component/pathway-adhoc/dialog/pathway-adhoc-dialog/pathway-adhoc-dialog.component';
import { DeletePatientValidationDetailsDialogComponent } from './Validate/component/pathway-adhoc/dialog/delete/delete.component';
import { ValidateNowService } from './Validate/validate.service';
import { PathwayMergeDialogComponent } from './Validate/component/pathway-adhoc/dialog/pathway-merge-dialog/pathway-merge-dialog.component';

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
    MatStepperModule,
    MatAutocompleteModule,
  ],
  declarations: [
    AllAppointmentComponent,
    AddAppointmentComponent,
    DeleteAppointmentDialogComponent,
    CancelAppointmentDialogComponent,

    AllPatientComponent,
    AddPatientComponent,
    DeletePatientDialogComponent,
    AddPatientDocumentDialogComponent,
    DeletePatientDocumentDialogComponent,

    AllWaitinglistComponent,
    AddWaitinglistComponent,
    DeleteWaitinglistDialogComponent,

    AllPathwayComponent,
    AddPathwayComponent,
    DeletePathwayDialogComponent,

    AllDiagnosticComponent,
    DeleteDiagnosticDialogComponent,
    AddDiagnosticComponent,
    AddDiagnosticResultDialogComponent,
    DeleteDiagnosticResultDialogComponent,

    AddRefferalComponent,
    AllRefferalComponent,
    DeleteReferralDialogComponent,

    ValidateNowComponent,
    ValidateNowPathwaysComponent,
    ValidatePatientComponent,

    PatientAppointmentsComponent,
    PatientDiagnosticComponent,
    PatientReferralComponent,
    PatientWaitinglistComponent,
    PatientDocumentsComponent,
    PathwayAdhocDialogComponent,
    PathwayAdhocComponent,
    DeletePatientValidationDetailsDialogComponent,
    PathwayMergeDialogComponent
  ],
  providers: [PatientService, AppointmentService, WaitinglistService, DiagnosticService, ReferralService, PathwayService, ValidateNowService],
})
export class NhsModule {}
