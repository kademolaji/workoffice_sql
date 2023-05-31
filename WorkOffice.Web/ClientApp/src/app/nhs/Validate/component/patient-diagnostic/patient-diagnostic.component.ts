import { SelectionModel } from '@angular/cdk/collections';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { SearchCall, SearchParameter } from 'src/app/core/utilities/api-response';
import { DiagnosticModel } from 'src/app/nhs/diagnostic/diagnostic.model';
import { DiagnosticService } from 'src/app/nhs/diagnostic/diagnostic.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';

@Component({
  selector: 'app-patient-diagnostic',
  templateUrl: './patient-diagnostic.component.html',
  styleUrls: ['./patient-diagnostic.component.css']
})
export class PatientDiagnosticComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{

displayedColumns: string[] = [
  'patientName',
  'consultantName',
  'specialty',
  'problem',
  'status',
];
selection = new SelectionModel<DiagnosticModel>(true, []);
ELEMENT_DATA: DiagnosticService[] = [];
isLoading = false;
totalRows = 0;
pageSize = 10;
currentPage = 0;
pageSizeOptions: number[] = [5, 10, 25, 100];
dataSource: MatTableDataSource<DiagnosticModel> =
  new MatTableDataSource();
searchQuery = '';
sortOrder = '';
sortField = '';
isTblLoading = false;
@Input() patientId = 0;

constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public DiagnosticService: DiagnosticService,
  private snackBar: MatSnackBar,
  private router: Router
) {
  super();
}
@ViewChild(MatPaginator, { static: true })
paginator!: MatPaginator;
@ViewChild(MatSort, { static: true })
sort!: MatSort;

ngOnInit() {
  this.loadData(this.searchQuery, this.sortField, this.sortOrder);
}

pageChanged(event: PageEvent) {
  this.pageSize = event.pageSize;
  this.currentPage = event.pageIndex;
  this.loadData(this.searchQuery, this.sortField, this.sortOrder);
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
      id: this.patientId
    },
  };
  this.DiagnosticService
    .getAllDiagnostic(options)
    .subscribe((res) => {
      this.isTblLoading = false;
      this.dataSource.data = res.result;
      this.totalRows = res.totalCount;
    });
}
}

