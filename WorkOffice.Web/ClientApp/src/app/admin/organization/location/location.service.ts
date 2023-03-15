import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { CreateResponse, GetResponse, } from 'src/app/core/utilities/api-response';
import { LocationModel } from './location.model';
@Injectable()
export class LocationService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<LocationModel[]> = new BehaviorSubject<LocationModel[]>([]);
  // Temporarily stores data from dialogs
  dialogData!: LocationModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): LocationModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }
  /** CRUD METHODS */

  getAllLocation(pageNumber: number, pageSize: number): void {
    this.subs.sink = this.httpClient.get<GetResponse<LocationModel[]>>(`api/location/GetList?pageNumber=${pageNumber}&pageSize=${pageSize}`).subscribe({
      next: (data) => {
        this.isTblLoading = false;
        this.dataChange.next(data.entity);
      },
      error: (error: HttpErrorResponse) => {
        this.isTblLoading = false;
        console.log(error.name + ' ' + error.message);
      },
    });
  }

  addLocation(location: LocationModel) {
    this.dialogData = location;
    return this.httpClient.post<CreateResponse>(`api/location/Create`, location);

  }
  updateLocation(user: LocationModel): void {
    this.dialogData = user;

    // this.httpClient.put(this.API_URL + staff.id, staff)
    //     .subscribe({
    //       next: (data) => {
    //         this.dialogData = staff;
    //       },
    //       error: (error: HttpErrorResponse) => {
    //          // error code here
    //       },
    //     });
  }
  deleteLocation(id: number): void {
    console.log(id);

    // this.httpClient.delete(this.API_URL + id)
    //     .subscribe({
    //       next: (data) => {
    //         console.log(id);
    //       },
    //       error: (error: HttpErrorResponse) => {
    //          // error code here
    //       },
    //     });
  }
}
