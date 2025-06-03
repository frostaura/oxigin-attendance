import type { Allocation } from "../../models/jobAllocationModels";
import { PostAsync, GetAsync, DeleteAsync } from "./backend";

/**
 * Creates a new job allocation.
 * @param allocation The allocation details
 * @returns The created allocation from the server
 */
export async function createJobAllocationAsync(allocation: Allocation): Promise<Allocation> {
    const response = await PostAsync<Allocation>('JobAllocation', allocation);
    return response;
}

/**
 * Gets all allocations for a given job.
 * @param jobId The job ID
 * @returns A list of allocations for the job
 */
export async function getAllocationsForJobAsync(jobId: string): Promise<Array<Allocation>> {
    const response = await GetAsync<Array<Allocation>>(`JobAllocation/job/${jobId}`);
    return response;
}

/**
 * Gets all allocations for a given employee.
 * @param employeeId The employee ID
 * @returns A list of allocations for the employee
 */
export async function getAllocationsForEmployeeAsync(employeeId: string): Promise<Array<Allocation>> {
    const response = await GetAsync<Array<Allocation>>(`JobAllocation/employee/${employeeId}`);
    return response;
}

/**
 * Gets a single allocation by its ID.
 * @param id The allocation ID
 * @returns The allocation, or null if not found
 */
export async function getJobAllocationByIdAsync(id: string): Promise<Allocation | null> {
    const response = await GetAsync<Allocation>(`JobAllocation/${id}`);
    return response;
}

/**
 * Soft deletes an allocation by its ID.
 * @param id The allocation ID
 */
export async function deleteJobAllocationAsync(id: string): Promise<void> {
    await DeleteAsync(`JobAllocation/${id}`);
}
