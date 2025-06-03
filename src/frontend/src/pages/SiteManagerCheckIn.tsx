import React, { useState, useEffect } from "react";
import { Layout, Card, Button, Table, Select, message } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobsAsync } from "../services/data/job";
import type { Job } from "../models/jobModels";

const { Header, Content } = Layout;
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
  const [selectedJob, setSelectedJob] = useState<string>("Job ID");
  const [selectedEmployee, setSelectedEmployee] = useState<string>("Employee ID");
  const [jobs, setJobs] = useState<Job[]>([]);
  const [loading, setLoading] = useState(false);
  const [checkInHistory, setCheckInHistory] = useState<CheckInRecord[]>([
    { key: "1", jobId: "J001", employeeId: "E001", employeeName: "John Doe", timeIn: "2025-03-20 08:00" },
    { key: "2", jobId: "J002", employeeId: "E002", employeeName: "Jane Smith", timeIn: "2025-03-20 08:30" },
  ]);

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

  const handleCheckIn = () => {
    const newCheckIn: CheckInRecord = {
      key: `${checkInHistory.length + 1}`,
      jobId: selectedJob,
      employeeId: selectedEmployee,
      employeeName: "John Doe", // This can be dynamic if needed
      timeIn: new Date().toLocaleString(),
    };
    setCheckInHistory([...checkInHistory, newCheckIn]);
    setSelectedJob("Job ID");
    setSelectedEmployee("Employee ID");
  };

  const columns: ColumnsType<CheckInRecord> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Employee ID", dataIndex: "employeeId", key: "employeeId" },
    { title: "Employee Name", dataIndex: "employeeName", key: "employeeName" },
    { title: "Time In", dataIndex: "timeIn", key: "timeIn" },
  ];

  const handleJobChange = (value: string) => {
    setSelectedJob(value);
  };

  const handleEmployeeChange = (value: string) => {
    setSelectedEmployee(value);
  };

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        {/* Page Header */}
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Check In Page</h2>
        </Header>

        {/* Main Content */}
        <Content style={{ flex: 1, padding: 20 }}>
          {/* Job ID and Employee ID inputs with Check In button */}
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
            <div style={{ display: "flex", gap: 10, flex: 1 }}>
              <div style={{ flex: 1, minWidth: "70px" }}>
                <Select
                  value={selectedJob}
                  onChange={handleJobChange}
                  style={{ width: "100%", marginBottom: "20px" }}
                  loading={loading}
                >
                  <Option value="Job ID" disabled>Select Job</Option>
                  {jobs.map(job => (
                    <Option key={job.id} value={job.id || ''}>
                      {job.jobName} ({job.purchaseOrderNumber})
                    </Option>
                  ))}
                </Select>
              </div>
              <div style={{ flex: 1, minWidth: "70px" }}>
                <Select
                  value={selectedEmployee}
                  onChange={handleEmployeeChange}
                  style={{ width: "100%", marginBottom: "20px" }}
                >
                  <Option value="Employee ID">Select Employee</Option>
                  <Option value="E001">Employee 1</Option>
                  <Option value="E002">Employee 2</Option>
                </Select>
              </div>
            </div>
            <Button type="primary" onClick={handleCheckIn} style={{ marginLeft: "20px" }}>Check In</Button>
          </div>

          {/* Check-in History Table */}
          <Card title="Check-in History">
            <Table columns={columns} dataSource={checkInHistory} pagination={false} />
            <div style={{ marginTop: 20, display: "flex", justifyContent: "flex-end" }}>
              <Button onClick={() => navigate(-1)}>Back</Button>
            </div>
          </Card>
        </Content>
      </Card>
    </Layout>
  );
};

export default SiteManagerCheckIn; 