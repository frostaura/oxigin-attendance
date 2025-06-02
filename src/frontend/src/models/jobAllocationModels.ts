// Frontend model for job allocation, aligned with backend Allocation entity (inherits from BaseEntity)

export interface Allocation {
    id: string;
    jobID: string;
    employeeID: string;
    allocationType: string; // Adjust type if you have an enum for allocation type
    startTime: string; // ISO string
    endTime: string | null; // ISO string or null
    notes?: string | null;
    deleted: boolean;
    created: string; // ISO string
    updated: string; // ISO string
}
