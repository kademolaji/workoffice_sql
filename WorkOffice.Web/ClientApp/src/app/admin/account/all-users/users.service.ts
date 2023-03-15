import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { AddEditUserModel, SearchUserListOptions, UserListModel } from './users.model';
import { SearchReply } from 'src/app/core/utilities/api-response';
@Injectable()
export class UsersService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<UserListModel[]> = new BehaviorSubject<UserListModel[]>([]);
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
  /** CRUD METHODS */

  getAllUsers(options: SearchUserListOptions): void {
    this.subs.sink = this.httpClient.post<SearchReply<UserListModel[]>>(`api/useraccount/all-users`, options).subscribe({
      next: (data) => {
        this.isTblLoading = false;
        this.dataChange.next(data.result);
      },
      error: (error: HttpErrorResponse) => {
        this.isTblLoading = false;
        console.log(error.name + ' ' + error.message);
      },
    });
  }

  addUser(user: AddEditUserModel) {
    this.dialogData = user;
    return this.httpClient.post<any>(`api/useraccount/register`, user);

  }
  updateUser(user: AddEditUserModel): void {
    this.dialogData = user;

    // this.httpClient.put(this.API_URL + staff.id, staff)
    //     .subscribe({
    //       next: (data) => {
    //         this.dialogData = staff;
    //       },
    //       error: (error: HttpErrorResponse) => {
    //          // error code here
    //       },
    //     });
  }
  deleteUser(id: number): void {
    console.log(id);

    // this.httpClient.delete(this.API_URL + id)
    //     .subscribe({
    //       next: (data) => {
    //         console.log(id);
    //       },
    //       error: (error: HttpErrorResponse) => {
    //          // error code here
    //       },
    //     });
  }
}
