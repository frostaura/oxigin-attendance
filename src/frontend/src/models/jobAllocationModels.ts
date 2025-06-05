// Frontend model for job allocation, aligned with backend Allocation entity (inherits from BaseEntity)

import type { Job } from "./jobModels";
import type { Employee } from "./employeeModels";

export interface Allocation {
    name: string;
    description?: string;
    time: Date;
    hoursNeeded: number;
    jobID: string;
    employeeID: string;
    job?: Job;
    employee?: Employee;
    deleted?: boolean;
}
