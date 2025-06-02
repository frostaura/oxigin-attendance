import type { Job } from "./jobModels";
import type { Employee } from "./employeeModels";

/**
 * Represents a timesheet entry for an employee's work session.
 * Mirrors the backend Timesheet entity (inherits from BaseEntity).
 */
export interface Timesheet {
    /** Unique identifier for the timesheet. */
    id: string;
    /** Date and time of check-in. */
    timeIn: string; // ISO string
    /** Date and time of check-out. */
    timeOut: string; // ISO string or empty if not signed out
    /** Foreign key to the Job. */
    jobID: string;
    /** Navigation property for the related Job. */
    job?: Job;
    /** Foreign key to the Employee. */
    employeeID: string;
    /** Navigation property for the related Employee. */
    employee?: Employee;
    /** Foreign key to the SiteManager. */
    siteManagerID: string;
    /** Navigation property for the related SiteManager (optional, if needed). */
    // siteManager?: Employee;
    /** Indicates if the timesheet is soft-deleted. */
    deleted: boolean;
    /** Date/time the entity was created. */
    created: string;
    /** Date/time the entity was last updated. */
    updated: string;
}
