import { Component, OnInit, ViewChild } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { DiagnosticModel, DiagnosticResultModel } from '../diagnostic.model';
import { SelectionModel } from '@angular/cdk/collections';
import { Direction } from '@angular/cdk/bidi';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import {
  SearchCall,
  SearchParameter,
} from 'src/app/core/utilities/api-response';
import { PatientService } from '../../patient/patient.service';
import { AddDiagnosticResultDialogComponent } from './dialog/add-diagnostic-result/add-diagnostic-result.component';
import { DeleteDiagnosticResultDialogComponent } from './dialog/delete/delete.component';
import { DiagnosticService } from '../diagnostic.service';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { catchError, debounceTime, distinctUntilChanged, filter, finalize, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-add-diagnostic',
  templateUrl: './add-diagnostic.component.html',
  styleUrls: ['./add-diagnostic.component.css'],
})
export class AddDiagnosticComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  diagnosticForm!: UntypedFormGroup;
  diagnosticResultForm!: UntypedFormGroup;

  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  isLinear = false;
  minLengthTerm=3;

  displayedColumns: string[] = [
    'select',
    'documentName',
    'consultantName',
    'testResultDate',
    'speciality',
    'dateUploaded',
    'actions',
  ];
  selection = new SelectionModel<DiagnosticResultModel>(true, []);
  ELEMENT_DATA: PatientService[] = [];
  isLoading = false;
  totalRows = 0;
  pageSize = 10;
  currentPage = 0;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  dataSource: MatTableDataSource<DiagnosticResultModel> =
    new MatTableDataSource();
  searchQuery = '';
  sortOrder = '';
  sortField = '';
  isTblLoading = false;

  @ViewChild(MatPaginator, { static: true })
  paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true })
  sort!: MatSort;
  patientList: GeneralSettingsModel[] = [];
  specialityList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private diagnosticService: DiagnosticService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.diagnosticForm = this.fb.group({
      patientId: ['', [Validators.required]],
      specialtyId: ['', [Validators.required]],
      dtd: ['', [Validators.required]],
      problem: ['', [Validators.required]],
      status: ['', [Validators.required]],
      consultantName: ['', [Validators.required]],
    });

    this.subs.sink = this.generalSettingsService
      .getConsultant()
      .subscribe((response) => {
        this.consultantList = response.entity;
      });
    this.subs.sink = this.generalSettingsService
      .getSpecialty()
      .subscribe((response) => {
        this.specialityList = response.entity;
      });

    this.diagnosticResultForm = this.fb.group({});
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.loadData(this.searchQuery, this.sortField, this.sortOrder);
      this.subs.sink = this.diagnosticService
        .getDiagnosticById(this.id)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.diagnosticForm.setValue({
                patientId: { label: res.entity.patientName, value: res.entity.patientId},
                specialtyId: res.entity.specialtyId,
                dtd: res.entity.dtd,
                problem: res.entity.problem,
                status: res.entity.status,
                consultantName: res.entity.consultantName,
              });
            }
          },
        });
    }
    this.diagnosticForm.get('patientId')?.valueChanges
    .pipe(
      filter(res => {
        return res !== null && res.length >= this.minLengthTerm
      }),
      distinctUntilChanged(),
      debounceTime(1000),
      tap(() => {
        this.patientList = [];
        this.isLoading = true;
      }),
      switchMap(value => this.generalSettingsService.getPatientList(value as string).pipe(catchError((err) => this.router.navigateByUrl('/')))
        .pipe(
          finalize(() => {
            this.isLoading = false
          }),
        )
      )
    )
    .subscribe((data: any) => {
      if (data['entity'] == undefined) {
        this.patientList = [];
      } else {
        this.patientList = data['entity'];
      }
    });
  }

  displayWith(value: any) {
    return value?.label;
  }
  cancelForm() {
    this.router.navigate(['/nhs/all-diagnostic']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    console.log("diagnosticForm", this.diagnosticForm.value)
    // stop here if form is invalid
    if (this.diagnosticForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new diagnostic failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const diagnostic: DiagnosticModel = {
        diagnosticId: this.id ? this.id : 0,
        patientId: +this.diagnosticForm.value.patientId.value,
        specialtyId: +this.diagnosticForm.value.specialtyId,
        dtd: this.diagnosticForm.value.dtd,
        problem: this.diagnosticForm.value.problem,
        status: this.diagnosticForm.value.status,
        consultantName: this.diagnosticForm.value.consultantName,
        patientName: '',
        specialty: '',
      };
      this.subs.sink = this.diagnosticService
        .addDiagnostic(diagnostic)
        .subscribe({
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
              this.router.navigate(['/nhs/all-diagnostic']);
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
    const row: DiagnosticResultModel = {
      diagnosticResultId: 0,
      diagnosticId: +this.id,
      problem: '',
      consultantName: '',
      documentName: '',
      documentExtension: '',
      documentFile: '',
      testResultDate: new Date(),
      specialityId: 0,
      dateUploaded: new Date(),
    };
    let tempDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      tempDirection = 'rtl';
    } else {
      tempDirection = 'ltr';
    }
    const dialogRef = this.dialog.open(AddDiagnosticResultDialogComponent, {
      data: row,
      direction: tempDirection,
    });
    this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
      console.log('result', result);
      this.refresh();
      this.showNotification(
        'snackbar-success',
        'Record Uploaded Successfully...!!!',
        'top',
        'right'
      );
    });
  }

  viewDocument(row: DiagnosticResultModel) {
    this.subs.sink = this.diagnosticService
      .downloadDocument(row.diagnosticResultId)
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

  deleteItem(row: DiagnosticResultModel) {
    let tempDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      tempDirection = 'rtl';
    } else {
      tempDirection = 'ltr';
    }
    const dialogRef = this.dialog.open(DeleteDiagnosticResultDialogComponent, {
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
      (data) => data.diagnosticResultId
    );
    this.subs.sink = this.diagnosticService
      .deleteMultipleDiagnostic(targetIds)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.refresh();
            this.selection = new SelectionModel<DiagnosticResultModel>(
              true,
              []
            );
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
        id: this.id
      },
    };
    this.diagnosticService.getAllDiagnosticResult(options).subscribe((res) => {
      this.isTblLoading = false;
      this.dataSource.data = res.result;
      this.totalRows = res.totalCount;
    });
  }
}
