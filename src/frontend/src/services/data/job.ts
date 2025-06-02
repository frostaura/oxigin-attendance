import type { Job } from "../../models/jobModels";
import { PostAsync, GetAsync } from "./backend";

/**
 * Creates a new job request.
 * @param request The job request details
 * @returns The created job request from the server
 */
export async function createJobAsync(request: Job): Promise<Job> {
    const response = await PostAsync<Job>('Job', request);
    return response;
}

/**
 * Gets all job requests for the current user.
 * @returns A list of job requests
 */
export async function getJobsAsync(): Promise<Array<Job>> {
    const response = await GetAsync<Array<Job>>('Job');
    return response;
}

/**
 * Approves a pending job request.
 * @param request The job request to approve (must include id)
 * @returns The updated job request with approved status
 */
export async function approveJobAsync(request: Job): Promise<Job> {
    const response = await PostAsync<Job>('Job/approve', request);
    return response;
}

/**
 * Rejects a pending job request.
 * @param request The job request to reject (must include id)
 * @returns The updated job request with rejected status
 */
export async function rejectJobAsync(request: Job): Promise<Job> {
    const response = await PostAsync<Job>('Job/reject', request);
    return response;
}

/**
 * Gets all jobs that require approval by the current user.
 * @returns A list of jobs requiring approval
 */
export async function getJobsRequiringApprovalAsync(): Promise<Array<Job>> {
    const response = await GetAsync<Array<Job>>('Job/requiring-approval');
    return response;
}

/**
 * Gets all jobs that are awaiting confirmation by the current user.
 * @returns A list of jobs awaiting confirmation
 */
export async function getJobsAwaitingConfirmationAsync(): Promise<Array<Job>> {
    const response = await GetAsync<Array<Job>>('Job/awaiting-confirmation');
    return response;
}