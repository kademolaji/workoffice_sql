import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import {
  CreateResponse,
  DeleteReply,
  GetResponse,
} from 'src/app/core/utilities/api-response';
import { UserRoleActivitiesModel, UserRoleAndActivityModel } from './user-role.model';

@Injectable()
export class UserRoleService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<UserRoleAndActivityModel[]> = new BehaviorSubject<
  UserRoleAndActivityModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: UserRoleAndActivityModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): UserRoleAndActivityModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllUserRole() {
   return this.httpClient
      .get<GetResponse<UserRoleAndActivityModel[]>>(
        `api/Administration/GetAllUserRoleDefinitions`
      );
  }

  getUserRoleById(id: number) {
    return this.httpClient.get<GetResponse<UserRoleAndActivityModel>>(
      `api/Administration/GetUserRoleDefinition?userRoleId=${id}`
    );
  }

  getUserRoleAndActivities(userRoleId: number) {
    return this.httpClient
       .get<GetResponse<UserRoleActivitiesModel[]>>(
         `api/Administration/GetUserRoleAndActivities?userRoleId=${userRoleId}`
       );
   }


  addUserRole(data: UserRoleAndActivityModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/Administration/CreateUserRoleAndActivity`,
      data
    );
  }

  deleteUserRole(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/Administration/DeleteUserRoleDefinition?userRoleDefinitionId=${id}`
    );
  }

  deleteMultipleUserRole(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/Administration/DeleteMultipleUserRoleDefinition`, {targetIds: targetIds}
    );
  }
}
