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
import { WaitingTypeModel } from './waitingtype.model';
@Injectable()
export class WaitingTypeService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<WaitingTypeModel[]> = new BehaviorSubject<
    WaitingTypeModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: WaitingTypeModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): WaitingTypeModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllWaitingType(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<WaitingTypeModel[]>>(
        `api/WaitingType/GetList`, option
      );
  }

  getWaitingTypeById(id: number) {
    return this.httpClient.get<GetResponse<WaitingTypeModel>>(
      `api/WaitingType/Get?waitingTypeId=${id}`
    );
  }

  addWaitingType(data: WaitingTypeModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/WaitingType/Create`,
      data
    );
  }
  updateWaitingType(data: WaitingTypeModel): void {
    this.dialogData = data;
  }
  deleteWaitingType(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/WaitingType/Delete?waitingTypeId=${id}`
    );
  }
  deleteMultipleWaitingType(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/WaitingType/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
