// user.ts
// Service functions and types for user authentication and user-related API calls.
// Handles user sign-in and session management for the Oxigin Attendance frontend.

import type { UserSignUpResponse, UserSigninResponse } from "../../models/userModels";
import { PostAsync } from "./backend";

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

/**
 * Registers a new user and stores the session in localStorage.
 * @param {string} name - The user's name.
 * @param {string} contactNr - The user's contact number.
 * @param {string} email - The user's email address.
 * @param {string} password - The user's password.
 * @returns {Promise<UserSignUpResponse>} The user and session ID from the backend.
 */
export async function SignUpAsync(name: string, contactNr: string, email: string, password: string): Promise<UserSignUpResponse> {
    const response = await PostAsync<UserSignUpResponse>('User/SignUp', { name, contactNr, email, password });
    localStorage.setItem("session", JSON.stringify(response));

    return response;
}

/**
 * Refreshes the current user's session by calling the backend and updating localStorage.
 * @returns {Promise<UserSigninResponse>} The refreshed user and session ID from the backend.
 */
export async function RefreshSessionAsync(): Promise<UserSigninResponse> {
    const response = await PostAsync<UserSigninResponse>('User/RefreshSession', {});
    localStorage.setItem("session", JSON.stringify(response));

    return response;
}


