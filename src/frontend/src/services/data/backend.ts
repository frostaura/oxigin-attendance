// backend.ts
// Service functions for interacting with the backend API for Oxigin Attendance.
// Provides utility methods for authentication checks and making POST requests.

import type { UserSigninResponse } from "../../models/userModels";

// Base URL for the backend API.
const BASE_BACKEND_URL: string = "http://localhost:5275";

/**
 * Send a POST request to the backend API with the provided URL and body.
 * Automatically includes the session ID from localStorage in the headers.
 * @template T The expected response type.
 * @param {string} url - The endpoint to send the request to (relative to the backend base URL).
 * @param {object} body - The request payload to send as JSON.
 * @returns {Promise<T>} The parsed JSON response from the backend.
 */
export async function PostAsync<T>(url: string, body: object, sessionId?: string | null): Promise<T>{
    if(!sessionId){
        try {
            const userContext = await GetLoggedInUserContextAsync();
            // If no sessionId is provided, use the one from userContext
            sessionId = userContext?.sessionId || "";
        }
        catch (error) {
            console.error("Error parsing session from localStorage:", error);
        }
    }

    const finalUrl = `${BASE_BACKEND_URL}/${url}`;
    const request = await fetch(finalUrl, {
        method: "POST",
        headers:
        {
            "Content-Type": "application/json",
            "SessionId": sessionId || ""
        },
        body: JSON.stringify(body)
    });

    if(request.status === 403) {
        NavigateToSignInPage();
    }
    
    const responseText = await request.text();
    if(!request.ok) {
        try {
            const errorData = JSON.parse(responseText);
            throw new Error(JSON.stringify(errorData));
        } catch {
            throw new Error(responseText);
        }
    }

    try {
        return JSON.parse(responseText) as T;
    } catch {
        throw new Error("Invalid JSON response from server");
    }
}

/**
 * Send a GET request to the backend API with the provided URL and optional query parameters.
 * Automatically includes the session ID from localStorage in the headers.
 * @template T The expected response type.
 * @param {string} url - The endpoint to send the request to (relative to the backend base URL).
 * @returns {Promise<T>} The parsed JSON response from the backend.
 */
export async function GetAsync<T>(url: string, sessionId?: string | null): Promise<T> {
    if(!sessionId){
        const userContext = await GetLoggedInUserContextAsync();
        sessionId = userContext?.sessionId || "";
    }

    const finalUrl = `${BASE_BACKEND_URL}/${url}`;
    const request = await fetch(finalUrl, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "SessionId": sessionId,
        }
    });

    if(request.status === 403) {
        NavigateToSignInPage();
    }
    
    const responseText = await request.text();
    if(!request.ok) {
        try {
            const errorData = JSON.parse(responseText);
            throw new Error(JSON.stringify(errorData));
        } catch {
            throw new Error(responseText);
        }
    }

    try {
        return JSON.parse(responseText) as T;
    } catch {
        throw new Error("Invalid JSON response from server");
    }
}

/**
 * Send a DELETE request to the backend API with the provided URL.
 * Automatically includes the session ID from localStorage in the headers.
 * @template T The expected response type.
 * @param {string} url - The endpoint to send the request to (relative to the backend base URL).
 * @returns {Promise<T>} The parsed JSON response from the backend.
 */
export async function DeleteAsync<T>(url: string, sessionId?: string | null): Promise<T> {
    if(!sessionId){
        const userContext = await GetLoggedInUserContextAsync();
        sessionId = userContext?.sessionId || "";
    }
    const finalUrl = `${BASE_BACKEND_URL}/${url}`;
    const request = await fetch(finalUrl, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "SessionId": sessionId,
        }
    });
    if(request.status === 403) {
        NavigateToSignInPage();
    }
    
    const responseText = await request.text();
    if(!request.ok) {
        try {
            const errorData = JSON.parse(responseText);
            throw new Error(JSON.stringify(errorData));
        } catch {
            throw new Error(responseText);
        }
    }

    // If the backend returns no content, just return undefined as T
    if(request.status === 204) return undefined as T;

    try {
        return JSON.parse(responseText) as T;
    } catch {
        throw new Error("Invalid JSON response from server");
    }
}

/**
 * Send a PUT request to the backend API with the provided URL and body.
 * Automatically includes the session ID from localStorage in the headers.
 * @template T The expected response type.
 * @param {string} url - The endpoint to send the request to (relative to the backend base URL).
 * @param {object | string} body - The request payload to send as JSON. Can be a string for raw body.
 * @returns {Promise<T>} The parsed JSON response from the backend.
 */
export async function PutAsync<T>(url: string, body: object | string, sessionId?: string | null): Promise<T> {
    if(!sessionId){
        const userContext = await GetLoggedInUserContextAsync();
        sessionId = userContext?.sessionId || "";
    }
    const finalUrl = `${BASE_BACKEND_URL}/${url}`;
    const request = await fetch(finalUrl, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "SessionId": sessionId,
        },
        body: typeof body === "string" ? body : JSON.stringify(body)
    });
    if(request.status === 403) {
        NavigateToSignInPage();
    }
    
    const responseText = await request.text();
    if(!request.ok) {
        try {
            const errorData = JSON.parse(responseText);
            throw new Error(JSON.stringify(errorData));
        } catch {
            throw new Error(responseText);
        }
    }

    try {
        return JSON.parse(responseText) as T;
    } catch {
        throw new Error("Invalid JSON response from server");
    }
}

/**
 * Send a PATCH request to the backend API with the provided URL and body.
 * Automatically includes the session ID from localStorage in the headers.
 * @template T The expected response type.
 * @param {string} url - The endpoint to send the request to (relative to the backend base URL).
 * @param {object} body - The request payload to send as JSON.
 * @returns {Promise<T>} The parsed JSON response from the backend.
 */
export async function PatchAsync<T>(url: string, body: object, sessionId?: string | null): Promise<T> {
    if(!sessionId){
        const userContext = await GetLoggedInUserContextAsync();
        sessionId = userContext?.sessionId || "";
    }
    const finalUrl = `${BASE_BACKEND_URL}/${url}`;
    const request = await fetch(finalUrl, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            "SessionId": sessionId,
        },
        body: JSON.stringify(body)
    });
    if(request.status === 403) {
        NavigateToSignInPage();
    }
    
    const responseText = await request.text();
    if(!request.ok) {
        try {
            const errorData = JSON.parse(responseText);
            throw new Error(JSON.stringify(errorData));
        } catch {
            throw new Error(responseText);
        }
    }

    try {
        return JSON.parse(responseText) as T;
    } catch {
        throw new Error("Invalid JSON response from server");
    }
}

/**
 * Check if the user is currently logged in by verifying the presence of a session ID in localStorage.
 * @returns {boolean} True if a session ID exists, otherwise false.
 */
export async function GetLoggedInUserContextAsync(): Promise<UserSigninResponse | null> {
    const session = localStorage.getItem("session");
    const parsedSession: UserSigninResponse = session ? JSON.parse(session) : null;

    if(!parsedSession) return null;

    // Refresh the session if it exists.
    const newSession = await PostAsync<UserSigninResponse>('User/RefreshSession', {}, parsedSession.sessionId);

    localStorage.setItem("session", JSON.stringify(newSession));
    return newSession;
}

/**
 * Sign a user out and navigate to the sign in page.
 */
export function NavigateToSignInPage(): void{
    localStorage.removeItem("session");
    window.location.href = window.location.origin;
}