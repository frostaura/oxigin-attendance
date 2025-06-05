import type { Job } from "../../models/jobModels";
import { PostAsync, GetAsync, PatchAsync } from "./backend";

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
 * Gets all jobs.
 * @returns A list of all jobs
 */
export async function getJobsAsync(): Promise<Array<Job>> {
    const response = await GetAsync<Array<Job>>('Job');
    return response;
}

/**
 * Approve a job request.
 * @param jobId The ID of the job to approve
 * @returns The updated job
 */
export async function approveJobAsync(jobId: string): Promise<Job> {
    const response = await PatchAsync<Job>(`Job/approve`, { id: jobId, approved: true } as Job);
    return response;
}

/**
 * Reject a job request.
 * @param jobId The ID of the job to reject
 * @returns The updated job
 */
export async function rejectJobAsync(jobId: string): Promise<Job> {
    const response = await PatchAsync<Job>(`Job/reject`, { id: jobId, approved: false } as Job);
    return response;
}

/**
 * Updates a job's approval status.
 * @param jobId The ID of the job to update
 * @param approved Whether the job is approved or rejected
 * @returns The updated job
 */
export async function updateJobApprovalAsync(jobId: string, approved: boolean): Promise<Job> {
    const response = await PatchAsync<Job>(`Job/${jobId}`, { approved });
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