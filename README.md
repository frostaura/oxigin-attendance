# Oxigin Staff Scheduling & Attendance Platform

## Overview

This system is designed to manage staff allocation, client event requests, and attendance check-ins for Oxigin. It supports multiple user roles and provides a web-based platform accessible via desktop and mobile browsers. Notifications are delivered via SMS and Email.

---

## Key Features & Use Cases

### 1. Client Request Management
- Clients securely log in to the platform.
- Clients create staffing requests specifying event date, time, number of staff, role, and location.
- Job requests require approval from both the client and the site manager/team lead before proceeding.
- Clients can view, approve, or reject their own job requests.

### 2. Staff Allocation & Notifications
- Upon full approval of a request (client + site manager), admins allocate employees to the event.
- Allocated employees receive SMS and Email notifications detailing event information.
- Site managers/team leads receive notifications about job approvals and employee check-ins.

### 3. Employee Features
- Employees, including site supervisors, register and authenticate on the platform.
- Employees receive SMS/Email notifications for job assignments, reminders, and check-in confirmations.
- Employees check in onsite via a geo-location and facial recognition-enabled device with a medium-sized screen and monitor lamp.
- Check-in data includes GPS coordinates and timestamp and is recorded and sent to site managers.

### 4. Reminders & Notifications
- Automated SMS and Email reminders are sent to:
  - Clients: to confirm upcoming events.
  - Employees: to ensure punctuality and attendance.
  - Site managers/team leads: to approve/reject jobs and track onsite attendance.

### 5. Administrative Features
- Admins manage employees, clients, and client requests.
- Admins allocate staff to events after approval.
- Admin dashboard provides real-time monitoring of jobs, employee allocations, and attendance.
- Timesheets can be generated and printed for various timeframes (daily, weekly, fortnightly, monthly, or by job).

### 6. Accessibility & Branding
- The platform is accessible via web browsers on desktop and mobile devices (responsive design).
- Notifications are via SMS and Email (no native app or push notifications).
- Consistent branding including styling, icons, system name, and a custom domain are used throughout the platform.

---

## User Roles & Responsibilities

| Role               | Key Responsibilities                                 |
|--------------------|-----------------------------------------------------|
| Client             | Submit, approve/reject job requests, view jobs.     |
| Employee           | Register, login, receive notifications, check-in.  |
| Site Manager/Team Lead | Approve/reject job requests, receive check-in notifications, receive reminders. |
| Admin/Management   | Manage clients, employees, job requests, allocations, attendance monitoring, and reporting. |

---

## Hardware Requirements for Check-ins

- Medium-sized screen with monitor lamp for facial recognition.
- Primary network connectivity via on-site WiFi.
- LTE modem backup with contract SIM card for connectivity fallback.

---

## Notification Channels

- SMS and Email only for all notifications and reminders.
- Push notifications are not used as per client preference and platform constraints.

---

## Summary

This platform enables smooth coordination between clients, employees, site managers, and administrative staff to schedule, approve, allocate, and monitor staffing for client events. Automated notifications and attendance tracking help ensure operational efficiency and accountability.

---

## Documentation
| Content | Description
| -- | -- |
| [Backend Project](./.docs/backend.md) | The backend project, developed in Dotnet 9 to serve as the system's API, handle DB connectivity, auth etc.|
| [Frontend Project](./.docs/frontend.md) | The frontend project, developed with ReactJS and serves as the user interface for allowing for the various use cases the system requires.