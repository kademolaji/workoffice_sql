import { Role } from "../role";

export interface RegisterUser {
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
  email: string;
  country: string;
  userRole: Role;
  acceptTerms: boolean;
}
