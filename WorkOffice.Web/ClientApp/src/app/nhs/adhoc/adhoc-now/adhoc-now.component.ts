import { SelectionModel } from '@angular/cdk/collections';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { SearchCall, SearchParameter } from 'src/app/core/utilities/api-response';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { PatientModel } from '../../patient/patient.model';
import { PatientService } from '../../patient/patient.service';

@Component({
  selector: 'app-adhoc-now',
  templateUrl: './adhoc-now.component.html',
  styleUrls: ['./adhoc-now.component.css']
})
export class AdhocNowComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
displayedColumns: string[] = [
  'patientName',
  'patientUniqueNumber',
  'address',
  'email',
  'phoneNumber',
  'dob',
];
selection = new SelectionModel<PatientModel>(true, []);
ELEMENT_DATA: PatientService[] = [];
isLoading = false;
totalRows = 0;
pageSize = 10;
currentPage = 0;
pageSizeOptions: number[] = [5, 10, 25, 100];
dataSource: MatTableDataSource<PatientModel> = new MatTableDataSource();
searchQuery = '';
sortOrder = '';
sortField = '';
isTblLoading = false;

constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public patientService: PatientService,
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

selectCall(row: { patientId: number }) {
  this.router.navigate(['nhs', 'adhoc', row.patientId]);
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
    },
  };
  this.patientService.getAllPatient(options).subscribe((res) => {
    this.isTblLoading = false;
    this.dataSource.data = res.result;
    this.totalRows = res.totalCount;
  });
}
}
