import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { SelectionModel } from '@angular/cdk/collections';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { Direction } from '@angular/cdk/bidi';
import { UsersService } from './users.service';
import { UserListModel } from './users.model';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import {
  SearchCall,
  SearchParameter,
} from 'src/app/core/utilities/api-response';
import { DeleteDialogComponent } from './dialog/delete/delete.component';
@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.sass'],
})
export class AllUsersComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  displayedColumns = [
    'select',
    'profilePicture',
    'firstName',
    'lastName',
    'email',
    'country',
    'userRole',
    'actions',
  ];
  selection = new SelectionModel<UserListModel>(true, []);
  ELEMENT_DATA: UserListModel[] = [];
  isLoading = false;
  totalRows = 0;
  pageSize = 10;
  currentPage = 0;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  dataSource: MatTableDataSource<UserListModel> = new MatTableDataSource();
  searchQuery = '';
  sortOrder = '';
  sortField = '';
  isTblLoading = false;

  constructor(
    public httpClient: HttpClient,
    public dialog: MatDialog,
    public usersService: UsersService,
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
    this.currentPage = 0;
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
    this.router.navigate(['admin', 'users', 'add-user']);
  }

  editCall(row: { userId: number }) {
    this.router.navigate(['admin', 'users', 'edit-user', row.userId]);
  }

  activateDeactivateUser(row: UserListModel) {
    let tempDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      tempDirection = 'rtl';
    } else {
      tempDirection = 'ltr';
    }
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: row,
      direction: tempDirection,
    });
    this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
      this.refresh();
      this.showNotification(
        'snackbar-success',
        'Record Updated Successfully...!!!',
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
    const targetIds = this.selection.selected.map((data) => data.userId);
    this.subs.sink = this.usersService.deleteMultipleUser(targetIds).subscribe({
      next: (res) => {
        if (res.status) {
          this.refresh();
          this.selection = new SelectionModel<UserListModel>(true, []);
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
    this.usersService.getAllUser(options).subscribe((res) => {
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
