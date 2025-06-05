import React, { useEffect, useState } from "react";
import { Layout, Card, Button, Table, Checkbox, message } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobsAsync as getJobsAsync, getJobsRequiringApprovalAsync } from "../services/data/job";
import type { Job } from "../models/jobModels";
import { GetLoggedInUserContextAsync } from "../services/data/backend";

const { Content } = Layout;

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
  const [upcomingJobData, setUpcomingJobData] = useState<JobData[]>([]);
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

  // Fetch all data we need, and ensure the effect only runs once on mount ([]).
  useEffect(() => {
    const fetchJobs = async () => {
      try {
        // Get the current user context to get the client ID
        const userContext = await GetLoggedInUserContextAsync();
        if (!userContext?.user?.clientID) {
          message.error('Unable to fetch client information');
          return;
        }

        const clientId = userContext.user.clientID;

        // Fetch jobs awaiting confirmation and filter by client ID
        const allJobs: Array<Job> = await getJobsAsync();
        const clientJobs = allJobs.filter(job => job.clientID === clientId);
        const awaitingConfirmation = clientJobs.filter(job => !job.approved);
        setJobsAwaitingConfirmation(awaitingConfirmation);

        // Set upcoming jobs (approved jobs for this client)
        const upcomingJobs = clientJobs
          .filter(job => job.approved)
          .map(job => ({
            key: job.id || '',
            jobId: job.id || '',
            purchaseOrder: job.purchaseOrderNumber,
            jobName: job.jobName,
            location: job.location,
            date: new Date(job.time).toLocaleDateString()
          }));
        setUpcomingJobData(upcomingJobs);

        // Fetch jobs requiring approval and filter by client ID
        const jobsRequiringApproval = await getJobsRequiringApprovalAsync();
        const clientJobsRequiringApproval = jobsRequiringApproval.filter(job => job.clientID === clientId);
        const formattedJobs: JobData[] = clientJobsRequiringApproval.map(job => ({
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
    <Layout className="min-h-screen flex justify-center items-center p-4">
      <Card className="responsive-card w-full max-w-[1200px]">
        <h2 className="page-title mb-4">Home Page</h2>

        <Content className="flex flex-col gap-4">
          <Card title="Jobs Pending Confirmation">
            <div className="responsive-table">
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
                pagination={{ 
                  pageSize: 8,
                  position: ['bottomCenter']
                }}
                scroll={{ x: 'max-content' }}
                size="middle"
                bordered
              />
            </div>
          </Card>

          <Card title="Jobs Pending My Approval">
            <div className="responsive-table">
              <Table 
                columns={jobColumns} 
                dataSource={jobsPendingApproval} 
                pagination={{ 
                  pageSize: 8,
                  position: ['bottomCenter']
                }}
                scroll={{ x: 'max-content' }}
                size="middle"
                bordered
              />
              <div className="mt-4 text-right">
                <Button
                  type="primary"
                  onClick={processJobs}
                  disabled={!jobsPendingApproval.some((job) => job.approved || job.declined)}
                >
                  Process Selected Jobs
                </Button>
              </div>
            </div>
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