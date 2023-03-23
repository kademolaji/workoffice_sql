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
import { CustomIdentityFormatModel } from './custom-identity-format.model';

@Injectable()
export class CustomIdentityFormatService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<CustomIdentityFormatModel[]> = new BehaviorSubject<
  CustomIdentityFormatModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: CustomIdentityFormatModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): CustomIdentityFormatModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllCustomIdentityFormat(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<CustomIdentityFormatModel[]>>(
        `api/CustomIdentityFormatSetting/GetList`, option
      );
  }

  getCustomIdentityFormatById(id: number) {
    return this.httpClient.get<GetResponse<CustomIdentityFormatModel>>(
      `api/CustomIdentityFormatSetting/Get?customIdentityFormatSettingId=${id}`
    );
  }

  addCustomIdentityFormat(data: CustomIdentityFormatModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/CustomIdentityFormatSetting/Create`,
      data
    );
  }

  deleteCustomIdentityFormat(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/CustomIdentityFormatSetting/Delete?customIdentityFormatSettingId=${id}`
    );
  }

  deleteMultipleCustomIdentityFormat(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/CustomIdentityFormatSetting/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
