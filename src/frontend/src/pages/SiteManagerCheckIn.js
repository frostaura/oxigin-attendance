import React, { useState } from "react";
import { Layout, Card, Input, Button, Table, Select } from "antd";
import { useNavigate } from "react-router-dom";
import checkInHistory from "../models/CheckInHistory";

const { Header, Content } = Layout;
const {Option} = Select;

const SiteManagerCheckIn = () => {
  const navigate = useNavigate();
  const [selectedJob, setSelectedJob] = useState("Job ID");
  const [selectedEmployee, setSelectedEmployee] = useState("Employee ID");
  const [jobId, setJobId] = useState("");
  const [employeeId, setEmployeeId] = useState("");

  const handleCheckIn = () => {
    const newCheckIn = {
      key: `${checkInHistory.length + 1}`,
      jobId: jobId,
      employeeId: employeeId,
      employeeName: "John Doe", // This can be dynamic if needed
      timeIn: new Date().toLocaleString(),
    };
    setCheckInHistory([...checkInHistory, newCheckIn]);
    setJobId("");
    setEmployeeId("");
  };

  const columns = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Employee ID", dataIndex: "employeeId", key: "employeeId" },
    { title: "Employee Name", dataIndex: "employeeName", key: "employeeName" },
    { title: "Time In", dataIndex: "timeIn", key: "timeIn" },
  ];
  const handleJobChange = (value) => {
    setSelectedJob(value);
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
            >
              <Option value="Job 1">Job 1</Option>
              <Option value="Job 2">Job 2</Option>
            </Select>
              </div>
              <div style={{ flex: 1, minWidth: "70px" }}>
              <Select
              value={selectedEmployee}
              onChange={handleJobChange}
              style={{ width: "100%", marginBottom: "20px" }}
            >
              <Option value="E001">Job 1</Option>
              <Option value="E002">Job 2</Option>
            </Select>
              </div>
            </div>
            <Button type="primary" onClick={handleCheckIn} style={{ marginLeft: "20px" }}>Check In</Button>
          </div>

          {/* Check-in History Table */}
          <Card title="Check-in History">
            <Table columns={columns} dataSource={checkInHistory} pagination={false} />
          </Card>
        </Content>
      </Card>
    </Layout>
  );
};

export default SiteManagerCheckIn;
