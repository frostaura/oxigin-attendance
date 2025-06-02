import React, { useEffect, useState } from "react";
import { Layout, Card, Button, Table, Checkbox, message } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobRequestsAsync as getJobsAsync, getJobsRequiringApprovalAsync } from "../services/data/jobRequests";
import type { Job } from "../models/jobModels";

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
  approved?: boolean;
  declined?: boolean;
}

const ClientHome: React.FC = () => {
  const navigate = useNavigate();
  const [jobsPendingApproval, setJobsPendingApproval] = useState<JobData[]>([]);

  const [upcomingJobData] = useState<JobData[]>([
    { key: "1", jobId: "91011", purchaseOrder: "PO1112", jobName: "Roof Repair", location: "NYC", date: "2025-04-01" },
    { key: "2", jobId: "13141", purchaseOrder: "PO1314", jobName: "Flooring", location: "LA", date: "2025-04-05" },
  ]);

  // Add state for jobs awaiting confirmation
  const [jobsAwaitingConfirmation, setJobsAwaitingConfirmation] = useState<Job[]>([]);

  const handleApproveChange = (record: JobData) => {
    const updatedJobs = jobsPendingApproval.map((job) =>
      job.key === record.key ? { ...job, approved: !job.approved, declined: false } : job
    );
    setJobsPendingApproval(updatedJobs);
  };

  const handleDeclineChange = (record: JobData) => {
    const updatedJobs = jobsPendingApproval.map((job) =>
      job.key === record.key ? { ...job, declined: !job.declined, approved: false } : job
    );
    setJobsPendingApproval(updatedJobs);
  };

  const processJobs = () => {
    const approvedJobs = jobsPendingApproval.filter((job) => job.approved);
    const declinedJobs = jobsPendingApproval.filter((job) => job.declined);
    console.log("Approved Jobs:", approvedJobs);
    console.log("Declined Jobs:", declinedJobs);
    setJobsPendingApproval(jobsPendingApproval.filter((job) => !job.approved && !job.declined));
  };

  const jobColumns: ColumnsType<JobData> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Location", dataIndex: "location", key: "location" },
    { title: "Date", dataIndex: "date", key: "date" },
    {
      title: "Approve",
      key: "approve",
      render: (_, record) => (
        <Checkbox checked={record.approved} onChange={() => handleApproveChange(record)} />
      ),
    },
    {
      title: "Decline",
      key: "decline",
      render: (_, record) => (
        <Checkbox checked={record.declined} onChange={() => handleDeclineChange(record)} />
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
  useEffect(() => {
    const fetchJobs = async () => {
      try {
        // Fetch jobs awaiting confirmation
        const allJobs: Array<Job> = await getJobsAsync();
        const awaitingConfirmation = allJobs.filter(job => !job.approved);
        setJobsAwaitingConfirmation(awaitingConfirmation);

        // Fetch jobs requiring approval
        const jobsRequiringApproval = await getJobsRequiringApprovalAsync();
        const formattedJobs: JobData[] = jobsRequiringApproval.map(job => ({
          key: job.id || '',
          jobId: job.id || '',
          purchaseOrder: job.purchaseOrderNumber,
          jobName: job.jobName,
          location: job.location,
          date: new Date(job.time).toLocaleDateString(),
          approved: false,
          declined: false
        }));
        setJobsPendingApproval(formattedJobs);

      } catch (error) {
        console.error("Error fetching jobs:", error);
        message.error('Failed to fetch jobs. Please try again later.');
      }
    };

    fetchJobs();
  }, []);

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Home Page</h2>
        </Header>

        <Content style={{ flex: 1, padding: 20, display: "flex", flexDirection: "column", gap: 20 }}>
          <Card title="Jobs Pending Confirmation">
            <Table 
              columns={[
                { title: "Purchase Order #", dataIndex: "purchaseOrderNumber", key: "purchaseOrderNumber" },
                { title: "Job Name", dataIndex: "jobName", key: "jobName" },
                { title: "Location", dataIndex: "location", key: "location" },
                { title: "Date", dataIndex: "time", key: "time", 
                  render: (time: Date) => new Date(time).toLocaleDateString() 
                },
                { title: "Workers Needed", dataIndex: "numWorkers", key: "numWorkers" },
                { title: "Hours Required", dataIndex: "numHours", key: "numHours" }
              ]} 
              dataSource={jobsAwaitingConfirmation} 
              rowKey="id"
              pagination={false} 
            />
          </Card>

          <Card title="Jobs Pending My Approval">
            <Table columns={jobColumns} dataSource={jobsPendingApproval} pagination={false} />
            <div style={{ textAlign: "right", marginTop: 20 }}>
              <Button
                type="primary"
                onClick={processJobs}
                disabled={!jobsPendingApproval.some((job) => job.approved || job.declined)}
              >
                Process Selected Jobs
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