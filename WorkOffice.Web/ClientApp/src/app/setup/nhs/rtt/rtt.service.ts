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
import { RTTModel } from './rtt.model';
@Injectable()
export class RTTService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<RTTModel[]> = new BehaviorSubject<
    RTTModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: RTTModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): RTTModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllRTT(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<RTTModel[]>>(
        `api/RTT/GetList`, option
      );
  }

  getRTTById(id: number) {
    return this.httpClient.get<GetResponse<RTTModel>>(
      `api/RTT/Get?rttId=${id}`
    );
  }

  addRTT(data: RTTModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/RTT/Create`,
      data
    );
  }
  updateRTT(data: RTTModel): void {
    this.dialogData = data;
  }
  deleteRTT(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/RTT/Delete?rttId=${id}`
    );
  }
  deleteMultipleRTT(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/RTT/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
