import React, { useState, useEffect } from "react";
import { Layout, Card, Button, Table, Checkbox, message } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getJobsAsync, getJobsRequiringApprovalAsync, updateJobApprovalAsync } from "../services/data/job";
import type { Job } from "../models/jobModels";

const { Content } = Layout;

const AdminJobsHome: React.FC = () => {
  const navigate = useNavigate();
  const [jobsPendingApproval, setJobsPendingApproval] = useState<Job[]>([]);
  const [jobsAwaitingConfirmation, setJobsAwaitingConfirmation] = useState<Job[]>([]);
  const [processing, setProcessing] = useState(false);

  const handleApproveChange = (record: Job) => {
    const updatedJobs = jobsPendingApproval.map((job) =>
      job.id === record.id ? { ...job, approved: true } : job
    );
    setJobsPendingApproval(updatedJobs);
  };

  const handleRejectChange = (record: Job) => {
    const updatedJobs = jobsPendingApproval.map((job) =>
      job.id === record.id ? { ...job, approved: false } : job
    );
    setJobsPendingApproval(updatedJobs);
  };

  const processJobs = async () => {
    try {
      setProcessing(true);
      const jobsToProcess = jobsPendingApproval.filter((job) => job.approved !== undefined);

      // Process all jobs
      for (const job of jobsToProcess) {
        if (job.id) {
          await updateJobApprovalAsync(job.id, job.approved || false);
        }
      }

      // Remove processed jobs from the list
      setJobsPendingApproval(jobsPendingApproval.filter((job) => job.approved === undefined));
      message.success('Jobs processed successfully');
    } catch (error) {
      console.error('Error processing jobs:', error);
      message.error('Failed to process jobs. Please try again.');
    } finally {
      setProcessing(false);
    }
  };

  // Table columns
  const jobColumns: ColumnsType<Job> = [
    { title: "Purchase Order #", dataIndex: "purchaseOrderNumber", key: "purchaseOrderNumber" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Location", dataIndex: "location", key: "location" },
    { 
      title: "Date", 
      dataIndex: "time", 
      key: "time",
      render: (time: Date) => new Date(time).toLocaleDateString()
    },
  ];

  const approvalColumns: ColumnsType<Job> = [
    ...jobColumns,
    {
      title: "Approve",
      key: "approve",
      render: (_, record) => (
        <Checkbox 
          checked={record.approved === true} 
          onChange={() => handleApproveChange(record)}
        />
      ),
    },
    {
      title: "Reject",
      key: "reject",
      render: (_, record) => (
        <Checkbox 
          checked={record.approved === false} 
          onChange={() => handleRejectChange(record)}
        />
      ),
    },
  ];

  // Fetch all data we need
  useEffect(() => {
    const fetchJobs = async () => {
      try {
        // Fetch jobs awaiting confirmation
        const allJobs = await getJobsAsync();
        const awaitingConfirmation = allJobs.filter(job => !job.approved);
        setJobsAwaitingConfirmation(awaitingConfirmation);

        // Fetch jobs requiring approval
        const jobsRequiringApproval = await getJobsRequiringApprovalAsync();
        setJobsPendingApproval(jobsRequiringApproval);
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
        <h2 className="page-title mb-4">Job Home Page</h2>

        <Content className="flex flex-col gap-4">
          <Card title="Jobs Pending Confirmation">
            <div className="responsive-table">
              <Table 
                columns={jobColumns} 
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
                columns={approvalColumns} 
                dataSource={jobsPendingApproval}
                rowKey="id"
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
                  loading={processing}
                  disabled={!jobsPendingApproval.some((job) => job.approved !== undefined)}
                >
                  Process Selected Jobs
                </Button>
              </div>
            </div>
          </Card>
        </Content>

        <div className="flex justify-end gap-2 mt-4">
          <Button type="primary" onClick={() => navigate("/adminjobrequest")}>Submit Job Request</Button>
        </div>
      </Card>
    </Layout>
  );
};

export default AdminJobsHome; 