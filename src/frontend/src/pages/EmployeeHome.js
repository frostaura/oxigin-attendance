import React, { useState } from "react";
import { Layout, Card, Table, Select, Button, Checkbox, Input } from "antd";
import { useNavigate } from "react-router-dom";

const { Header, Content } = Layout;
const { Option } = Select;

const EmployeeHome = () => {
  const navigate = useNavigate();
  const [selectedJob, setSelectedJob] = useState("Check in");
  const [jobsAwaitingConfirmation, setJobsAwaitingConfirmation] = useState([
    {
      key: "1",
      jobName: "Fix Plumbing",
      location: "NYC",
      date: "2025-04-01",
      time: "10:00 AM",
      workerType: "Plumber",
      checked: false,
    },
    {
      key: "2",
      jobName: "Install Wiring",
      location: "LA",
      date: "2025-04-02",
      time: "11:00 AM",
      workerType: "Electrician",
      checked: false,
    },
  ]);

  const [upcomingJobs, setUpcomingJobs] = useState([
    { key: "1", jobName: "Roof Repair", location: "NYC", date: "2025-04-03", time: "9:00 AM", workerType: "Roofer" },
    { key: "2", jobName: "Flooring", location: "LA", date: "2025-04-05", time: "1:00 PM", workerType: "Floor Specialist" },
  ]);

  const handleJobChange = (value) => {
    setSelectedJob(value);
  };

  const handleCheckboxChange = (record) => {
    const updatedJobs = jobsAwaitingConfirmation.map((job) =>
      job.key === record.key ? { ...job, checked: !job.checked } : job
    );
    setJobsAwaitingConfirmation(updatedJobs);
  };

  const approveJob = () => {
    const approvedJobs = jobsAwaitingConfirmation.filter((job) => job.checked);
    setUpcomingJobs([...upcomingJobs, ...approvedJobs]);
    setJobsAwaitingConfirmation(jobsAwaitingConfirmation.filter((job) => !job.checked));
  };

  const jobColumns = [
    {
      title: "Job Name",
      dataIndex: "jobName",
      key: "jobName",
    },
    {
      title: "Job Location",
      dataIndex: "location",
      key: "location",
    },
    {
      title: "Date",
      dataIndex: "date",
      key: "date",
    },
    {
      title: "Time",
      dataIndex: "time",
      key: "time",
    },
    {
      title: "Worker Type",
      dataIndex: "workerType",
      key: "workerType",
    },
    {
      title: "Select",
      key: "select",
      render: (_, record) => (
        <Checkbox checked={record.checked} onChange={() => handleCheckboxChange(record)} />
      ),
    },
  ];

  const upcomingJobColumns = [
    {
      title: "Job Name",
      dataIndex: "jobName",
      key: "jobName",
    },
    {
      title: "Job Location",
      dataIndex: "location",
      key: "location",
    },
    {
      title: "Date",
      dataIndex: "date",
      key: "date",
    },
    {
      title: "Time",
      dataIndex: "time",
      key: "time",
    },
    {
      title: "Worker Type",
      dataIndex: "workerType",
      key: "workerType",
    },
  ];

  return (
    <Layout style={{ minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: "100%", padding: "20px" }}>
      <Header style={{ textAlign: "center", marginBottom: "20px", background: "transparent" }}>
  <h2>Home Page</h2>
</Header>


        <Content>
        <div style={{ width: "100%", marginBottom: "20px" }}>
            <Select
              value={selectedJob}
              onChange={handleJobChange}
              style={{ width: "100%", marginBottom: "20px" }}
            >
              <Option value="Check in">Check in</Option>
              <Option value="Job 1">Job 1</Option>
              <Option value="Job 2">Job 2</Option>
            </Select>

            <Card title="Jobs Awaiting Confirmation" style={{ marginBottom: "20px" }}>
              <Table
                columns={jobColumns}
                dataSource={jobsAwaitingConfirmation}
                rowKey="key"
                pagination={false}
              />
            </Card>

            <div style={{ width: "100%", textAlign: "right", marginBottom: "20px" }}>
            <Button
              type="primary"
              onClick={approveJob}
              disabled={!jobsAwaitingConfirmation.some((job) => job.checked)}
            >
              Approve Selected Jobs
            </Button>
          </div>
          </div>

          <div style={{ width: "100%", marginBottom: "20px" }}>
            <Card title="Upcoming Jobs" style={{ height: "100%" }}>
              <Table
                columns={upcomingJobColumns}
                dataSource={upcomingJobs}
                rowKey="key"
                pagination={false}
                style={{ height: "100%" }}
              />
            </Card>
          </div>
        </Content>
      </Card>
    </Layout>
  );
};

export default EmployeeHome;