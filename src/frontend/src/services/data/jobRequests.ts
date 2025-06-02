import { PostAsync } from "./backend";
import type { JobRequest } from "../../types";

/**
 * Creates a new job request.
 * @param request The job request details
 * @returns The created job request from the server
 */
export async function createJobRequestAsync(request: Omit<JobRequest, 'id' | 'clientId' | 'status' | 'createdAt' | 'updatedAt'>): Promise<JobRequest> {
    const response = await PostAsync<JobRequest>('JobRequest', request);
    return response;
}

/**
 * Gets all job requests for the current user.
 * @returns A list of job requests
 */
export async function getJobRequestsAsync(): Promise<JobRequest[]> {
    const response = await PostAsync<JobRequest[]>('JobRequest/GetAll', {});
    return response;
} 