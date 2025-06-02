export interface Job {
  id?: string;
  deleted?: boolean;
  timeStamp?: Date;
  requestorID?: string;
  clientID?: string;
  purchaseOrderNumber: string;
  jobName: string;
  time: Date;
  location: string;
  numWorkers: number;
  numHours: number;
  approved: boolean;
}