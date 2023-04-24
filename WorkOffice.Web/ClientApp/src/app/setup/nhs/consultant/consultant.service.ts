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
import { ConsultantModel } from './consultant.model';
@Injectable()
export class ConsultantService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<ConsultantModel[]> = new BehaviorSubject<
    ConsultantModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: ConsultantModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): ConsultantModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllConsultant(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<ConsultantModel[]>>(
        `api/consultant/GetList`, option
      );
  }

  getConsultantById(id: number) {
    return this.httpClient.get<GetResponse<ConsultantModel>>(
      `api/consultant/Get?consultantId=${id}`
    );
  }

  addConsultant(data: ConsultantModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/consultant/Create`,
      data
    );
  }

  deleteConsultant(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/consultant/Delete?consultantId=${id}`
    );
  }
  deleteMultipleConsultant(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/consultant/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
