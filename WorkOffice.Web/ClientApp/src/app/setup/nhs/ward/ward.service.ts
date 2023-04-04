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
import { WardModel } from './ward.model';
@Injectable()
export class WardService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<WardModel[]> = new BehaviorSubject<
    WardModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: WardModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): WardModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllWard(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<WardModel[]>>(
        `api/Ward/GetList`, option
      );
  }

  getWardById(id: number) {
    return this.httpClient.get<GetResponse<WardModel>>(
      `api/Ward/Get?wardId=${id}`
    );
  }

  addWard(data: WardModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/Ward/Create`,
      data
    );
  }
  updateWard(data: WardModel): void {
    this.dialogData = data;
  }
  deleteWard(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/Ward/Delete?wardId=${id}`
    );
  }
  deleteMultipleWard(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/Ward/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
