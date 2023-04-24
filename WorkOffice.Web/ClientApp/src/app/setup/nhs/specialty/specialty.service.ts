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
import { SpecialtyModel } from './specialty.model';
@Injectable()
export class SpecialtyService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<SpecialtyModel[]> = new BehaviorSubject<
    SpecialtyModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: SpecialtyModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): SpecialtyModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllSpecialty(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<SpecialtyModel[]>>(
        `api/specialty/GetList`, option
      );
  }

  getSpecialtyById(id: number) {
    return this.httpClient.get<GetResponse<SpecialtyModel>>(
      `api/specialty/Get?specialtyId=${id}`
    );
  }

  addSpecialty(data: SpecialtyModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/specialty/Create`,
      data
    );
  }
  deleteSpecialty(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/specialty/Delete?specialtyId=${id}`
    );
  }
  deleteMultipleSpecialty(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/specialty/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
