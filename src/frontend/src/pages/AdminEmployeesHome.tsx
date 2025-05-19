import React from "react";
import { Layout, Table, Button, Card } from "antd";
import { useNavigate } from "react-router-dom";

const { Header, Content } = Layout;

interface Employee {
  key: number;
  id: string;
  name: string;
  contact: string;
}

interface Attendance {
  key: number;
  jobId: string;
  jobName: string;
  company: string;
  date: string;
}

const AdminEmployeesHome: React.FC = () => {
  const navigate = useNavigate();

  // Sample data for available employees
  const employeeData: Employee[] = [
    { key: 1, id: "E001", name: "John Doe", contact: "123-456-7890" },
    { key: 2, id: "E002", name: "Jane Smith", contact: "987-654-3210" },
  ];

  // Employee Table Columns
  const employeeColumns = [
    { title: "Employee ID", dataIndex: "id", key: "id" },
    { title: "Employee Name", dataIndex: "name", key: "name" },
    { title: "Contact Number", dataIndex: "contact", key: "contact" },
  ];

  // Sample data for attendance registers
  const attendanceData: Attendance[] = [
    { key: 1, jobId: "J1001", jobName: "Construction Work", company: "XYZ Ltd.", date: "2025-03-20" },
    { key: 2, jobId: "J1002", jobName: "Plumbing Task", company: "ABC Corp.", date: "2025-03-21" },
  ];

  // Attendance Table Columns
  const attendanceColumns = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Company", dataIndex: "company", key: "company" },
    { title: "Date", dataIndex: "date", key: "date" },
  ];

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
      <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
        <h2 style={{ margin: 0, alignItems: "center"}}>Employee Home Page</h2>
        </Header>
      <Content style={{ display: "flex", flexDirection: "column", gap: 20 }}>
        {/* Employees Table */}
        <Card title="Available Employees" style={{ width: "100%" }}>
          <Table columns={employeeColumns} dataSource={employeeData} pagination={false} />
        </Card>

        {/* Attendance Table */}
        <Card title="Attendance Registers" style={{ width: "100%" }}>
          <Table columns={attendanceColumns} dataSource={attendanceData} pagination={false} />
        </Card>
      </Content>


        {/* Bottom Buttons */}
        <div style={{ display: "flex", justifyContent: "center", gap: 10, marginTop: 20 }}>
          <Button type="primary" onClick={() => navigate("/adminmanageemployees")}>Manage Employees</Button>
          <Button type="primary" onClick={() => navigate("/adminjoballocations")}>Allocate Jobs</Button>
        </div>
      </Card>
    </Layout>
  );
};

export default AdminEmployeesHome;
