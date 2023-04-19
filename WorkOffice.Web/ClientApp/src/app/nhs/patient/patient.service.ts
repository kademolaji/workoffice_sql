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
import { PatientModel } from './patient.model';
@Injectable()
export class PatientService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<PatientModel[]> = new BehaviorSubject<
    PatientModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: PatientModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): PatientModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllPatient(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<PatientModel[]>>(
        `api/patientinformation/GetList`, option
      );
  }

  getPatientById(id: number) {
    return this.httpClient.get<GetResponse<PatientModel>>(
      `api/patientinformation/Get?patientId=${id}`
    );
  }

  addPatient(data: PatientModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/patientinformation/Create`,
      data
    );
  }

  deletePatient(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/patientinformation/Delete?patientId=${id}`
    );
  }

  deleteMultiplePatient(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/patientinformation/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
