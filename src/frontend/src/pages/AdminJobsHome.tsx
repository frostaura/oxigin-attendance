import React, { useState, useEffect } from "react";
import { Layout, Card, Button, Table, Checkbox, message } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobsAsync, getJobsRequiringApprovalAsync, approveJobAsync, rejectJobAsync } from "../services/data/job";
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

const AdminJobsHome: React.FC = () => {
  const navigate = useNavigate();
  const [jobsPendingApproval, setJobsPendingApproval] = useState<JobData[]>([]);
  const [jobsAwaitingConfirmation, setJobsAwaitingConfirmation] = useState<Job[]>([]);
  const [processing, setProcessing] = useState(false);

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

  const processJobs = async () => {
    try {
      setProcessing(true);
      const approvedJobs = jobsPendingApproval.filter((job) => job.approved);
      const declinedJobs = jobsPendingApproval.filter((job) => job.declined);

      // Process approved jobs
      for (const job of approvedJobs) {
        await approveJobAsync({ id: job.jobId } as Job);
      }

      // Process declined jobs
      for (const job of declinedJobs) {
        await rejectJobAsync({ id: job.jobId } as Job);
      }

      // Remove processed jobs from the list
      setJobsPendingApproval(jobsPendingApproval.filter((job) => !job.approved && !job.declined));
      message.success('Jobs processed successfully');
    } catch (error) {
      console.error('Error processing jobs:', error);
      message.error('Failed to process jobs. Please try again.');
    } finally {
      setProcessing(false);
    }
  };

  // Table columns
  const jobColumns: ColumnsType<JobData> = [
    { title: "Purchase Order #", dataIndex: "purchaseOrder", key: "purchaseOrder" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Location", dataIndex: "location", key: "location" },
    { title: "Date", dataIndex: "date", key: "date" },
  ];

  const approvalColumns: ColumnsType<JobData> = [
    ...jobColumns,
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
          requestor: job.requestorID,
          contact: '-', // This could be enhanced if we had user details
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
          <h2 style={{ margin: 0, textAlign: "center", width: "100%" }}>Job Home Page</h2>
        </Header>

        <Content style={{ flex: 1, padding: 20 }}>
          <div style={{ display: "flex", flexDirection: "column", gap: 20, width: "100%" }}>
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
              <Table columns={approvalColumns} dataSource={jobsPendingApproval} pagination={false} />
              <div style={{ textAlign: "right", marginTop: 20 }}>
                <Button
                  type="primary"
                  onClick={processJobs}
                  loading={processing}
                  disabled={!jobsPendingApproval.some((job) => job.approved || job.declined)}
                >
                  Process Selected Jobs
                </Button>
              </div>
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