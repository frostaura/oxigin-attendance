import type { Timesheet } from "../../models/timesheetModels";
import { PostAsync, GetAsync, PutAsync } from "./backend";

/**
 * Creates a new timesheet (sign in).
 * @param timesheet The timesheet details
 * @returns The created timesheet from the server
 */
export async function signInAsync(timesheet: Timesheet): Promise<Timesheet> {
    const response = await PostAsync<Timesheet>("Timesheet/signin", timesheet);
    return response;
}

/**
 * Updates a timesheet with sign out (sets outTime).
 * @param timesheetId The timesheet ID
 * @param outTime The sign-out time (Date or ISO string)
 * @returns The updated timesheet from the server
 */
export async function signOutAsync(timesheetId: string, outTime: Date | string): Promise<Timesheet> {
    // Ensure the body is always an object, as required by fetch
    const body = { outTime: typeof outTime === "string" ? outTime : outTime.toISOString() };
    const response = await PutAsync<Timesheet>(`Timesheet/signout/${timesheetId}`, body);
    return response;
}

/**
 * Gets all timesheets.
 * @returns A list of all timesheets
 */
export async function getAllTimesheetsAsync(): Promise<Array<Timesheet>> {
    const response = await GetAsync<Array<Timesheet>>("Timesheet");
    return response;
}

/**
 * Gets all timesheets for a particular job.
 * @param jobId The job ID
 * @returns A list of timesheets for the job
 */
export async function getTimesheetsForJobAsync(jobId: string): Promise<Array<Timesheet>> {
    const response = await GetAsync<Array<Timesheet>>(`Timesheet/job/${jobId}`);
    return response;
}
