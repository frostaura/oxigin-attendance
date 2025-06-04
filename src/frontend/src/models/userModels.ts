// userModels.ts
// Interfaces for user-related data structures in the Oxigin Attendance frontend.

import type { UserType } from "../enums/userTypes";


/**
 * User entity interface representing a user in the system.
 */
export interface User {
    id: string;
    name: string;
    contactNr: string;
    email: string;
    userType: UserType;
    password: string;
    clientID?: string | null; // Optional client ID for users associated with a client
    employeeID?: string | null; // Optional employee ID for users who are employees
    //sessions?: UserSession[];
}

/**
 * Response type for user sign-in, containing the user object and session ID.
 */
export interface UserSigninResponse{
    user: User;
    sessionId: string;
}

export interface UserSignUpValues {
    name: string;
    contactNr: string;
    email: string;
    password: string;
    userType: UserType;
}

export interface UserSignUpResponse{
    user: User;
    sessionId: string;
}