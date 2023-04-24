import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import {
  CreateResponse,
  DeleteReply,
  GetResponse,
  SearchCall,
  SearchParameter,
  SearchReply,
} from 'src/app/core/utilities/api-response';
import { WaitinglistModel } from './watinglist.model';
@Injectable()
export class WaitinglistService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<WaitinglistModel[]> = new BehaviorSubject<
    WaitinglistModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: WaitinglistModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): WaitinglistModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllWaitinglist(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<WaitinglistModel[]>>(
        `api/waitingList/GetList`, option
      );
  }

  getWaitinglistById(id: number) {
    return this.httpClient.get<GetResponse<WaitinglistModel>>(
      `api/waitinglist/Get?waitinglistId=${id}`
    );
  }

  addWaitinglist(data: WaitinglistModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/waitinglist/Create`,
      data
    );
  }

  deleteWaitinglist(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/waitingList/Delete?waitinglistId=${id}`
    );
  }

  deleteMultipleWaitinglist(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/waitingList/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
