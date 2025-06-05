import React, { useState, useEffect } from "react";
import { Layout, Card, Button, Table, Select, message } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobsAsync } from "../services/data/job";
import { getAllocationsForJobAsync } from "../services/data/jobAllocation";
import { signInAsync } from "../services/data/timesheet";
import { GetLoggedInUserContextAsync } from "../services/data/backend";
import type { Job } from "../models/jobModels";
import type { Employee } from "../models/employeeModels";
import type { Timesheet } from "../models/timesheetModels";
import { getTimesheetsForJobAsync } from "../services/data/timesheet";

const { Content } = Layout;
const { Option } = Select;

interface CheckInRecord {
  key: string;
  jobId: string;
  employeeId: string;
  employeeName: string;
  timeIn: string;
}

const SiteManagerCheckIn: React.FC = () => {
  const navigate = useNavigate();
  const [selectedJob, setSelectedJob] = useState<string>("");
  const [selectedEmployee, setSelectedEmployee] = useState<string>("");
  const [jobs, setJobs] = useState<Job[]>([]);
  const [allocatedEmployees, setAllocatedEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(false);
  const [loadingEmployees, setLoadingEmployees] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [checkInHistory, setCheckInHistory] = useState<CheckInRecord[]>([]);

  useEffect(() => {
    const fetchJobs = async () => {
      try {
        setLoading(true);
        const jobsData = await getJobsAsync();
        setJobs(jobsData);
      } catch (error) {
        console.error("Failed to fetch jobs:", error);
        message.error("Failed to fetch jobs");
      } finally {
        setLoading(false);
      }
    };
    fetchJobs();
  }, []);

  const fetchAllocatedEmployees = async (jobId: string) => {
    try {
      setLoadingEmployees(true);
      const [allocations, timesheets] = await Promise.all([
        getAllocationsForJobAsync(jobId),
        getTimesheetsForJobAsync(jobId)
      ]);
      
      // Get list of employees who are already checked in
      const checkedInEmployeeIds = new Set(
        timesheets
          .filter(timesheet => 
            !timesheet.deleted && 
            timesheet.timeIn && 
            timesheet.timeOut === "0001-01-01T00:00:00" // Not checked out
          )
          .map(timesheet => timesheet.employeeID)
      );
      
      // Filter for valid allocations and exclude already checked-in employees
      const validAllocations = allocations
        .filter(allocation => 
          !allocation.deleted && 
          allocation.employee &&
          !checkedInEmployeeIds.has(allocation.employee.id)
        );
      
      // Remove duplicate employees by keeping the most recent allocation for each employee
      const uniqueEmployees = validAllocations.reduce((acc, current) => {
        const existing = acc.find(a => a.employee?.id === current.employee?.id);
        if (!existing || new Date(current.time) > new Date(existing.time)) {
          const filtered = acc.filter(a => a.employee?.id !== current.employee?.id);
          return [...filtered, current];
        }
        return acc;
      }, [] as typeof validAllocations);

      // Map to employee objects
      const employees = uniqueEmployees
        .map(allocation => allocation.employee as Employee);
      
      setAllocatedEmployees(employees);
    } catch (error) {
      console.error("Failed to fetch allocated employees:", error);
      message.error("Failed to fetch allocated employees");
    } finally {
      setLoadingEmployees(false);
    }
  };

  const handleCheckIn = async () => {
    if (!selectedJob || !selectedEmployee) {
      message.error("Please select both a job and an employee");
      return;
    }

    try {
      setSubmitting(true);
      const userContext = await GetLoggedInUserContextAsync();
      if (!userContext?.user?.id) {
        message.error("No site manager ID found. Please sign in again.");
        return;
      }

      const selectedEmployeeData = allocatedEmployees.find(emp => emp.id === selectedEmployee);
      if (!selectedEmployeeData) {
        message.error("Selected employee not found");
        return;
      }

      // Create the timesheet with required fields
      const timesheet: Timesheet = {
        timeIn: new Date().toISOString(),
        jobID: selectedJob,
        employeeID: selectedEmployee,
        siteManagerID: userContext.user.id
      };

      // Submit the timesheet
      await signInAsync(timesheet);
      
      // Add to check-in history
      const newCheckIn: CheckInRecord = {
        key: `${checkInHistory.length + 1}`,
        jobId: selectedJob,
        employeeId: selectedEmployee,
        employeeName: selectedEmployeeData.accountHolderName || "Unknown",
        timeIn: new Date().toLocaleString(),
      };
      setCheckInHistory(prev => [...prev, newCheckIn]);

      // Reset selections
      setSelectedJob("");
      setSelectedEmployee("");
      message.success("Successfully checked in employee!");
    } catch (error) {
      console.error("Failed to check in:", error);
      message.error("Failed to check in employee. Please try again.");
    } finally {
      setSubmitting(false);
    }
  };

  const columns: ColumnsType<CheckInRecord> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Employee ID", dataIndex: "employeeId", key: "employeeId" },
    { title: "Employee Name", dataIndex: "employeeName", key: "employeeName" },
    { title: "Time In", dataIndex: "timeIn", key: "timeIn" },
  ];

  const handleJobChange = (value: string) => {
    setSelectedJob(value);
    setSelectedEmployee("");
    if (value) {
      fetchAllocatedEmployees(value);
    } else {
      setAllocatedEmployees([]);
    }
  };

  const handleEmployeeChange = (value: string) => {
    setSelectedEmployee(value);
  };

  return (
    <Layout className="min-h-screen flex justify-center items-center p-4">
      <Card className="responsive-card w-full max-w-[1200px]">
        <h2 className="page-title mb-4">Check In</h2>

        <Content className="flex flex-col gap-4">
          <div className="flex justify-between items-center gap-4 mb-4">
            <div className="flex gap-4 flex-1">
              <div className="flex-1 min-w-[70px]">
                <Select
                  placeholder="Job"
                  allowClear
                  showSearch={false}
                  value={selectedJob || undefined}
                  onChange={handleJobChange}
                  style={{ width: "100%" }}
                  loading={loading}
                >
                  {jobs.map(job => (
                    <Option key={job.id} value={job.id || ''}>
                      {job.jobName} ({job.purchaseOrderNumber})
                    </Option>
                  ))}
                </Select>
              </div>
              <div className="flex-1 min-w-[70px]">
                <Select
                  placeholder="Employee"
                  allowClear
                  showSearch={false}
                  value={selectedEmployee || undefined}
                  onChange={handleEmployeeChange}
                  style={{ width: "100%" }}
                  loading={loadingEmployees}
                  disabled={!selectedJob}
                >
                  {allocatedEmployees.map(employee => (
                    <Option key={employee.id} value={employee.id || ''}>
                      {employee.accountHolderName} ({employee.idNumber || 'No ID'})
                    </Option>
                  ))}
                </Select>
              </div>
            </div>
            <Button 
              type="primary" 
              onClick={handleCheckIn} 
              loading={submitting}
              disabled={!selectedJob || !selectedEmployee}
            >
              Check In
            </Button>
          </div>

          <Card title="Check-in History">
            <div className="responsive-table">
              <Table 
                columns={columns} 
                dataSource={checkInHistory} 
                pagination={{ 
                  pageSize: 8,
                  position: ['bottomCenter']
                }}
                scroll={{ x: 'max-content' }}
                size="middle"
                bordered
              />
            </div>
            <div className="mt-4 text-right">
              <Button onClick={() => navigate(-1)}>Back</Button>
            </div>
          </Card>
        </Content>
      </Card>
    </Layout>
  );
};

export default SiteManagerCheckIn; 