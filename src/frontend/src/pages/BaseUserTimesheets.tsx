import React, { useState } from "react";
import { Layout, Card, Input, Table, Checkbox, Button } from "antd";

const { Header, Content } = Layout;

interface Job {
  key: string;
  jobId: string;
  purchaseOrderNo: string;
  jobName: string;
  companyName: string;
  checked: boolean;
}

interface Timesheet {
  key: string;
  date: string;
  employeeId: string;
  timeIn: string;
  timeOut: string;
  hoursWorked: string;
}

const BaseUserTimesheets: React.FC = () => {
  const [jobDate, setJobDate] = useState<string | null>(null);
  const [jobsOnDate, setJobsOnDate] = useState<Job[]>([
    { key: "1", jobId: "J001", purchaseOrderNo: "PO001", jobName: "Job A", companyName: "Company 1", checked: false },
    { key: "2", jobId: "J002", purchaseOrderNo: "PO002", jobName: "Job B", companyName: "Company 2", checked: false },
  ]);
  const [timesheetData, setTimesheetData] = useState<Timesheet[]>([
    { key: "1", date: "2025-03-20", employeeId: "E001", timeIn: "08:00", timeOut: "16:00", hoursWorked: "8" },
    { key: "2", date: "2025-03-20", employeeId: "E002", timeIn: "09:00", timeOut: "17:00", hoursWorked: "8" },
  ]);

  const handleJobDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setJobDate(e.target.value);
  };

  const handleCheckboxChange = (record: Job) => {
    const updatedJobs = jobsOnDate.map((job) =>
      job.key === record.key ? { ...job, checked: !job.checked } : { ...job, checked: false }
    );
    setJobsOnDate(updatedJobs);
  };

  const columnsJobsOnDate = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Purchase Order Number", dataIndex: "purchaseOrderNo", key: "purchaseOrderNo" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Company Name", dataIndex: "companyName", key: "companyName" },
    {
      title: "Select",
      key: "select",
      render: (_: any, record: Job) => (
        <Checkbox checked={record.checked} onChange={() => handleCheckboxChange(record)} />
      ),
    },
  ];

  const columnsTimesheet = [
    { title: "Date", dataIndex: "date", key: "date" },
    { title: "Employee ID", dataIndex: "employeeId", key: "employeeId" },
    { title: "Time In", dataIndex: "timeIn", key: "timeIn" },
    { title: "Time Out", dataIndex: "timeOut", key: "timeOut" },
    { title: "Hours Worked", dataIndex: "hoursWorked", key: "hoursWorked" },
  ];

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        {/* Page Header */}
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Base User Timesheets</h2>
        </Header>

        {/* Main Content */}
        <Content style={{ flex: 1, padding: 20 }}>
        <div style={{ marginBottom: 20 }}>
          <label style={{ display: "block", fontWeight: "bold", marginBottom: 5, fontSize: "16px" }}>
            Job Date:
          </label>
          <Input type="date" value={jobDate || ''} onChange={handleJobDateChange} style={{ width: "100%", padding: 8 }} />
        </div>


          {/* Tables Container */}
          <Card style={{ display: "flex", justifyContent: "space-between", gap: "20px", marginTop: 20 }}>
            {/* Left Side Table - Jobs on Date */}
            <div style={{ flex: 1 }}>
              <Card title="Jobs on Date">
                <Table
                  columns={columnsJobsOnDate}
                  dataSource={jobsOnDate}
                  pagination={false}
                  rowKey="key"
                  scroll={{ y: 300 }}  // Allows for scrolling if there are too many rows
                />
              </Card>
            </div>

            {/* Right Side Table - Timesheet */}
            <div style={{ flex: 1 }}>
              <Card title="Timesheet">
                <Table
                  columns={columnsTimesheet}
                  dataSource={timesheetData}
                  pagination={false}
                  rowKey="key"
                  scroll={{ y: 300 }}  // Allows for scrolling if there are too many rows
                />
              </Card>
            </div>
          </Card>

          {/* Print Button */}
          <div style={{ display: "flex", justifyContent: "flex-end", marginTop: 20 }}>
            <Button type="primary">Print</Button>
          </div>
        </Content>
      </Card>
    </Layout>
  );
};

export default BaseUserTimesheets;
