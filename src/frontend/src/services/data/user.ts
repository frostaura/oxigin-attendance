// user.ts
// Service functions and types for user authentication and user-related API calls.
// Handles user sign-in and session management for the Oxigin Attendance frontend.

import type { UserSignUpResponse, UserSigninResponse } from "../../models/userModels";
import { PostAsync } from "./backend";
//import { UserSigninResponse, UserSession } from "../models/userModels";

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
    localStorage.setItem("session", JSON.stringify(response));

    return response;
}

export async function SignUpAsync(name: string, contactNr: string, email: string, password: string): Promise<UserSignUpResponse> {
    const response = await PostAsync<UserSignUpResponse>('User/SignUp', { name, contactNr, email, password });
    localStorage.setItem("session", JSON.stringify(response));

    return response;
}


