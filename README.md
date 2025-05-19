# Staffing & Attendance Management System

This document outlines the primary use cases, managers, and key function signatures for the Staffing & Attendance Management System, based on client requirements and architectural design principles.

---

## Table of Contents
- [Overview](#overview)
- [Use Case Summary](#use-case-summary)
- [Managers and Key Functions](#managers-and-key-functions)
- [Data Accessors / Repositories](#data-accessors--repositories)

---

## Overview

This platform enables clients to request staff for events, allows employees to register, check in onsite, and receive notifications, while admins and site managers control allocation, attendance monitoring, and approvals. The system uses:
- Secure login and role-based access
- Dual approval workflow (client & site manager)
- SMS and Email notifications (no push notifications)
- Geo-location and facial recognition check-ins with reliable on-site hardware
- Web-based responsive platform accessible on desktop and mobile
- Timesheet generation and printing capabilities

---

## Use Case Summary

| Use Case ID | Name                        | Primary Actor(s)               | Description                                                                                     |
|-------------|-----------------------------|-------------------------------|-------------------------------------------------------------------------------------------------|
| **UC-A1**  | Monitor Attendance          | Admin                         | View employee check-ins (GPS + timestamp), flag no-shows.                                       |
| **UC-A2**  | Manage Employees           | Admin                         | Create, update, and manage employee profiles including supervisors.                             |
| **UC-A3**  | Manage Jobs               | Admin                         | View and manage client job requests and assigned employees.                                    |
| **UC-A4**  | Manage Clients            | Admin                         | Manage client profiles and monitor staffing requests.                                         |
| **UC-A5**  | Allocate Employees to Event | Admin                         | Assign employees to approved events; trigger notifications.                                    |
| **UC-A6**  | Monitor Employee Check-ins | Admin, Site Manager           | Track onsite attendance and notify site managers.                                              |
| **UC-E1**  | Register                  | Employee                      | Employee registration including team leads/supervisors.                                       |
| **UC-E2**  | Login                     | Employee                      | Authenticate employees.                                                                         |
| **UC-E3**  | Check In                 | Employee                      | Onsite check-in using GPS and facial recognition.                                              |
| **UC-E4**  | Receive Notifications      | Employee                      | SMS/Email notifications for assignments and reminders.                                        |
| **UC-SM1** | Receive Notification       | Site Manager / Team Lead      | Notifications about employee check-ins and job statuses.                                      |
| **UC-SM2** | Reminders (Upcoming Events)| Site Manager / Team Lead      | Event reminder notifications.                                                                  |
| **UC-SM3** | Reminders (Confirm Events) | Site Manager / Team Lead      | Reminders to approve/reject staffing requests.                                                |
| **UC-SM4** | Approve / Reject Job       | Site Manager / Team Lead, Client | Joint job approval/rejection for staffing requests.                                      |
| **UC-C1**  | Log Job Request            | Client                        | Submit job requests specifying dates, roles, hours, and locations.                            |
| **UC-C2**  | Approve / Reject Job       | Client, Site Manager          | Approve or reject job requests collaboratively.                                               |
| **UC-B1**  | View Jobs                 | Any Authorized User           | View job listings relevant to user role.                                                     |
| **UC-B2**  | Print Timesheet            | Any Authorized User           | Generate printable timesheets filtered by periods or job.                                    |

---

## Managers and Key Functions

### AuthenticationManager
- `AuthenticateUser(username: String, password: String): AuthResult`  
  Authenticate credentials and start user session.

- `RegisterEmployee(employeeData: EmployeeDTO): RegistrationResult`  
  Register a new employee.

- `LogoutUser(userId: Guid): void`  
  Terminate user session.

---

### ClientRequestManager
- `CreateJobRequest(clientId: Guid, requestDetails: JobRequestDTO): JobRequest`  
  Create a new staffing request.

- `GetJobRequests(clientId: Guid = null, status: JobStatus = null): List<JobRequest>`  
  Retrieve job requests with optional filters.

- `ApproveRequest(requestId: Guid, approverId: Guid): ApprovalResult`  
  Approve job request.

- `RejectRequest(requestId: Guid, approverId: Guid, reason: String): RejectionResult`  
  Reject job request with explanation.

- `IsFullyApproved(requestId: Guid): bool`  
  Check if both client and site manager approvals are obtained.

---

### EmployeeManager
- `CreateEmployee(employeeData: EmployeeDTO): Employee`  
  Add new employee profile.

- `GetEmployee(employeeId: Guid): Employee`  
  Fetch employee data.

- `UpdateEmployee(employeeId: Guid, employeeData: EmployeeDTO): bool`  
  Update profile details.

- `GetAvailableEmployees(dateRange: DateRange, role: String): List<Employee>`  
  Find employees available for assignment.

- `AssignRole(employeeId: Guid, role: UserRole): bool`  
  Assign user role.

---

### AllocationManager
- `AllocateEmployeeToEvent(requestId: Guid, employeeId: Guid): AllocationResult`  
  Assign employee to an event.

- `GetAllocations(eventId: Guid): List<EmployeeAllocation>`  
  Retrieve all allocations for an event.

- `RemoveAllocation(allocationId: Guid): bool`  
  Remove employee allocation.

- `TriggerAllocationNotifications(eventId: Guid): void`  
  Notify allocated employees by SMS/Email.

---

### CheckInManager
- `RecordCheckIn(employeeId: Guid, eventId: Guid, gpsLocation: GeoPoint, timestamp: DateTime): CheckInRecordResponse`  
  Record GPS and timestamp check-in with facial recognition.

- `GetCheckIns(eventId: Guid): List<CheckInRecord>`  
  Retrieve check-ins for an event.

- `NotifySiteManager(checkInRecord: CheckInRecord): void`  
  Notify site manager of new check-ins.

---

### NotificationManager
- `SendNotification(recipientId: Guid, message: String, type: NotificationType): bool`  
  Send SMS or Email notification.

- `ScheduleReminder(recipientId: Guid, message: String, sendDateTime: DateTime): bool`  
  Schedule future reminder delivery.

- `BulkSendNotifications(recipientIds: List<Guid>, message: String, type: NotificationType): int`  
  Send notifications to multiple recipients.

---

### TimesheetManager
- `GenerateTimesheet(employeeId: Guid, startDate: DateTime, endDate: DateTime): TimesheetReport`  
  Create timesheet report for a time period.

- `PrintTimesheet(timesheet: TimesheetReport, format: PrintFormat): byte[]`  
  Produce printable/exportable timesheet document.

---

### DashboardManager
- `GetJobSummary(filters: JobFilterCriteria): JobSummaryDTO`  
  Summary of job requests by status, time, client, etc.

- `GetEmployeeAttendanceSummary(filters: AttendanceFilterCriteria): AttendanceSummaryDTO`  
  Overview of employee attendance and exceptions.

- `GetAllocationOverview(): List<AllocationOverviewDTO>`  
  Allocation status and staffing overview.

---

## Data Accessors / Repositories

Interfaces for persistent storage and retrieval:

- **IEmployeeRepository**  
  Methods:
  - `Add(Employee employee): Task<bool>`
  - `Update(Employee employee): Task<bool>`
  - `FindById(Guid employeeId): Task<Employee>`
  - `FindAvailable(DateRange dates, string role): Task<List<Employee>>`

- **IJobRequestRepository**  
  Methods:
  - `Add(JobRequest request): Task<bool>`
  - `Update(JobRequest request): Task<bool>`
  - `FindById(Guid requestId): Task<JobRequest>`
  - `FindByClient(Guid clientId): Task<List<JobRequest>>`
  - `FindByStatus(JobStatus status): Task<List<JobRequest>>`

- **IAllocationRepository**  
  Methods:
  - `Add(Allocation allocation): Task<bool>`
  - `Remove(Guid allocationId): Task<bool>`
  - `FindByEvent(Guid eventId): Task<List<Allocation>>`

- **ICheckInRepository**  
  Methods:
  - `Add(CheckInRecord record): Task<bool>`
  - `FindByEvent(Guid eventId): Task<List<CheckInRecord>>`

- **INotificationRepository**  
  Methods:
  - `LogNotification(Notification notification): Task<bool>`
  - `GetPendingNotifications(DateTime from, DateTime to): Task<List<Notification>>`

---

## Additional Notes

- **Dual approval** required on client job requests (client & site manager).
- **Notification delivery** via SMS and Email only.
- **Check-in soundness** ensured by GPS location and facial recognition hardware.
- **Responsive web UI** with no native/mobile app planned.
- **Role-based access** across clients, employees (including site managers) and admins.

---

*For questions or collaboration, please contact the development team.*
