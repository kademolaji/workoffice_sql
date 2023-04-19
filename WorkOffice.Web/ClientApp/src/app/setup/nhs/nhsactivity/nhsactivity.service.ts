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
import { NHSActivityModel } from './nhsactivity.model';
@Injectable()
export class NHSActivityService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<NHSActivityModel[]> = new BehaviorSubject<
    NHSActivityModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: NHSActivityModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): NHSActivityModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllNHSActivity(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<NHSActivityModel[]>>(
        `api/Activity/GetList`, option
      );
  }

  getNHSActivityById(id: number) {
    return this.httpClient.get<GetResponse<NHSActivityModel>>(
      `api/Activity/Get?appTypeId=${id}`
    );
  }

  addNHSActivity(data: NHSActivityModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/Activity/Create`,
      data
    );
  }
  updateNHSActivity(data: NHSActivityModel): void {
    this.dialogData = data;
  }
  deleteNHSActivity(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/Activity/Delete?appTypeId=${id}`
    );
  }
  deleteMultipleNHSActivity(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/Activity/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
