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
        `api/appointments/GetList`, option
      );
  }

  getAppointmentById(id: number) {
    return this.httpClient.get<GetResponse<AppointmentResponseModel>>(
      `api/appointments/Get?appointmentId=${id}`
    );
  }

  addAppointment(data: CreateAppointmentModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/appointments/Create`,
      data
    );
  }

  deleteAppointment(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/appointments/Delete?appointmentId=${id}`
    );
  }

  deleteMultipleAppointment(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/appointments/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
