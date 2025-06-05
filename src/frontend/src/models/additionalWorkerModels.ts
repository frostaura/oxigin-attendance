// additionalWorkerModels.ts
// Model representing an AdditionalWorker entity, matching the backend C# model.

import type { Job } from './jobModels';

export interface AdditionalWorker {
    /**
     * The type of worker (e.g., "Bartender", "Security").
     */
    workerType: string;
    /**
     * The number of workers needed of this type.
     */
    numWorkers: number;
    /**
     * The number of hours needed for this worker type.
     */
    numHours: number;
    /**
     * The job ID this additional worker is linked to.
     */
    jobID: string;
    /**
     * The job this additional worker is linked to.
     */
    job?: Job;
}
