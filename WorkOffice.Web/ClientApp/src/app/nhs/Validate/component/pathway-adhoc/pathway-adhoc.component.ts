import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { PatientValidationDetailsModel } from '../../validate.model';
import { ValidateNowService } from '../../validate.service';
import { SelectionModel } from '@angular/cdk/collections';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SearchCall, SearchParameter } from 'src/app/core/utilities/api-response';
import { Direction } from '@angular/cdk/bidi';
import { DeletePatientValidationDetailsDialogComponent } from './dialog/delete/delete.component';
import { PathwayAdhocDialogComponent } from './dialog/pathway-adhoc-dialog/pathway-adhoc-dialog.component';
import { PathwayMergeDialogComponent } from './dialog/pathway-merge-dialog/pathway-merge-dialog.component';

@Component({
  selector: 'app-pathway-adhoc',
  templateUrl: './pathway-adhoc.component.html',
  styleUrls: ['./pathway-adhoc.component.css']
})
export class PathwayAdhocComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{

displayedColumns: string[] = [
  'activity',
  'startDate',
  'specialty',
  'status',
  'consultant',
  'endDate',
  'actions'
];
selection = new SelectionModel<PatientValidationDetailsModel>(true, []);
ELEMENT_DATA: ValidateNowService[] = [];
isLoading = false;
totalRows = 0;
pageSize = 10;
currentPage = 0;
pageSizeOptions: number[] = [5, 10, 25, 100];
dataSource: MatTableDataSource<PatientValidationDetailsModel> =
  new MatTableDataSource();
searchQuery = '';
sortOrder = '';
sortField = '';
isTblLoading = false;
@Input() patientValidationId = 0;
@Input() patientId = 0;
@Output() refreshPathwayData: EventEmitter<any> = new EventEmitter();

constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public validateNowService: ValidateNowService,
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
      id: this.patientValidationId
    },
  };
  this.validateNowService
    .getAllPatientValidationDetails(options)
    .subscribe((res) => {
      this.isTblLoading = false;
      this.dataSource.data = res.result;
      this.totalRows = res.totalCount;
    });
}


mergeCall(row: PatientValidationDetailsModel) {
  let tempDirection: Direction;
  if (localStorage.getItem('isRtl') === 'true') {
    tempDirection = 'rtl';
  } else {
    tempDirection = 'ltr';
  }
  const mergeDialogRef = this.dialog.open(PathwayMergeDialogComponent, {
    data: row,
    direction: tempDirection,
      width: '600px',
      maxWidth: '600px',
  });
  this.subs.sink = mergeDialogRef.afterClosed().subscribe((result) => {
    this.refreshPathwayData.emit('refresh');
    this.refresh();
  });
}

deleteItem(row: PatientValidationDetailsModel) {
  let tempDirection: Direction;
  if (localStorage.getItem('isRtl') === 'true') {
    tempDirection = 'rtl';
  } else {
    tempDirection = 'ltr';
  }
  const dialogRef = this.dialog.open(
    DeletePatientValidationDetailsDialogComponent,
    {
      data: row,
      direction: tempDirection,
    }
  );
  this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
    this.refreshPathwayData.emit('refresh');
      this.refresh();
  });
}

refresh() {
  this.searchQuery = '';
  this.sortOrder = '';
  this.sortField = '';
  this.loadData(this.searchQuery, this.sortField, this.sortOrder);
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

editCall(row: PatientValidationDetailsModel) {
  let tempDirection: Direction;
  if (localStorage.getItem('isRtl') === 'true') {
    tempDirection = 'rtl';
  } else {
    tempDirection = 'ltr';
  }
  const dialogRef = this.dialog.open(PathwayAdhocDialogComponent, {
    data: row,
    direction: tempDirection,
  });
  this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
    this.refreshPathwayData.emit('refresh');
    this.refresh();
  });
}

addNew() {
  const row: PatientValidationDetailsModel = {
    patientValidationDetailsId: 0,
    patientValidationId: this.patientValidationId,
    pathWayStatusId:0,
    specialityId: 0,
    date: '',
    consultantId: 0,
    endDate: '',
    patientId: this.patientId,
    activity: '',
    specialityCode: '',
    specialityName: '',
    pathWayStatusCode: '',
    pathWayStatusName: '',
    consultantCode: '',
    consultantName: '',
  };
  let tempDirection: Direction;
  if (localStorage.getItem('isRtl') === 'true') {
    tempDirection = 'rtl';
  } else {
    tempDirection = 'ltr';
  }
  const dialogRef = this.dialog.open(PathwayAdhocDialogComponent, {
    data: row,
    direction: tempDirection,
  });
  this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
    this.refreshPathwayData.emit('refresh');
    this.refresh();
  });
}

}


