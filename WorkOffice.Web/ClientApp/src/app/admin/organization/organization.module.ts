import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddLocationComponent } from './location/add-location/add-location.component';
import { AllLocationComponent } from './location/all-location/all-location.component';
import { AddStructureDefinitionComponent } from './structure-definition/add-structure-definition/add-structure-definition.component';
import { AllStructureDefinitionComponent } from './structure-definition/all-structure-definition/all-structure-definition.component';
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
import { LocationService } from './location/location.service';
import { StructureDefinitionService } from './structure-definition/structure-definition.service';
import { OrganizationRoutingModule } from './organization-routing.module';
import { DeleteLocationDialogComponent } from './location/all-location/dialog/delete/delete.component';
import { DeleteStructureDefinitionDialogComponent } from './structure-definition/all-structure-definition/dialog/delete/delete.component';
import { AllCompanyStructureComponent } from './company-structure/all-company-structure/all-company-structure.component';
import { AddCompanyStructureComponent } from './company-structure/add-company-structure/add-company-structure.component';
import { DeleteCompanyStructureDialogComponent } from './company-structure/all-company-structure/dialog/delete/delete.component';
import { CompanyStructureService } from './company-structure/company-structure.service';
import { AddGeneralInformationComponent } from './general-information/add-general-information/add-general-information.component';
import { AllGeneralInformationComponent } from './general-information/all-general-information/all-general-information.component';
import { AllCustomIdentitySettingsComponent } from './custom-identity-format/all-custom-identity-settings/all-custom-identity-settings.component';
import { AddCustomIdentitySettingsComponent } from './custom-identity-format/add-custom-identity-settings/add-custom-identity-settings.component';

@NgModule({
  declarations: [AddLocationComponent,
    AllLocationComponent,
    AddLocationComponent,
    DeleteLocationDialogComponent,

    AddStructureDefinitionComponent,
    AllStructureDefinitionComponent,
    DeleteStructureDefinitionDialogComponent,

    AllCompanyStructureComponent,
    AddCompanyStructureComponent,
    DeleteCompanyStructureDialogComponent,

    AllGeneralInformationComponent,
    AddGeneralInformationComponent,

    AllCustomIdentitySettingsComponent,
    AddCustomIdentitySettingsComponent,


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
    OrganizationRoutingModule,
    ComponentsModule,
    SharedModule,
  ],
  providers: [LocationService, StructureDefinitionService, CompanyStructureService ],
})
export class OrganizationModule { }
