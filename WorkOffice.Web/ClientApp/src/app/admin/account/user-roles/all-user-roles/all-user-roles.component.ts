import { Direction } from '@angular/cdk/bidi';
import { SelectionModel } from '@angular/cdk/collections';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { UserRoleActivitiesModel, UserRoleAndActivityModel } from '../user-role.model';
import { UserRoleService } from '../user-role.service';
import { DeleteUserRoleDialogComponent } from './dialog/delete/delete.component';

@Component({
  selector: 'app-all-user-roles',
  templateUrl: './all-user-roles.component.html',
  styleUrls: ['./all-user-roles.component.css']
})
export class AllUserRolesComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
displayedColumns: string[] = [
  'select',
  'userRoleDefinition',
  'parentActivities',
  'activities',
  'actions',
];
selection = new SelectionModel<UserRoleAndActivityModel>(true, []);
ELEMENT_DATA: UserRoleService[] = [];
isLoading = false;
totalRows = 0;
pageSize = 10;
currentPage = 0;
pageSizeOptions: number[] = [5, 10, 25, 100];
dataSource: MatTableDataSource<UserRoleAndActivityModel> =
  new MatTableDataSource();
searchQuery = '';
sortOrder = '';
sortField = '';
isTblLoading = false;

constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public userRoleService: UserRoleService,
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
  this.loadData();
}

applyFilter(event: Event) {
  const filterValue = (event.target as HTMLInputElement).value;
  this.searchQuery = filterValue.trim();
  // this.loadData(this.searchQuery, this.sortField, this.sortOrder);
}

sortData(sort: Sort) {
  this.sortField = sort.active;
  this.sortOrder = sort.direction;
  // this.loadData(this.searchQuery, this.sortField, this.sortOrder);
}

pageChanged(event: PageEvent) {
  this.pageSize = event.pageSize;
  this.currentPage = event.pageIndex;
  // this.loadData(this.searchQuery, this.sortField, this.sortOrder);
}

refresh() {
  this.searchQuery = '';
  this.sortOrder = '';
  this.sortField = '';
  this.loadData();
}

addNew() {
  this.router.navigate(['admin', 'users', 'add-user-role']);
}

editCall(row: { userRoleAndActivityId: number }) {
  this.router.navigate([
    'admin',
    'users',
    'edit-user-role',
    row.userRoleAndActivityId,
  ]);
}

deleteItem(row: UserRoleAndActivityModel) {
  let tempDirection: Direction;
  if (localStorage.getItem('isRtl') === 'true') {
    tempDirection = 'rtl';
  } else {
    tempDirection = 'ltr';
  }
  const dialogRef = this.dialog.open(
    DeleteUserRoleDialogComponent,
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
    (data) => data.userRoleAndActivityId
  );
  this.subs.sink = this.userRoleService
    .deleteMultipleUserRole(targetIds)
    .subscribe({
      next: (res) => {
        if (res.status) {
          this.refresh();
          this.selection = new SelectionModel<UserRoleAndActivityModel>(
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
public loadData() {
  this.isTblLoading = true;

  this.userRoleService
    .getAllUserRole()
    .subscribe((res) => {
      this.isTblLoading = false;
      this.dataSource.data = res.entity;
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

getParentActivities(activites: UserRoleActivitiesModel[]){

  return  activites.map(x=>x.userActivityParentName).filter((value, index, array) => array.indexOf(value) === index);
}

getActivities(activites: UserRoleActivitiesModel[]){
  return activites.map(x=>x.userActivityName).join(", ")
}

}
