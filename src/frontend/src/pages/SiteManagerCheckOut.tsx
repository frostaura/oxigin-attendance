import React, { useState, useEffect } from "react";
import { Layout, Card, Button, Table, Select, message } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobsAsync } from "../services/data/job";
import { getTimesheetsForJobAsync, signOutAsync } from "../services/data/timesheet";
import type { Job } from "../models/jobModels";
import type { Timesheet } from "../models/timesheetModels";

const { Header, Content } = Layout;
const { Option } = Select;

interface CheckOutRecord {
  key: string;
  jobId: string;
  employeeId: string;
  employeeName: string;
  timeOut: string;
}

const SiteManagerCheckOut: React.FC = () => {
  const navigate = useNavigate();
  const [selectedJob, setSelectedJob] = useState<string>("");
  const [selectedEmployee, setSelectedEmployee] = useState<string>("");
  const [jobs, setJobs] = useState<Job[]>([]);
  const [checkedInEmployees, setCheckedInEmployees] = useState<Timesheet[]>([]);
  const [loading, setLoading] = useState(false);
  const [loadingEmployees, setLoadingEmployees] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [checkOutHistory, setCheckOutHistory] = useState<CheckOutRecord[]>([]);

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

  const fetchCheckedInEmployees = async (jobId: string) => {
    try {
      setLoadingEmployees(true);
      const timesheets = await getTimesheetsForJobAsync(jobId);
      
      // Filter for employees who are checked in but not checked out for this job
      // Also exclude any employees who appear in the checkOutHistory
      const checkedOutEmployeeIds = new Set(checkOutHistory.map(record => record.employeeId));
      
      const checkedInTimesheets = timesheets.filter(timesheet => {
        const matchesJob = timesheet.jobID === jobId;
        const notDeleted = !timesheet.deleted;
        const hasTimeIn = Boolean(timesheet.timeIn);
        const notCheckedOut = timesheet.timeOut === "0001-01-01T00:00:00";
        const hasEmployee = Boolean(timesheet.employee);
        const notInHistory = !checkedOutEmployeeIds.has(timesheet.employeeID);
        
        return matchesJob && notDeleted && hasTimeIn && notCheckedOut && hasEmployee && notInHistory;
      });

      // Remove duplicate entries for the same employee
      const uniqueEmployees = checkedInTimesheets.reduce((acc, current) => {
        // Keep the most recent timesheet for each employee
        const existing = acc.find(t => t.employeeID === current.employeeID);
        if (!existing || new Date(current.timeIn) > new Date(existing.timeIn)) {
          const filtered = acc.filter(t => t.employeeID !== current.employeeID);
          return [...filtered, current];
        }
        return acc;
      }, [] as Timesheet[]);
      
      setCheckedInEmployees(uniqueEmployees);
    } catch (error) {
      console.error("Failed to fetch checked-in employees:", error);
      message.error("Failed to fetch checked-in employees");
    } finally {
      setLoadingEmployees(false);
    }
  };

  const handleCheckOut = async () => {
    if (!selectedJob || !selectedEmployee) {
      message.error("Please select both a job and an employee");
      return;
    }

    try {
      setSubmitting(true);
      const selectedTimesheet = checkedInEmployees.find(
        timesheet => timesheet.employeeID === selectedEmployee
      );

      if (!selectedTimesheet?.id) {
        message.error("No active check-in found for this employee");
        return;
      }

      // Submit the check-out
      const response = await signOutAsync(selectedTimesheet.id, new Date().toISOString());
      
      if (response) {
        // Add to check-out history
        const newCheckOut: CheckOutRecord = {
          key: selectedTimesheet.id,
          jobId: selectedJob,
          employeeId: selectedEmployee,
          employeeName: selectedTimesheet.employee?.accountHolderName || "Unknown",
          timeOut: new Date().toLocaleString(),
        };
        setCheckOutHistory(prev => [...prev, newCheckOut]);

        // Reset selections and refresh the employee list
        setSelectedEmployee("");
        await fetchCheckedInEmployees(selectedJob);
        message.success("Successfully checked out employee!");
      }
    } catch (error) {
      console.error("Failed to check out:", error);
      message.error("Failed to check out employee. Please try again.");
    } finally {
      setSubmitting(false);
    }
  };

  const columns: ColumnsType<CheckOutRecord> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Employee ID", dataIndex: "employeeId", key: "employeeId" },
    { title: "Employee Name", dataIndex: "employeeName", key: "employeeName" },
    { title: "Time Out", dataIndex: "timeOut", key: "timeOut" },
  ];

  const handleJobChange = (value: string) => {
    setSelectedJob(value);
    setSelectedEmployee("");
    if (value) {
      fetchCheckedInEmployees(value);
    } else {
      setCheckedInEmployees([]);
    }
  };

  const handleEmployeeChange = (value: string) => {
    setSelectedEmployee(value);
  };

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        {/* Page Header */}
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Check Out Page</h2>
        </Header>

        {/* Main Content */}
        <Content style={{ flex: 1, padding: 20 }}>
          {/* Job ID and Employee ID inputs with Check Out button */}
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
            <div style={{ display: "flex", gap: 10, flex: 1 }}>
              <div style={{ flex: 1, minWidth: "70px" }}>
                <Select
                  placeholder="Job"
                  allowClear
                  showSearch={false}
                  value={selectedJob || undefined}
                  onChange={handleJobChange}
                  style={{ width: "100%", marginBottom: "20px" }}
                  loading={loading}
                >
                  {jobs.map(job => (
                    <Option key={job.id} value={job.id || ''}>
                      {job.jobName} ({job.purchaseOrderNumber})
                    </Option>
                  ))}
                </Select>
              </div>
              <div style={{ flex: 1, minWidth: "70px" }}>
                <Select
                  placeholder="Employee"
                  allowClear
                  showSearch={false}
                  value={selectedEmployee || undefined}
                  onChange={handleEmployeeChange}
                  style={{ width: "100%", marginBottom: "20px" }}
                  loading={loadingEmployees}
                  disabled={!selectedJob}
                >
                  {checkedInEmployees.map(timesheet => (
                    <Option key={timesheet.id} value={timesheet.employeeID}>
                      {timesheet.employee?.accountHolderName} ({timesheet.employee?.idNumber || 'No ID'})
                    </Option>
                  ))}
                </Select>
              </div>
            </div>
            <Button 
              type="primary" 
              onClick={handleCheckOut} 
              style={{ marginLeft: "20px" }}
              loading={submitting}
              disabled={!selectedJob || !selectedEmployee}
            >
              Check Out
            </Button>
          </div>

          {/* Check-out History Table */}
          <Card title="Check-out History">
            <Table columns={columns} dataSource={checkOutHistory} pagination={false} />
            <div style={{ marginTop: 20, display: "flex", justifyContent: "flex-end" }}>
              <Button onClick={() => navigate(-1)}>Back</Button>
            </div>
          </Card>
        </Content>
      </Card>
    </Layout>
  );
};

export default SiteManagerCheckOut; 