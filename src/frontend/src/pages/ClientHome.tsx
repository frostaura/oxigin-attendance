import React, { useEffect, useState } from "react";
import { Layout, Card, Button, Table, Checkbox } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobRequestsAsync as getJobsAsync } from "../services/data/jobRequests";
import type { Job } from "../types";

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
  checked?: boolean;
}

const ClientHome: React.FC = () => {
  const navigate = useNavigate();
  const [jobsPendingApproval, setJobsPendingApproval] = useState<JobData[]>([
    { key: "1", jobId: "1234", purchaseOrder: "PO4567", jobName: "Fix Plumbing", requestor: "John Doe", contact: "555-1234", checked: false },
    { key: "2", jobId: "5678", purchaseOrder: "PO7890", jobName: "Install Wiring", requestor: "Jane Smith", contact: "555-5678", checked: false },
  ]);

  const [upcomingJobData] = useState<JobData[]>([
    { key: "1", jobId: "91011", purchaseOrder: "PO1112", jobName: "Roof Repair", location: "NYC", date: "2025-04-01" },
    { key: "2", jobId: "13141", purchaseOrder: "PO1314", jobName: "Flooring", location: "LA", date: "2025-04-05" },
  ]);

  const handleCheckboxChange = (record: JobData) => {
    const updatedJobs = jobsPendingApproval.map((job) =>
      job.key === record.key ? { ...job, checked: !job.checked } : job
    );
    setJobsPendingApproval(updatedJobs);
  };

  const approveJobs = () => {
    const approvedJobs = jobsPendingApproval.filter((job) => job.checked);
    console.log("Approved Jobs:", approvedJobs);
    setJobsPendingApproval(jobsPendingApproval.filter((job) => !job.checked));
  };

  const jobColumns: ColumnsType<JobData> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Requestor", dataIndex: "requestor", key: "requestor" },
    { title: "Contact", dataIndex: "contact", key: "contact" },
    {
      title: "Approve",
      key: "select",
      render: (_, record) => (
        <Checkbox checked={record.checked} onChange={() => handleCheckboxChange(record)} />
      ),
    },
  ];

  const upcomingJobColumns: ColumnsType<JobData> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Location", dataIndex: "location", key: "location" },
    { title: "Date", dataIndex: "date", key: "date" },
  ];

  // Fetch all data we need, and ensure the effect only runs once on mount ([]).
  const [jobs, setJobs] = useState<Array<JobData>>([]);
  useEffect(() => {
    setTimeout(async () => {
      const allJobs: Array<Job> = await getJobsAsync();
      
      // TODO: Map to JobData.

      // Set the job data (setJobs) and ensure you bind the table to (jobs)

      console.log("Fetched Jobs:", allJobs);

      debugger;
    });
  }, []);

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Home Page</h2>
        </Header>

        <Content style={{ flex: 1, padding: 20, display: "flex", flexDirection: "column", gap: 20 }}>
          <Card title="Jobs Pending Confirmation">
            <Table columns={jobColumns.filter((col) => col.key !== "select")} dataSource={jobsPendingApproval} pagination={false} />
          </Card>

          <Card title="Jobs Pending My Approval">
            <Table columns={jobColumns} dataSource={jobsPendingApproval} pagination={false} />
            <div style={{ textAlign: "right", marginTop: 20 }}>
              <Button
                type="primary"
                onClick={approveJobs}
                disabled={!jobsPendingApproval.some((job) => job.checked)}
              >
                Approve Selected Jobs
              </Button>
            </div>
          </Card>

          <Card title="Upcoming Jobs">
            <Table columns={upcomingJobColumns} dataSource={upcomingJobData} pagination={false} />
          </Card>
        </Content>

        <div style={{ display: "flex", justifyContent: "flex-end", gap: 10, marginTop: "auto" }}>
          <Button type="primary" onClick={() => navigate("/clientjobrequest")}>Request Job</Button>
        </div>
      </Card>
    </Layout>
  );
};

export default ClientHome;