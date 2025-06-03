import React, { useState, useEffect } from "react";
import { Layout, Card, Table, Select, Button, message, Modal } from "antd";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getAllocationsForEmployeeAsync } from "../services/data/jobAllocation";
import type { Allocation } from "../models/jobAllocationModels";
import { GetLoggedInUserContextAsync } from "../services/data/backend";
import { signInAsync } from "../services/data/timesheet";
import type { Timesheet } from "../models/timesheetModels";

const { Header, Content } = Layout;
const { Option } = Select;

const EmployeeHome: React.FC = () => {
  const navigate = useNavigate();
  const [selectedJob, setSelectedJob] = useState<string>("Check in");
  const [loading, setLoading] = useState(false);
  const [allocations, setAllocations] = useState<Allocation[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [selectedAllocation, setSelectedAllocation] = useState<Allocation | null>(null);

  useEffect(() => {
    const fetchAllocations = async () => {
      setLoading(true);
      try {
        const userContext = await GetLoggedInUserContextAsync();
        if (!userContext?.user?.employeeID) {
          message.error("No employee ID found. Please contact support.");
          return;
        }
        const allocationData = await getAllocationsForEmployeeAsync(userContext.user.employeeID);
        setAllocations(allocationData);
      } catch (error) {
        console.error("Failed to fetch allocations:", error);
        message.error("Failed to fetch job allocations");
      } finally {
        setLoading(false);
      }
    };

    fetchAllocations();
  }, []);

  const handleJobChange = (value: string) => {
    console.log("Job selected:", value);
    if (value === "Check in") {
      return;
    }
    
    const allocation = allocations.find(a => a.job?.jobName === value);
    console.log("Found allocation:", allocation);
    
    setSelectedJob(value);
    setSelectedAllocation(allocation || null);
    
    if (allocation) {
      setIsModalVisible(true);
    }
  };

  const handleModalCancel = () => {
    setIsModalVisible(false);
    setSelectedAllocation(null);
  };

  const handleCheckIn = async () => {
    if (!selectedAllocation?.job?.id) {
      message.error("No job selected");
      return;
    }

    if (!selectedAllocation.job.requestorID) {
      message.error("No site manager found for this job. Please contact support.");
      return;
    }

    try {
      const userContext = await GetLoggedInUserContextAsync();
      if (!userContext?.user?.employeeID) {
        message.error("No employee ID found. Please contact support.");
        return;
      }

      // Create the timesheet with only the required fields
      const timesheet: Timesheet = {
        timeIn: new Date().toISOString(),
        jobID: selectedAllocation.job.id,
        employeeID: userContext.user.employeeID,
        siteManagerID: selectedAllocation.job.requestorID
      };

      console.log("Sending timesheet data:", timesheet); // Debug log
      const response = await signInAsync(timesheet);
      console.log("Server response:", response); // Debug log
      
      message.success("Successfully checked in!");
      setIsModalVisible(false);
      setSelectedAllocation(null);
      setSelectedJob("Check in"); // Reset the dropdown to "Check in"
    } catch (error) {
      console.error("Failed to check in:", error);
      message.error("Failed to check in. Please try again.");
    }
  };

  const jobColumns: ColumnsType<Allocation> = [
    {
      title: "Job Name",
      key: "jobName",
      render: (_, record) => record.job?.jobName || "N/A",
    },
    {
      title: "Job Location",
      key: "location",
      render: (_, record) => record.job?.location || "N/A",
    },
    {
      title: "Date",
      key: "date",
      render: (_, record) => new Date(record.time).toLocaleDateString(),
    },
    {
      title: "Time",
      key: "time",
      render: (_, record) => new Date(record.time).toLocaleTimeString(),
    },
    {
      title: "Hours",
      dataIndex: "hoursNeeded",
      key: "hours",
    },
  ];

  return (
    <Layout style={{ minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: "100%", padding: "20px" }}>
        <Header style={{ textAlign: "center", marginBottom: "20px", background: "transparent" }}>
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
            <h2>Home Page</h2>
            <Button onClick={() => navigate("/employeeupdate")}>Update Profile</Button>
          </div>
        </Header>

        <Content>
          <div style={{ width: "100%", marginBottom: "20px" }}>
            <Select
              value={selectedJob}
              onChange={handleJobChange}
              style={{ width: "100%", marginBottom: "20px" }}
            >
              {allocations.map((allocation) => (
                <Option 
                  key={`${allocation.job?.id}-${allocation.time}`} 
                  value={allocation.job?.jobName || ""}
                >
                  {allocation.job?.jobName || "N/A"}
                </Option>
              ))}
            </Select>

            <Modal
              title="Confirm Check In"
              open={isModalVisible}
              onOk={handleCheckIn}
              onCancel={handleModalCancel}
              okText="Check In"
              cancelText="Cancel"
            >
              <p>Are you sure you want to check in to {selectedAllocation?.job?.jobName}?</p>
              <p>Location: {selectedAllocation?.job?.location}</p>
              <p>Hours Needed: {selectedAllocation?.hoursNeeded}</p>
            </Modal>

            <Card title="My Job Allocations" style={{ marginBottom: "20px" }}>
              <Table
                columns={jobColumns}
                dataSource={allocations}
                rowKey="id"
                pagination={false}
                loading={loading}
              />
            </Card>
          </div>
        </Content>
      </Card>
    </Layout>
  );
};

export default EmployeeHome; 