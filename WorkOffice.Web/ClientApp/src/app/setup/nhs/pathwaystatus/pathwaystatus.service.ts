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
import { PathwayStatusModel } from './pathwaystatus.model';
@Injectable()
export class PathwayStatusService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<PathwayStatusModel[]> = new BehaviorSubject<
    PathwayStatusModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: PathwayStatusModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): PathwayStatusModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllPathwayStatus(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<PathwayStatusModel[]>>(
        `api/pathwayStatus/GetList`, option
      );
  }

  getPathwayStatusById(id: number) {
    return this.httpClient.get<GetResponse<PathwayStatusModel>>(
      `api/pathwayStatus/Get?appTypeId=${id}`
    );
  }

  addPathwayStatus(data: PathwayStatusModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/pathwayStatus/Create`,
      data
    );
  }

  deletePathwayStatus(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/pathwayStatus/Delete?appTypeId=${id}`
    );
  }
  deleteMultiplePathwayStatus(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/pathwayStatus/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
