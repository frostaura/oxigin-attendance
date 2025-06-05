// user.ts
// Service functions and types for user authentication and user-related API calls.
// Handles user sign-in and session management for the Oxigin Attendance frontend.

import type { User, UserSignUpResponse, UserSigninResponse } from "../../models/userModels";
import { GetAsync, PostAsync, DeleteAsync, PatchAsync } from "./backend";

/**
 * Sign in a user with the provided email and password.
 * Stores the session ID in localStorage upon successful authentication.
 * @param {string} email - The user's email address.
 * @param {string} password - The user's password.
 * @returns {Promise<UserSigninResponse>} The user and session ID from the backend.
 */
export async function SignInAsync(email: string, password: string): Promise<UserSigninResponse>{
    // Send plain password to backend - it will handle hashing
    const response = await PostAsync<UserSigninResponse>('User/SignIn', { 
        email: email.toLowerCase(), // Ensure email is lowercase
        password 
    });

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
export async function SignUpAsync(
    name: string, 
    contactNr: string, 
    email: string, 
    password: string,
): Promise<UserSignUpResponse> {
    // Send plain password to backend - it will handle hashing
    const response = await PostAsync<UserSignUpResponse>('User/SignUp', { 
        name, 
        contactNr, 
        email, 
        password,
        userType: 0
    });
    localStorage.setItem("session", JSON.stringify(response));

    return response;
}

/**
 * Creates a new user without affecting the current admin's session.
 * This is specifically for admin use when creating new users.
 * @param {string} name - The user's name.
 * @param {string} contactNr - The user's contact number.
 * @param {string} email - The user's email address.
 * @param {string} password - The user's plain text password.
 * @param {number} userType - The user's type (from UserType enum).
 * @param {string | null} [clientID] - Optional client ID to associate with the user.
 * @param {string | null} [employeeID] - Optional employee ID to associate with the user.
 * @returns {Promise<UserSignUpResponse>} The created user and session ID from the backend.
 */
export async function CreateUserAsAdmin(
    name: string, 
    contactNr: string, 
    email: string, 
    password: string,
    userType: number,
    clientID?: string | null,
    employeeID?: string | null
): Promise<UserSignUpResponse> {
    // Only include clientID and employeeID in the request if they have values
    const requestData = {
        name, 
        contactNr, 
        email: email.toLowerCase(), // Ensure email is lowercase
        password, // Send plain password to backend - it will handle hashing
        userType,
        ...(clientID ? { clientID } : {}),
        ...(employeeID ? { employeeID } : {})
    };
    return await PostAsync<UserSignUpResponse>('User/SignUp', requestData);
}

/**
 * Get all users.
 * @returns {Promise<User[]>} List of User entities.
 */
export async function getUsersAsync(): Promise<User[]> {
    return await GetAsync<User[]>("User");
}

/**
 * Update a user's details.
 * @param {any} user - The user object with updated details.
 * @returns {Promise<User>} The updated user.
 */
export async function updateUserAsync(user: any): Promise<User> {
    // First get the existing user to get their password
    const existingUser = await GetAsync<User>(`User`);
    if (!existingUser) {
        throw new Error("User not found");
    }

    // Convert property names to match backend expectations
    const payload = {
        Id: user.id,
        Name: user.name,
        Email: user.email?.toLowerCase(), // Ensure email is lowercase
        ContactNr: user.contactNr,
        UserType: user.userType,
        ClientID: user.clientID,
        EmployeeID: user.employeeID,
        Password: user.password || existingUser.password // Use new password if provided, otherwise use existing
    };

    // Send the update request with the payload
    return await PatchAsync<User>(`User`, payload);
}

/**
 * Delete a user.
 * @param {string} userId - The ID of the user to delete.
 * @returns {Promise<void>}
 */
export async function deleteUserAsync(userId: string): Promise<void> {
    await DeleteAsync(`User/${userId}`);
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


