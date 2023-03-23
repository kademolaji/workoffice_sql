import { Direction } from '@angular/cdk/bidi';
import { SelectionModel, DataSource } from '@angular/cdk/collections';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { fromEvent, BehaviorSubject, Observable, merge, map } from 'rxjs';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { LocationModel } from '../location.model';
import { LocationService } from '../location.service';
import { DeleteLocationDialogComponent } from './dialog/delete/delete.component';

@Component({
  selector: 'app-all-location',
  templateUrl: './all-location.component.html',
  styleUrls: ['./all-location.component.css']
})
export class AllLocationComponent  extends UnsubscribeOnDestroyAdapter
implements OnInit
{
displayedColumns = [
  'select',
  'name',
  'country',
  'state',
  'city',
  'address',
  'zipCode',
  'phone',
  'actions',
];
exampleDatabase!: LocationService | null;
dataSource!: ExampleDataSource;
selection = new SelectionModel<LocationModel>(true, []);
index!: number;
id = 0;
locations!: LocationModel | null;
constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public locationService: LocationService,
  private snackBar: MatSnackBar,
  private router: Router,
) {
  super();
}
@ViewChild(MatPaginator, { static: true })
paginator!: MatPaginator;
@ViewChild(MatSort, { static: true })
sort!: MatSort;
@ViewChild('filter', { static: true })
filter!: ElementRef;
ngOnInit() {
  this.loadData();
}
refresh() {
  this.loadData();
}
addNew() {
  this.router.navigate(['/admin/company/add-location']);
}



editCall(row: { id: number }) {
  this.id = row.id;

}

deleteItem(row: { id: number }) {
  this.id = row.id;
  let tempDirection: Direction;
  if (localStorage.getItem('isRtl') === 'true') {
    tempDirection = 'rtl';
  } else {
    tempDirection = 'ltr';
  }
  const dialogRef = this.dialog.open(DeleteLocationDialogComponent, {
    data: row,
    direction: tempDirection,
  });
  this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
    if (result === 1) {
      const foundIndex = this.exampleDatabase?.dataChange.value.findIndex(
        (x) => x.locationId === this.id
      );
      // for delete we use splice in order to remove single object from DataService
      if (foundIndex !== undefined) {
        if (this.exampleDatabase) {
          this.exampleDatabase.dataChange.value.splice(foundIndex, 1);
        }
        this.refreshTable();
        this.showNotification(
          'snackbar-danger',
          'Delete Record Successfully...!!!',
          'bottom',
          'center'
        );
      }
    }
  });
}
private refreshTable() {
  this.paginator._changePageSize(this.paginator.pageSize);
}
/** Whether the number of selected elements matches the total number of rows. */
isAllSelected() {
  const numSelected = this.selection.selected.length;
  const numRows = this.dataSource?.renderedData.length;
  return numSelected === numRows;
}

/** Selects all rows if they are not all selected; otherwise clear selection. */
masterToggle() {
  this.isAllSelected()
    ? this.selection.clear()
    : this.dataSource?.renderedData.forEach((row) =>
        this.selection.select(row)
      );
}
removeSelectedRows() {
  const totalSelect = this.selection.selected.length;
  this.selection.selected.forEach((item) => {
    const index = this.dataSource?.renderedData.findIndex((d) => d === item);
    // console.log(this.dataSource.renderedData.findIndex((d) => d === item));
    if (index !== undefined) {
      if (this.exampleDatabase) {
        this.exampleDatabase.dataChange.value.splice(index, 1);
      }
      this.refreshTable();
      this.selection = new SelectionModel<LocationModel>(true, []);
    }
  });
  this.showNotification(
    'snackbar-danger',
    totalSelect + ' Record Delete Successfully...!!!',
    'bottom',
    'center'
  );
}
public loadData() {
  this.exampleDatabase = new LocationService(this.httpClient);
  this.dataSource = new ExampleDataSource(
    this.exampleDatabase,
    this.paginator,
    this.sort
  );
  this.subs.sink = fromEvent(this.filter.nativeElement, 'keyup').subscribe(
    () => {
      if (!this.dataSource) {
        return;
      }
      this.dataSource.filter = this.filter.nativeElement.value;
    }
  );
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
export class ExampleDataSource extends DataSource<LocationModel> {
filterChange = new BehaviorSubject('');
get filter(): string {
  return this.filterChange.value;
}
set filter(filter: string) {
  this.filterChange.next(filter);
}
filteredData: LocationModel[] = [];
renderedData: LocationModel[] = [];
constructor(
  public exampleDatabase: LocationService,
  public paginator: MatPaginator,
  public _sort: MatSort
) {
  super();
  // Reset to the first page when the user changes the filter.
  this.filterChange.subscribe(() => (this.paginator.pageIndex = 0));
}
/** Connect function called by the table to retrieve one stream containing the data to render. */
connect(): Observable<LocationModel[]> {
  // Listen for any changes in the base data, sorting, filtering, or pagination
  const displayDataChanges = [
    this.exampleDatabase.dataChange,
    this._sort.sortChange,
    this.filterChange,
    this.paginator.page,
  ];

  this.exampleDatabase.getAllLocation(1, 10);
  return merge(...displayDataChanges).pipe(
    map(() => {
      // Filter data
      this.filteredData = this.exampleDatabase.data
        .slice()
        .filter((locations: LocationModel) => {
          const searchStr = (
            locations.name +
            locations.country +
            locations.state
          ).toLowerCase();
          return searchStr.indexOf(this.filter.toLowerCase()) !== -1;
        });
      // Sort filtered data
      const sortedData = this.sortData(this.filteredData.slice());
      // Grab the page's slice of the filtered sorted data.
      const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
      this.renderedData = sortedData.splice(
        startIndex,
        this.paginator.pageSize
      );
      return this.renderedData;
    })
  );
}
// eslint-disable-next-line @typescript-eslint/no-empty-function
disconnect() {}
/** Returns a sorted copy of the database data. */
sortData(data: LocationModel[]): LocationModel[] {
  if (!this._sort.active || this._sort.direction === '') {
    return data;
  }
  return data.sort((a, b) => {
    let propertyA: number | string = '';
    let propertyB: number | string = '';
    switch (this._sort.active) {
      case 'locationId':
        [propertyA, propertyB] = [a.locationId, b.locationId];
        break;
      case 'name':
        [propertyA, propertyB] = [a.name, b.name];
        break;
      case 'country':
        [propertyA, propertyB] = [a.country, b.country];
        break;
      case 'state':
        [propertyA, propertyB] = [a.state, b.state];
        break;
      case 'city':
        [propertyA, propertyB] = [a.city, b.city];
        break;
      case 'address':
        [propertyA, propertyB] = [a.address, b.address];
        break;
        case 'zipCode':
          [propertyA, propertyB] = [a.zipCode, b.zipCode];
          break;
        case 'phone':
          [propertyA, propertyB] = [a.phone, b.phone];
          break;
    }
    const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
    const valueB = isNaN(+propertyB) ? propertyB : +propertyB;
    return (
      (valueA < valueB ? -1 : 1) * (this._sort.direction === 'asc' ? 1 : -1)
    );
  });
}
}
