// user.ts
// Service functions and types for user authentication and user-related API calls.
// Handles user sign-in and session management for the Oxigin Attendance frontend.

import { PostAsync } from "./backend";

/**
 * User entity interface representing a user in the system.
 */
export interface User {
    id: string;
    name: string;
    contactNr: string;
    email: string;
    //userType: UserType;
    password: string;
    //sessions?: UserSession[];
}

/**
 * Response type for user sign-in, containing the user object and session ID.
 */
export interface UserSigninResponse{
    user: User;
    sessionId: string;
}

/**
 * Sign in a user with the provided email and password.
 * Stores the session ID in localStorage upon successful authentication.
 * @param {string} email - The user's email address.
 * @param {string} password - The user's password.
 * @returns {Promise<UserSigninResponse>} The user and session ID from the backend.
 */
export async function SignInAsync(email: string, password: string): Promise<UserSigninResponse>{
    const response = await PostAsync<UserSigninResponse>('User/SignIn', { email, password });

    // Add the session id to localstorage.
    localStorage.setItem("sessionId", response.sessionId);

    return response;
}