// User Types
export interface BaseUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface Client extends BaseUser {
  companyName: string;
  address: string;
  type: 'client';
}

export interface Employee extends BaseUser {
  employeeId: string;
  role: 'employee' | 'site_manager' | 'admin';
  type: 'employee'; 
}

// Job Types
export interface JobRequest {
  id: string;
  jobName: string;
  requestorName: string;
  purchaseOrderNumber: string;
  time: Date;
  location: string;
  numberOfWorkers: number;
  numberOfHours: number;
  approved: boolean;
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

// Form Types
export interface LoginFormValues {
  email: string;
  password: string;
}

export interface ClientRegistrationFormValues {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  phone: string;
  companyName: string;
  address: string;
}

export interface EmployeeRegistrationFormValues {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  phone: string;
  employeeId: string;
  role: 'employee' | 'site_manager' | 'admin';
}

// API Response Types
export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: string;
}

// Notification Types
export interface Notification {
  id: string;
  userId: string;
  type: 'sms' | 'email';
  message: string;
  status: 'pending' | 'sent' | 'failed';
  createdAt: Date;
} 

export interface JobData {
  key: string;
  jobId: string;
  purchaseOrder: string;
  jobName: string;
  requestor?: string;
  contact?: string;
  location?: string;
  date?: string;
  status?: string;
  description?: string;
}

export interface EmployeeData {
  key: string;
  id: string;
  name: string;
  email: string;
  phone?: string;
  role?: string;
  status?: string;
}

export interface ClientData {
  key: string;
  id: string;
  name: string;
  email: string;
  phone?: string;
  company?: string;
  status?: string;
}

export interface TimesheetData {
  key: string;
  id: string;
  employeeId: string;
  employeeName: string;
  date: string;
  checkIn?: string;
  checkOut?: string;
  totalHours?: number;
  status?: string;
} 

export interface ErrorResponse {
  origin: string;
  message: string;
  data: Record<string, string>;
}
