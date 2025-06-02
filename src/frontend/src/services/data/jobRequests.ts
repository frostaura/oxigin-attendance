import type { Job } from "../../models/jobModels";
import { PostAsync, GetAsync } from "./backend";

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

/**
 * Gets all jobs that require approval from the current user.
 * @returns A list of jobs requiring approval
 */
export async function getJobsRequiringApprovalAsync(): Promise<Array<Job>> {
    const response = await GetAsync<Array<Job>>('Job/requiring-approval');
    return response;
}