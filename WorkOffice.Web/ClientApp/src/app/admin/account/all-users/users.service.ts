import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { AddEditUserModel,  UpdateUserModel,  UserListModel } from './users.model';
import { CreateResponse, DeleteReply, GetResponse, SearchCall, SearchParameter, SearchReply } from 'src/app/core/utilities/api-response';

  @Injectable()
export class UsersService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<UserListModel[]> = new BehaviorSubject<
  UserListModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: AddEditUserModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): UserListModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllUser(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<UserListModel[]>>(
        `api/useraccount/all-users`, option
      );
  }

  getUserById(id: number) {
    return this.httpClient.get<GetResponse<UserListModel>>(
      `api/useraccount/${id}/user-details`
    );
  }

  addUser(data: AddEditUserModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/useraccount/register`,
      data
    );
  }

  updateUser(data: UpdateUserModel) {
    return this.httpClient.post<CreateResponse>(
      `api/useraccount/update-user`,
      data
    );
  }

  deleteUser(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/useraccount/Delete?userId=${id}`
    );
  }

  activateDeactivateUser(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/useraccount/${id}/disable-enable`
    );
  }


  deleteMultipleUser(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/useraccount/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
