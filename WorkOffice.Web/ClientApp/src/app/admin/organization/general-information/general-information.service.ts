import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { CreateResponse, DeleteReply, GetResponse, SearchCall, SearchParameter, SearchReply, } from 'src/app/core/utilities/api-response';
import { GeneralInformationModel } from './general-information.model';
@Injectable()
export class GeneralInformationService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<GeneralInformationModel[]> = new BehaviorSubject<
    GeneralInformationModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: GeneralInformationModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): GeneralInformationModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllGeneralInformation(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<GeneralInformationModel[]>>(
        `api/generalinformation/GetList`, option
      );
  }

  getGeneralInformationById(id: number) {
    return this.httpClient.get<GetResponse<GeneralInformationModel>>(
      `api/generalinformation/Get?generalinformationId=${id}`
    );
  }

  addGeneralInformation(data: GeneralInformationModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/generalinformation/Create`,
      data
    );
  }

  deleteGeneralInformation(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/generalinformation/Delete?generalInformationId=${id}`
    );
  }

  deleteMultipleGeneralInformation(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/GeneralInformation/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
