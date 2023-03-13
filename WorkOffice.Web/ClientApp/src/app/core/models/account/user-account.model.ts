import { Role } from "../role";

export interface UserAccount {
 userId: number;
 firstName: string;
 lastName: string;
 password: string;
 email: string;
 country: string;
 profilePicture: string;
 token: string;
 userRole: Role;
 biography: string;
 fullName: string;
}
