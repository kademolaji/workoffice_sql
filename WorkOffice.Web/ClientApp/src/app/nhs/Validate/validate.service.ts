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
import { MergePathwayModel, PatientValidationDetailsModel } from './validate.model';
@Injectable()
export class ValidateNowService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<PatientValidationDetailsModel[]> = new BehaviorSubject<
  PatientValidationDetailsModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: PatientValidationDetailsModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): PatientValidationDetailsModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllPatientValidationDetails(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<PatientValidationDetailsModel[]>>(
        `api/PatientValidationDetails/GetList`, option
      );
  }

  getPatientValidationDetailsById(id: number) {
    return this.httpClient.get<GetResponse<PatientValidationDetailsModel>>(
      `api/PatientValidationDetails/Get?patientId=${id}`
    );
  }

  addPatientValidationDetails(data: PatientValidationDetailsModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/PatientValidationDetails/Create`,
      data
    );
  }

  mergePatientValidationDetails(data: MergePathwayModel) {
    return this.httpClient.post<CreateResponse>(
      `api/PatientValidationDetails/Merge`,
      data
    );
  }

  deletePatientValidationDetails(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/PatientValidationDetails/Delete?patientValidationDetailsId=${id}`
    );
  }



}
