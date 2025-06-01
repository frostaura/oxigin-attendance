import { PostAsync } from "./backend";

export interface User {
    id: string;
    name: string;
    contactNr: string;
    email: string;
    //userType: UserType;
    password: string;
    //sessions?: UserSession[];
}

export interface UserSigninResponse{
    user: User;
    sessionId: string;
}

export function SignInAsync(email: string, password: string): Promise<UserSigninResponse>{
    return PostAsync<UserSigninResponse>('User/SignIn', { email, password });
} 