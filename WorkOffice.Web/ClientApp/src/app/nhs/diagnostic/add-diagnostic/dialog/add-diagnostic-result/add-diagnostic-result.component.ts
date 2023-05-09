import { Component, Inject, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';

import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DiagnosticService } from '../../../diagnostic.service';
import { DiagnosticResultModel } from '../../../diagnostic.model';

@Component({
  selector: 'app-add-diagnostic-result',
  templateUrl: './add-diagnostic-result.component.html',
  styleUrls: ['./add-diagnostic-result.component.css'],
})
export class AddDiagnosticResultDialogComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  diagnosticResultForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  specialityList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];

  constructor(
    public dialogRef: MatDialogRef<AddDiagnosticResultDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DiagnosticResultModel,
    private fb: UntypedFormBuilder,
    private diagnosticService: DiagnosticService,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.diagnosticResultForm = this.fb.group({
      problem: ['', [Validators.required]],
      documentName: ['', [Validators.required]],
      testResultDate: ['', [Validators.required]],
      specialityId: ['', [Validators.required]],
      consultantName: ['', [Validators.required]],
      uploadFile: [''],
    });
    if (this.data.diagnosticResultId > 0) {
      this.diagnosticResultForm.setValue({
        documentName: this.data.documentName,
        specialityId: this.data.specialityId,
        problem:  this.data.problem,
        testResultDate:  this.data.testResultDate,
        consultantName:  this.data.consultantName,
      });
    }

    this.subs.sink = this.generalSettingsService
      .getSpecialty()
      .subscribe((response) => {
        this.specialityList = response.entity;
      });
      this.subs.sink = this.generalSettingsService
      .getConsultant()
      .subscribe((response) => {
        this.consultantList = response.entity;
      });

  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.diagnosticResultForm.invalid) {
      return;
    } else {
      const formData = new FormData();
      console.log('this.DiagnosticResultForm', this.diagnosticResultForm.value);
     formData.append('DiagnosticResultId', this.data.diagnosticResultId.toString());
      formData.append('diagnosticId',  this.data.diagnosticId.toString());
      formData.append('problem', this.diagnosticResultForm.value.problem);
      formData.append('documentName', this.diagnosticResultForm.value.documentName);
      formData.append('testResultDate', new Date(this.diagnosticResultForm.value.testResultDate).toISOString());
      formData.append('specialityId', this.diagnosticResultForm.value.specialityId);
      formData.append('consultantName', this.diagnosticResultForm.value.consultantName);
      formData.append('file', this.diagnosticResultForm.value.uploadFile);
      this.subs.sink = this.diagnosticService.addDiagnosticResult(formData).subscribe({
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
