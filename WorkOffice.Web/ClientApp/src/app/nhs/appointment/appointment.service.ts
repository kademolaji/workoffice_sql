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
import { AppointmentResponseModel, CreateAppointmentModel } from './appointment.model';
@Injectable()
export class AppointmentService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<AppointmentResponseModel[]> = new BehaviorSubject<
  AppointmentResponseModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: CreateAppointmentModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): AppointmentResponseModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllAppointment(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<AppointmentResponseModel[]>>(
        `api/appointment/GetList`, option
      );
  }

  getAppointmentById(id: number) {
    return this.httpClient.get<GetResponse<AppointmentResponseModel>>(
      `api/appointment/Get?appointmentId=${id}`
    );
  }

  addAppointment(data: CreateAppointmentModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/appointment/Create`,
      data
    );
  }

  deleteAppointment(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/appointment/Delete?appointmentId=${id}`
    );
  }

  deleteMultipleAppointment(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/appointment/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
