import React from "react";
import { Layout, Card, Button, Table } from "antd";
import { useNavigate } from "react-router-dom";

const { Header, Content } = Layout;

const AdminJobsHome = () => {
  const navigate = useNavigate();

    // Table columns
    const jobColumns = [
        { title: "Job ID", dataIndex: "jobId", key: "jobId" },
        { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
        { title: "Job Name", dataIndex: "jobName", key: "jobName" },
        { title: "Requestor", dataIndex: "requestor", key: "requestor" },
        { title: "Contact", dataIndex: "contact", key: "contact" },
      ];
    
      const upcomingJobColumns = [
        { title: "Job ID", dataIndex: "jobId", key: "jobId" },
        { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
        { title: "Job Name", dataIndex: "jobName", key: "jobName" },
        { title: "Location", dataIndex: "location", key: "location" },
        { title: "Date", dataIndex: "date", key: "date" },
      ];
    
      // Sample data
      const jobData = [
        { key: "1", jobId: "1234", purchaseOrder: "PO4567", jobName: "Fix Plumbing", requestor: "John Doe", contact: "555-1234" },
        { key: "2", jobId: "5678", purchaseOrder: "PO7890", jobName: "Install Wiring", requestor: "Jane Smith", contact: "555-5678" },
      ];
    
      const upcomingJobData = [
        { key: "1", jobId: "91011", purchaseOrder: "PO1112", jobName: "Roof Repair", location: "NYC", date: "2025-04-01" },
        { key: "2", jobId: "13141", purchaseOrder: "PO1314", jobName: "Flooring", location: "LA", date: "2025-04-05" },
      ];
    


  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
    <Card style={{ width: "80%", padding: 20 }}>
     <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
    <h2 style={{ margin: 0, alignItems: "center"}}>Job Home Page</h2>
    </Header>

    <Content style={{ flex: 1, padding: 20 }}>
    <div style={{ display: "flex", flexDirection: "column", gap: 20, width: "100%" }}>
      <Card title="Jobs Pending Confirmation">
        <Table columns={jobColumns} dataSource={jobData} pagination={false} />
      </Card>

      <Card title="Jobs Pending My Approval">
        <Table columns={jobColumns} dataSource={jobData} pagination={false} />
      </Card>

      <Card title="Upcoming Jobs">
        <Table columns={upcomingJobColumns} dataSource={upcomingJobData} pagination={false} />
      </Card>
    </div>
  </Content>

      {/* Bottom Right Buttons */}
      <div style={{ display: "flex", justifyContent: "flex-end", gap: 10, marginTop: "auto" }}>
        <Button type="primary" onClick={() => navigate("/adminjobrequest")}>Submit Job Request</Button>
      </div>
    </Card>
  </Layout>
  );
};

export default AdminJobsHome;
