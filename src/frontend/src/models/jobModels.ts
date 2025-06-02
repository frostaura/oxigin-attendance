export interface JobRequest {
    id: string;
    jobName: string;
    requestorName: string;
    purchaseOrderNumber: string;
    scheduledDateTime: Date;
    location: string;
    numberOfWorkers: number;
    numberOfHours: number;
    status: string;
    clientId: string;
    createdAt: Date;
    updatedAt: Date;
  }
  
  export interface JobAllocation {
    id: string;
    jobId: string;
    employeeId: string;
    status: 'pending' | 'accepted' | 'rejected' | 'completed';
    checkInTime?: Date;
    checkOutTime?: Date;
    createdAt: Date;
    updatedAt: Date;
  }