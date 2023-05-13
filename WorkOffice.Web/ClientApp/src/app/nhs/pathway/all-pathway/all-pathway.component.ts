import { Component, OnInit, ViewChild } from '@angular/core';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { PathwayService } from '../pathway.service';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { Direction } from '@angular/cdk/bidi';
import { SearchCall, SearchParameter } from 'src/app/core/utilities/api-response';
import { DeletePathwayDialogComponent } from './dialog/delete/delete.component';
import { PathwayResponseModel } from '../pathway.model';

@Component({
  selector: 'app-all-pathway',
  templateUrl: './all-pathway.component.html',
  styleUrls: ['./all-pathway.component.css']
})
export class AllPathwayComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
displayedColumns: string[] = [
  'select',
  'pathWayNumber',
  'pathWayStartDate',
  'pathWayEndDate',
  'specialityName',
  'actions',

];
selection = new SelectionModel<PathwayResponseModel>(true, []);
ELEMENT_DATA: PathwayService[] = [];
isLoading = false;
totalRows = 0;
pageSize = 10;
currentPage = 0;
pageSizeOptions: number[] = [5, 10, 25, 100];
dataSource: MatTableDataSource<PathwayResponseModel> =
  new MatTableDataSource();
searchQuery = '';
sortOrder = '';
sortField = '';
isTblLoading = false;
status = 'PartialBooked';

constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public pathwayService: PathwayService,
  private snackBar: MatSnackBar,
  private router: Router,
  private route: ActivatedRoute,

) {
  super();
}
@ViewChild(MatPaginator, { static: true })
paginator!: MatPaginator;
@ViewChild(MatSort, { static: true })
sort!: MatSort;

ngOnInit() {
  this.status = this.route.snapshot.params['status'];
  this.searchQuery =  this.status;
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

addNew() {
  this.router.navigate(['nhs', 'add-pathway']);
}

editCall(row: { patientValidationId: number }) {
  this.router.navigate([
    'nhs',
    'edit-pathway',
    row.patientValidationId,
  ]);
}

deleteItem(row: PathwayResponseModel) {
  let tempDirection: Direction;
  if (localStorage.getItem('isRtl') === 'true') {
    tempDirection = 'rtl';
  } else {
    tempDirection = 'ltr';
  }
  const dialogRef = this.dialog.open(
    DeletePathwayDialogComponent,
    {
      data: row,
      direction: tempDirection,
    }
  );
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
    (data) => data.patientValidationId
  );
  this.subs.sink = this.pathwayService
    .deleteMultiplePathway(targetIds)
    .subscribe({
      next: (res) => {
        if (res.status) {
          this.refresh();
          this.selection = new SelectionModel<PathwayResponseModel>(
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
    },
  };
  this.pathwayService
    .getAllPathway(options)
    .subscribe((res) => {
      this.isTblLoading = false;
      this.dataSource.data = res.result;
      this.totalRows = res.totalCount;
    });
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
}
