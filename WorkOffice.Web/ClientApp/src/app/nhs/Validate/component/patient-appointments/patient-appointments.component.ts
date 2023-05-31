import { SelectionModel } from '@angular/cdk/collections';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SearchCall, SearchParameter } from 'src/app/core/utilities/api-response';
import { AppointmentResponseModel } from 'src/app/nhs/appointment/appointment.model';
import { AppointmentService } from 'src/app/nhs/appointment/appointment.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';

@Component({
  selector: 'app-patient-appointments',
  templateUrl: './patient-appointments.component.html',
  styleUrls: ['./patient-appointments.component.css']
})
export class PatientAppointmentsComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
displayedColumns: string[] = [
  'patientName',
  'patientNumber',
  'patientPathNumber',
  'appointmentDate',
  'bookingDate',
  'speciality',
  'appointmentStatus',
];
selection = new SelectionModel<AppointmentResponseModel>(true, []);
ELEMENT_DATA: AppointmentService[] = [];
isLoading = false;
totalRows = 0;
pageSize = 10;
currentPage = 0;
pageSizeOptions: number[] = [5, 10, 25, 100];
dataSource: MatTableDataSource<AppointmentResponseModel> =
  new MatTableDataSource();
searchQuery = '';

isTblLoading = false;
status = 'PartialBooked';
@Input() patientId = 0;
constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public appointmentService: AppointmentService,

) {
  super();
}
@ViewChild(MatPaginator, { static: true })
paginator!: MatPaginator;
@ViewChild(MatSort, { static: true })
sort!: MatSort;

ngOnInit() {
  this.searchQuery =  this.status;
  this.loadData(this.searchQuery, this.patientId);
}

pageChanged(event: PageEvent) {
  this.pageSize = event.pageSize;
  this.currentPage = event.pageIndex;
  this.loadData(this.searchQuery,this.patientId);
}

public loadData(searchQuery: string, patientId: number) {
  this.isTblLoading = true;
  const options: SearchCall<SearchParameter> = {
    from: this.currentPage,
    pageSize: this.pageSize,
    sortField: "",
    sortOrder: "",
    parameter: {
      searchQuery,
      id: patientId
    },
  };

    this.appointmentService
    .getAllAppointment(options)
    .subscribe((res) => {
      this.isTblLoading = false;
      this.dataSource.data = res.result;
      this.totalRows = res.totalCount;
    });
}
}

