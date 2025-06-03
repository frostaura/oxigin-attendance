// additionalWorker.ts
// Service functions for interacting with the AdditionalWorkerController backend API.
// Provides utility methods for getting, adding, and removing additional workers for jobs.

import type { AdditionalWorker } from "../../models/additionalWorkerModels";
import { GetAsync, PostAsync, DeleteAsync } from "./backend";

/**
 * Get all additional workers for a given job.
 * @param {string} jobId - The job ID.
 * @param {string | null} sessionId - Optional session ID for authentication.
 * @returns {Promise<AdditionalWorker[]>} List of AdditionalWorker entities.
 */
export async function getAdditionalWorkersByJob(jobId: string, sessionId?: string | null): Promise<AdditionalWorker[]> {
    return await GetAsync<AdditionalWorker[]>(`AdditionalWorker/job/${jobId}`, sessionId);
}

/**
 * Add an additional worker to a job.
 * @param {AdditionalWorker} worker - The worker details to add
 * @param {string | null} sessionId - Optional session ID for authentication.
 * @returns {Promise<AdditionalWorker>} The created AdditionalWorker entity.
 */
export async function addAdditionalWorker(worker: AdditionalWorker, sessionId?: string | null): Promise<AdditionalWorker> {
    return await PostAsync<AdditionalWorker>("AdditionalWorker", worker, sessionId);
}

/**
 * Remove an additional worker from a job.
 * @param {string} id - The AdditionalWorker ID.
 * @param {string | null} sessionId - Optional session ID for authentication.
 * @returns {Promise<void>} Resolves when the worker is removed.
 */
export async function removeAdditionalWorker(id: string, sessionId?: string | null): Promise<void> {
    await DeleteAsync<void>(`AdditionalWorker/${id}`, sessionId);
}
