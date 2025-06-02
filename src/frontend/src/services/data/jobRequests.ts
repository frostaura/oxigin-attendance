import { PostAsync, GetAsync } from "./backend";
import type { Job } from "../../types";

/**
 * Creates a new job request.
 * @param request The job request details
 * @returns The created job request from the server
 */
export async function createJobsAsync(request: Job): Promise<Job> {
    const response = await PostAsync<Job>('Job', request);
    return response;
}

/**
 * Gets all job requests for the current user.
 * @returns A list of job requests
 */
export async function getJobRequestsAsync(): Promise<Array<Job>> {
    const response = await GetAsync<Array<Job>>('Job');
    return response;
}