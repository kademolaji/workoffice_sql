import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { CreateResponse, DeleteReply, GetResponse, SearchCall, SearchParameter, SearchReply, } from 'src/app/core/utilities/api-response';
import { LocationModel } from './location.model';
@Injectable()
export class LocationService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<LocationModel[]> = new BehaviorSubject<
    LocationModel[]
  >([]);
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

  getAllLocation(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<LocationModel[]>>(
        `api/location/GetList`, option
      );
  }

  getLocationById(id: number) {
    return this.httpClient.get<GetResponse<LocationModel>>(
      `api/location/Get?locationId=${id}`
    );
  }

  addLocation(data: LocationModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/location/Create`,
      data
    );
  }

  deleteLocation(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/location/Delete?locationId=${id}`
    );
  }

  deleteMultipleLocation(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/location/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
