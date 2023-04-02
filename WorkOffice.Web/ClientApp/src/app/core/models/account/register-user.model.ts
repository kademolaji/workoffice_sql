
export interface RegisterUser {
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
  email: string;
  country: string;
  userRoleId: number;
  acceptTerms: boolean;
}
