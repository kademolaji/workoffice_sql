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
import { SearchCall, SearchParameter } from 'src/app/core/utilities/api-response';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { GeneralInformationModel } from '../general-information.model';
import { GeneralInformationService } from '../general-information.service';
import { DeleteGeneralInformationDialogComponent } from './dialog/delete/delete.component';

@Component({
  selector: 'app-all-general-information',
  templateUrl: './all-general-information.component.html',
  styleUrls: ['./all-general-information.component.css']
})
export class AllGeneralInformationComponent extends UnsubscribeOnDestroyAdapter
implements OnInit
{
displayedColumns = [
  'select',
  'organisationName',
  'regno',
  'email',
  'phone',
  'country',
  'actions',
];
selection = new SelectionModel<GeneralInformationModel>(true, []);
  ELEMENT_DATA: GeneralInformationService[] = [];
  isLoading = false;
  totalRows = 0;
  pageSize = 10;
  currentPage = 0;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  dataSource: MatTableDataSource<GeneralInformationModel> = new MatTableDataSource();
  searchQuery = '';
  sortOrder = '';
  sortField = '';
  isTblLoading = false;

  constructor(
    public httpClient: HttpClient,
    public dialog: MatDialog,
    public generalInformationService: GeneralInformationService,
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

  addNew() {
    this.router.navigate(['admin', 'company', 'add-general-information']);
  }

  editCall(row: { generalInformationId: number }) {
    this.router.navigate([
      'admin',
      'company',
      'edit-general-information',
      row.generalInformationId,
    ]);
  }

  deleteItem(row: GeneralInformationModel) {
    let tempDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      tempDirection = 'rtl';
    } else {
      tempDirection = 'ltr';
    }
    const dialogRef = this.dialog.open(
      DeleteGeneralInformationDialogComponent,
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
      (data) => data.generalInformationId
    );
    this.subs.sink = this.generalInformationService
      .deleteMultipleGeneralInformation(targetIds)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.refresh();
            this.selection = new SelectionModel<GeneralInformationModel>(
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
    this.generalInformationService
      .getAllGeneralInformation(options)
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
