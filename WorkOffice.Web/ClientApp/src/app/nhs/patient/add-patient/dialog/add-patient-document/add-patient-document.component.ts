import { Component, Inject, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';

import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { PatientService } from '../../../patient.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PatientDocumentModel } from '../../../patient.model';

@Component({
  selector: 'app-add-patient-document',
  templateUrl: './add-patient-document.component.html',
  styleUrls: ['./add-patient-document.component.css'],
})
export class AddPatientDocumentDialogComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  patientDocumentForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  specialityList: GeneralSettingsModel[] = [];

  constructor(
    public dialogRef: MatDialogRef<AddPatientDocumentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PatientDocumentModel,
    private fb: UntypedFormBuilder,
    private patientService: PatientService,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.patientDocumentForm = this.fb.group({
      // documentTypeId: ['', [Validators.required]],
      physicalLocation: ['', [Validators.required]],
      documentName: ['', [Validators.required]],
      clinicDate: ['', [Validators.required]],
      specialityId: ['', [Validators.required]],
      uploadFile: [''],
    });
    if (this.data.patientDocumentId > 0) {
      this.patientDocumentForm.setValue({
        // documentTypeId: this.data.documentTypeId,
        physicalLocation: this.data.physicalLocation,
        documentName: this.data.documentName,
        clinicDate: this.data.clinicDate,
        specialityId: this.data.specialityId,
      });
    }

    this.subs.sink = this.generalSettingsService
      .getSpecialty()
      .subscribe((response) => {
        this.specialityList = response.entity;
      });
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.patientDocumentForm.invalid) {
      return;
    } else {
      const formData = new FormData();
      formData.append('patientDocumentId', this.data.patientDocumentId.toString());
      formData.append('patientId',  this.data.patientId.toString());
      formData.append('documentTypeId', this.data.documentTypeId.toString());
      formData.append('physicalLocation', this.patientDocumentForm.value.physicalLocation);
      formData.append('documentName', this.patientDocumentForm.value.documentName);
      formData.append('clinicDate', new Date(this.patientDocumentForm.value.clinicDate).toISOString());
      formData.append('specialityId', this.patientDocumentForm.value.specialityId);
      formData.append('file', this.patientDocumentForm.value.uploadFile);
      this.subs.sink = this.patientService.addPatientDocument(formData).subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.dialogRef.close()
          }
        },
        error: (error) => {
          this.submitted = false;
          this.loading = false;
        },
      });
    }
  }

  onNoClick() {
    this.dialogRef.close();
  }
}
