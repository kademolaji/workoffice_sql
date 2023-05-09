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
import { ReferralModel } from './referral.model';
@Injectable()
export class ReferralService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<ReferralModel[]> = new BehaviorSubject<
  ReferralModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: ReferralModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): ReferralModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllReferral(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<ReferralModel[]>>(
        `api/referral/GetList`, option
      );
  }

  getReferralById(id: number) {
    return this.httpClient.get<GetResponse<ReferralModel>>(
      `api/referral/Get?referralId=${id}`
    );
  }

  addReferral(formData: FormData) {
    return this.httpClient.post<CreateResponse>('/api/referral/create', formData);
  }

  deleteReferral(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/referral/Delete?referralId=${id}`
    );
  }

  deleteMultipleReferral(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/referral/MultipleDelete`, {targetIds: targetIds}
    );
  }


    downloadDocument(id: number) {
      return this.httpClient.get(
        `api/referral/Download?referralId=${id}`
        , {
          reportProgress: true,
          observe: 'events',
          responseType: 'blob'
        });
    }
}
