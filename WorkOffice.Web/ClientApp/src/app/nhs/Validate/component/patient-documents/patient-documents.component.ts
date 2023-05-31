import { SelectionModel } from '@angular/cdk/collections';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SearchCall, SearchParameter } from 'src/app/core/utilities/api-response';
import { PatientDocumentModel } from 'src/app/nhs/patient/patient.model';
import { PatientService } from 'src/app/nhs/patient/patient.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';

@Component({
  selector: 'app-patient-documents',
  templateUrl: './patient-documents.component.html',
  styleUrls: ['./patient-documents.component.css']
})
export class PatientDocumentsComponent

extends UnsubscribeOnDestroyAdapter
implements OnInit
{
patientInformationForm!: UntypedFormGroup;
patientDocumentForm!: UntypedFormGroup;

submitted = false;
loading = false;
isLinear = false;

displayedColumns: string[] = [
  'documentName',
  'consultantName',
  'clinicDate',
  'speciality',
  'dateUploaded',
];
selection = new SelectionModel<PatientDocumentModel>(true, []);
ELEMENT_DATA: PatientService[] = [];
isLoading = false;
totalRows = 0;
pageSize = 10;
currentPage = 0;
pageSizeOptions: number[] = [5, 10, 25, 100];
dataSource: MatTableDataSource<PatientDocumentModel> =
  new MatTableDataSource();
searchQuery = '';
sortOrder = '';
sortField = '';
isTblLoading = false;
@Input() patientId = 0;

@ViewChild(MatPaginator, { static: true })
paginator!: MatPaginator;
@ViewChild(MatSort, { static: true })
sort!: MatSort;

constructor(
  private patientService: PatientService,
) {
  super();
}
ngOnInit() {
  this.loadData(this.searchQuery, this.sortField, this.sortOrder);
}

pageChanged(event: PageEvent) {
  this.pageSize = event.pageSize;
  this.currentPage = event.pageIndex;
  this.loadData(this.searchQuery, this.sortField, this.sortOrder);
}


viewDocument(row: PatientDocumentModel) {
  this.subs.sink = this.patientService
    .downloadDocument(row.patientDocumentId)
    .subscribe((event) => {
      if (event.type === HttpEventType.UploadProgress)
        console.log('event', event);
      else if (event.type === HttpEventType.Response) {
        this.downloadFile(event);
      }
    });
}

private downloadFile = (data: HttpResponse<Blob>) => {
  const downloadedFile = new Blob([data.body as BlobPart], {
    type: data.body?.type,
  });
  const a = document.createElement('a');
  a.setAttribute('style', 'display:none;');
  document.body.appendChild(a);
  a.download = '';
  a.href = URL.createObjectURL(downloadedFile);
  a.target = '_blank';
  a.click();
  document.body.removeChild(a);
};


public loadData(searchQuery: string, sortField: string, sortOrder: string) {
  this.isTblLoading = true;
  const options: SearchCall<SearchParameter> = {
    from: this.currentPage,
    pageSize: this.pageSize,
    sortField,
    sortOrder,
    parameter: {
      searchQuery,
      id:  this.patientId
    },
  };
  this.patientService.getAllPatientDocument(options).subscribe((res) => {
    this.isTblLoading = false;
    this.dataSource.data = res.result;
    this.totalRows = res.totalCount;
  });
}

getDocumentType(documentTypeId: number) {
  switch (documentTypeId) {
    case 1:
      return 'CLINICAL LETTER';
    case 2:
      return 'DISCHARGE LETTER';
    case 3:
      return 'REFERRAL LETTER';
    case 4:
      return 'CLINICAL NOTE';
    case 5:
      return 'TEST RESULT';
    case 6:
      return 'GENERAL LETTER';
    case 7:
      return 'DIAGNOSTIC REQUEST FORM';
    case 8:
      return 'CONSENT LETTER';
    case 9:
      return 'DISCHARGED SUMMARY LETTER';
    default:
      return 'CLINICAL LETTER';
  }
}
}
