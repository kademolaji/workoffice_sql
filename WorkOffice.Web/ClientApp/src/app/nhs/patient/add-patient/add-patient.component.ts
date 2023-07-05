import { Component, OnInit, ViewChild } from '@angular/core';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
} from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { PatientDocumentModel, PatientModel } from '../patient.model';
import { PatientService } from '../patient.service';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { Direction } from '@angular/cdk/bidi';
import {
  SearchCall,
  SearchParameter,
} from 'src/app/core/utilities/api-response';
import { AddPatientDocumentDialogComponent } from './dialog/add-patient-document/add-patient-document.component';
import { DeletePatientDocumentDialogComponent } from './dialog/delete/delete.component';
import Swal from 'sweetalert2';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

@Component({
  selector: 'app-add-patient',
  templateUrl: './add-patient.component.html',
  styleUrls: ['./add-patient.component.css'],
})
export class AddPatientComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  patientInformationForm!: UntypedFormGroup;
  patientDocumentForm!: UntypedFormGroup;

  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  isLinear = false;

  displayedColumns: string[] = [
    'select',
    'documentName',
    'consultantName',
    'clinicDate',
    'speciality',
    'dateUploaded',
    'actions',
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

  @ViewChild(MatPaginator, { static: true })
  paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true })
  sort!: MatSort;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private patientService: PatientService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog
  ) {
    super();
  }
  ngOnInit() {
    this.patientInformationForm = this.fb.group({
      districtNumber: [''],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      middleName: ['', [Validators.required]],
      dob: ['', [Validators.required]],
      age: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phoneNo: ['', [Validators.required]],
      email: ['', [Validators.required]],
      sex: ['', [Validators.required]],
      postalCode: ['', [Validators.required]],
      nhsNumber: [''],
    });

    this.patientDocumentForm = this.fb.group({});

    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.loadData(this.searchQuery, this.sortField, this.sortOrder);
      this.subs.sink = this.patientService.getPatientById(this.id).subscribe({
        next: (res) => {
          if (res.status) {
            this.patientInformationForm.setValue({
              districtNumber: res.entity.districtNumber,
              firstName: res.entity.firstName,
              lastName: res.entity.lastName,
              middleName: res.entity.middleName,
              dob: res.entity.dob,
              age: res.entity.age,
              address: res.entity.address,
              phoneNo: res.entity.phoneNo,
              email: res.entity.email,
              sex: res.entity.sex,
              postalCode: res.entity.postalCode,
              nhsNumber: res.entity.nhsNumber,
            });
          }
        },
      });
    }
  }
  cancelForm() {
    this.router.navigate(['/nhs/all-patient']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.patientInformationForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new patient information failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const patient: PatientModel = {
        patientId: this.id ? this.id : 0,
        districtNumber: this.patientInformationForm.value.districtNumber,
        firstName: this.patientInformationForm.value.firstName,
        lastName: this.patientInformationForm.value.lastName,
        middleName: this.patientInformationForm.value.middleName,
        dob: this.patientInformationForm.value.dob,
        age: Number(this.patientInformationForm.value.age),
        address: this.patientInformationForm.value.address,
        phoneNo: this.patientInformationForm.value.phoneNo,
        email: this.patientInformationForm.value.email,
        sex: this.patientInformationForm.value.sex,
        postalCode: this.patientInformationForm.value.postalCode,
        nhsNumber: this.patientInformationForm.value.nhsNumber,
        active: true,
        fullName: 'null',
      };
      this.subs.sink = this.patientService.addPatient(patient).subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.id = +res.id;
            this.showNotification(
              'snackbar-success',
              res.message,
              'top',
              'right'
            );
            this.router.navigate(['/nhs/all-patient']);
          } else {
            this.showNotification(
              'snackbar-danger',
              res.message,
              'top',
              'right'
            );
          }
        },
        error: (error) => {
          this.showNotification('snackbar-danger', error, 'top', 'right');
          this.submitted = false;
          this.loading = false;
        },
      });
    }
  }

  showNotification(
    colorName: string,
    text: string,
    placementFrom: MatSnackBarVerticalPosition,
    placementAlign: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.searchQuery = filterValue.trim();
    this.loadData(this.searchQuery, this.sortField, this.sortOrder);
  }

  sortData(sort: Sort) {
    this.sortField = sort.active;
    this.sortOrder = sort.direction;
    this.loadData(this.searchQuery, this.sortField, this.sortOrder);
  }

  pageChanged(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    this.loadData(this.searchQuery, this.sortField, this.sortOrder);
  }

  refresh() {
    this.searchQuery = '';
    this.sortOrder = '';
    this.sortField = '';
    this.loadData(this.searchQuery, this.sortField, this.sortOrder);
  }

  addNew() {
    const row: PatientDocumentModel = {
      patientDocumentId: 0,
      documentTypeId: 1,
      patientId: +this.id,
      physicalLocation: '',
      documentName: '',
      documentExtension: '',
      documentFile: '',
      clinicDate: new Date(),
      specialityId: 0,
      consultantName: '',
      dateUploaded: new Date(),
    };
    let tempDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      tempDirection = 'rtl';
    } else {
      tempDirection = 'ltr';
    }
    const dialogRef = this.dialog.open(AddPatientDocumentDialogComponent, {
      data: row,
      direction: tempDirection,
    });
    this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
      this.refresh();
      this.showNotification(
        'snackbar-success',
        'Record Uploaded Successfully...!!!',
        'top',
        'right'
      );
    });
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

  deleteItem(row: PatientDocumentModel) {
    let tempDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      tempDirection = 'rtl';
    } else {
      tempDirection = 'ltr';
    }
    const dialogRef = this.dialog.open(DeletePatientDocumentDialogComponent, {
      data: row,
      direction: tempDirection,
    });
    this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
      this.refresh();
      this.showNotification(
        'snackbar-success',
        'Delete Record Successfully...!!!',
        'top',
        'right'
      );
    });
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource?.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource?.data.forEach((row) => this.selection.select(row));
  }
  removeSelectedRows() {
    const totalSelect = this.selection.selected.length;
    const targetIds = this.selection.selected.map(
      (data) => data.patientDocumentId
    );
    this.subs.sink = this.patientService
      .deleteMultiplePatient(targetIds)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.refresh();
            this.selection = new SelectionModel<PatientDocumentModel>(true, []);
            this.showNotification(
              'snackbar-success',
              totalSelect + ' Record Delete Successfully...!!!',
              'top',
              'right'
            );
          }
        },
        error: (error) => {
          this.showNotification('snackbar-danger', error, 'top', 'right');
        },
      });
  }
  public loadData(searchQuery: string, sortField: string, sortOrder: string) {
    this.isTblLoading = true;
    const options: SearchCall<SearchParameter> = {
      from: this.currentPage,
      pageSize: this.pageSize,
      sortField,
      sortOrder,
      parameter: {
        searchQuery,
        id:  this.id
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

  updateAgeWithDOB(e:MatDatepickerInputEvent<Date>){

    if(e.value){
      const dob = new Date(e.value)
      const ageTilNowInMilliseconds = Date.now() - dob.getTime();
      const ageDate = new Date(ageTilNowInMilliseconds);

       this.patientInformationForm.patchValue({
        age:  Math.abs(ageDate.getUTCFullYear() - 1970),
      });
    }
  }
}
