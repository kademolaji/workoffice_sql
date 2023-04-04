import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import {
  CreateResponse,
  DeleteReply,
  GetResponse,
  SearchCall,
  SearchParameter,
  SearchReply,
} from 'src/app/core/utilities/api-response';
import { AppTypeModel } from './apptype.model';
@Injectable()
export class AppTypeService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<AppTypeModel[]> = new BehaviorSubject<
    AppTypeModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: AppTypeModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): AppTypeModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllAppType(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<AppTypeModel[]>>(
        `api/AppType/GetList`, option
      );
  }

  getAppTypeById(id: number) {
    return this.httpClient.get<GetResponse<AppTypeModel>>(
      `api/AppType/Get?appTypeId=${id}`
    );
  }

  addAppType(data: AppTypeModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/AppType/Create`,
      data
    );
  }
  updateAppType(data: AppTypeModel): void {
    this.dialogData = data;
  }
  deleteAppType(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/AppType/Delete?appTypeId=${id}`
    );
  }
  deleteMultipleAppType(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/AppType/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
