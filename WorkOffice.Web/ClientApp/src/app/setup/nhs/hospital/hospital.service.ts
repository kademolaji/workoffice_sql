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
import { HospitalModel } from './hospital.model';
@Injectable()
export class HospitalService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<HospitalModel[]> = new BehaviorSubject<
    HospitalModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: HospitalModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): HospitalModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllHospital(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<HospitalModel[]>>(
        `api/hospital/GetList`, option
      );
  }

  getHospitalById(id: number) {
    return this.httpClient.get<GetResponse<HospitalModel>>(
      `api/hospital/Get?hospitalId=${id}`
    );
  }

  addHospital(data: HospitalModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/hospital/Create`,
      data
    );
  }
  deleteHospital(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/hospital/Delete?hospitalId=${id}`
    );
  }
  deleteMultipleHospital(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/hospital/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
