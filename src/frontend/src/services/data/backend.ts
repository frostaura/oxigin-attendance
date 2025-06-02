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
        const userContext = await GetLoggedInUserContextAsync();
        // If no sessionId is provided, use the one from userContext or localStorage.
        sessionId = userContext?.sessionId || localStorage.getItem("sessionId") || "";
    }

    const finalUrl = `${BASE_BACKEND_URL}/${url}`;
    const request = await fetch(finalUrl, {
        method: "POST",
        headers:
         {
            "Content-Type": "application/json",
            "SessionId": sessionId,
        },
        body: JSON.stringify(body)
    });

    if(request.status === 403) {
        NavigateToSignInPage();
    }
    if(!request.ok) throw new Error(await request.text());

    return await request.json() as T;
}

/**
 * Send a GET request to the backend API with the provided URL and optional query parameters.
 * Automatically includes the session ID from localStorage in the headers.
 * @template T The expected response type.
 * @param {string} url - The endpoint to send the request to (relative to the backend base URL).
 * @param {object} [query] - Optional query parameters as an object.
 * @returns {Promise<T>} The parsed JSON response from the backend.
 */
export async function GetAsync<T>(url: string, query?: object, sessionId?: string | null): Promise<T> {
    if(!sessionId){
        const userContext = await GetLoggedInUserContextAsync();
        sessionId = userContext?.sessionId || localStorage.getItem("sessionId") || "";
    }

    let finalUrl = `${BASE_BACKEND_URL}/${url}`;
    if (query && Object.keys(query).length > 0) {
        const params = new URLSearchParams(query as Record<string, string>);
        finalUrl += `?${params.toString()}`;
    }

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
    if(!request.ok) throw new Error(await request.text());

    return await request.json() as T;
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