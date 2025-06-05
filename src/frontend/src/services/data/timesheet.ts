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
 * Get all timesheets for a job.
 * @param jobId The job ID
 * @returns List of timesheets for the job
 */
export async function getTimesheetsForJobAsync(jobId: string): Promise<Timesheet[]> {
    const response = await GetAsync<Timesheet[]>(`Timesheet/job/${jobId}`);
    return response;
}

/**
 * Updates a timesheet with sign out (sets outTime).
 * @param timesheetId The timesheet ID
 * @param outTime The sign-out time (Date or ISO string)
 * @returns The updated timesheet from the server
 */
export async function signOutAsync(timesheetId: string, outTime: Date | string): Promise<Timesheet> {
    // Convert to Date object if string
    const date = typeof outTime === "string" ? new Date(outTime) : outTime;
    
    // Ensure the date is in UTC and format it without milliseconds
    const utcDate = new Date(Date.UTC(
        date.getUTCFullYear(),
        date.getUTCMonth(),
        date.getUTCDate(),
        date.getUTCHours(),
        date.getUTCMinutes(),
        date.getUTCSeconds()
    ));
    
    // Format date in a way that ASP.NET Core can parse
    const formattedDate = utcDate.toISOString().split('.')[0] + "Z";
    
    // Send the formatted date string
    const response = await PutAsync<Timesheet>(`Timesheet/signout/${timesheetId}`, `"${formattedDate}"`);
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
