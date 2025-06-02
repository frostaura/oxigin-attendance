// additionalWorkerModels.ts
// Model representing an AdditionalWorker entity, matching the backend C# model.

export interface AdditionalWorker {
    /**
     * The unique identifier for the additional worker.
     */
    id: string;
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
     * (Optional) The related job entity, if included in the response.
     */
    job?: any;
}
