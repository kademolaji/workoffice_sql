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
import { PathwayResponseModel, CreatePathwayModel } from './pathway.model';
@Injectable()
export class PathwayService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<PathwayResponseModel[]> = new BehaviorSubject<
  PathwayResponseModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: CreatePathwayModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): PathwayResponseModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllPathway(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<PathwayResponseModel[]>>(
        `api/patientValidation/GetList`, option
      );
  }

  getPathwayById(id: number) {
    return this.httpClient.get<GetResponse<PathwayResponseModel>>(
      `api/patientValidation/Get?patientValidationId=${id}`
    );
  }

  addPathway(data: CreatePathwayModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/patientValidation/Create`,
      data
    );
  }

  deletePathway(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/patientValidation/Delete?patientValidationId=${id}`
    );
  }

  deleteMultiplePathway(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/patientValidation/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
