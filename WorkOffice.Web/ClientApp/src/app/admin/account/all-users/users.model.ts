import { Role } from 'src/app/core/models/role';

export interface AddEditUserModel {
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
  email: string;
  country: string;
  userRole: Role;
  acceptTerms: boolean;
}

export interface UserListModel {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  country: string;
  biography: string;
  userRole: string;
  status: string;
  profilePicture: string;
}

export interface SearchUserListOptions {
  from: number;
  pageSize: number;
  parameter: SearchUserList;
}

export interface SearchUserList {
  searchQuery: string;
  userRole: Role;
}
