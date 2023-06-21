export interface AddEditUserModel {
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
  email: string;
  country: string;
  userRoleId: number;
  acceptTerms: boolean;
  phoneNumber: string;
  accesslevel: number;
  securityQuestion: string;
  securityAnswer: string;
  userAccessIds: number[];
  userRoleIds: number[];
  lastLogin: string;
}

export interface UserListModel {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  country: string;
  biography: string;
  userRole: string;
  status: string;
  profilePicture: string;
  phoneNumber: string;
  securityQuestion: string;
  securityAnswer: string;
  userRoleId: number[];
  lastLogin: string;
}

export interface SearchUserListOptions {
  from: number;
  pageSize: number;
  parameter: SearchUserList;
}

export interface SearchUserList {
  searchQuery: string;
  userRole: string;
}

export interface UpdateUserModel {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  country: string;
  phoneNumber: string;
  securityQuestion: string;
  securityAnswer: string;
  userRoleIds: number[];
  lastLogin: string;
}
