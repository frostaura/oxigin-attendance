import React from "react";
import { Layout, Card, Button, Table } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";

const { Header, Content } = Layout;

interface JobData {
  key: string;
  jobId: string;
  purchaseOrder: string;
  jobName: string;
  requestor?: string;
  contact?: string;
  location?: string;
  date?: string;
}

const AdminClientHome: React.FC = () => {
  const navigate = useNavigate();

  // Table columns
  const jobColumns: ColumnsType<JobData> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Requestor", dataIndex: "requestor", key: "requestor" },
    { title: "Contact", dataIndex: "contact", key: "contact" },
  ];

  const upcomingJobColumns: ColumnsType<JobData> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Location", dataIndex: "location", key: "location" },
    { title: "Date", dataIndex: "date", key: "date" },
  ];

  // Sample data
  const jobData: JobData[] = [
    { key: "1", jobId: "1234", purchaseOrder: "PO4567", jobName: "Fix Plumbing", requestor: "John Doe", contact: "555-1234" },
    { key: "2", jobId: "5678", purchaseOrder: "PO7890", jobName: "Install Wiring", requestor: "Jane Smith", contact: "555-5678" },
  ];

  const upcomingJobData: JobData[] = [
    { key: "1", jobId: "91011", purchaseOrder: "PO1112", jobName: "Roof Repair", location: "NYC", date: "2025-04-01" },
    { key: "2", jobId: "13141", purchaseOrder: "PO1314", jobName: "Flooring", location: "LA", date: "2025-04-05" },
  ];

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20, position: "relative" }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Client Home Page</h2>
        </Header>

        <Content style={{ flex: 1, padding: 20, display: "flex", gap: 20, alignItems: "stretch" }}>
          <div style={{ display: "flex", width: "100%", gap: 20 }}>
            <div style={{ display: "flex", flexDirection: "column", gap: 20, flex: 1 }}>
              <Card title="Jobs Pending Client Confirmation" style={{ flex: 1 }}>
                <Table columns={jobColumns} dataSource={jobData} pagination={false} />
              </Card>
              <Card title="Jobs Pending My Approval" style={{ flex: 1 }}>
                <Table columns={jobColumns} dataSource={jobData} pagination={false} />
              </Card>
              <Card title="Upcoming Jobs" style={{ flex: 1 }}>
                <Table columns={upcomingJobColumns} dataSource={upcomingJobData} pagination={false} />
              </Card>
            </div>
          </div>
        </Content>

        {/* Align the button with the tables */}
        <div style={{
          position: "absolute", 
          bottom: 20, 
          right: 20, 
          width: "calc(100% - 40px)", 
          display: "flex", 
          justifyContent: "flex-end"
        }}>
          <Button type="primary" onClick={() => navigate("/adminmanageclients")}>Manage Clients</Button>
        </div>
      </Card>
    </Layout>
  );
};

export default AdminClientHome; 