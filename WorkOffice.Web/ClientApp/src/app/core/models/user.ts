import { Role } from './role';

export class User {
  userId!: string;
  firstName!: string;
  lastName!: string;
  email!: string;
  profilePicture!: string;
  token!: string;
  userRole!: Role;
  isVerified!: boolean;
  fullName!: string
}
