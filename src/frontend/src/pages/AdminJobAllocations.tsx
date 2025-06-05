import React, { useState, useEffect } from "react";
import { Table, Select, Button, Checkbox, Card, message } from "antd";
//import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import type { Job } from "../models/jobModels";
import type { Employee } from "../models/employeeModels";
import type { Allocation } from "../models/jobAllocationModels";
import { getJobsAsync } from "../services/data/job";
import { getEmployeesAsync } from "../services/data/employee";
import { getAllocationsForJobAsync, createJobAllocationAsync } from "../services/data/jobAllocation";

const { Option } = Select;

interface EmployeeWithCheck extends Employee {
  checked?: boolean;
}

const AdminJobAllocations: React.FC = () => {
  //const navigate = useNavigate();
  const [selectedJob, setSelectedJob] = useState<string | null>(null);
  const [jobs, setJobs] = useState<Job[]>([]);
  const [availableEmployees, setAvailableEmployees] = useState<EmployeeWithCheck[]>([]);
  const [allocatedEmployees, setAllocatedEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  // Fetch jobs on component mount
  useEffect(() => {
    const fetchJobs = async () => {
      try {
        const jobsData = await getJobsAsync();
        setJobs(jobsData);
      } catch (error) {
        console.error("Failed to fetch jobs:", error);
        message.error("Failed to fetch jobs");
      }
    };
    fetchJobs();
  }, []);

  // Check employee availability when a job is selected
  const checkEmployeeAvailability = async (jobId: string) => {
    setLoading(true);
    try {
      const [selectedJobData, allEmployees, existingAllocations] = await Promise.all([
        jobs.find(j => j.id === jobId),
        getEmployeesAsync(),
        getAllocationsForJobAsync(jobId)
      ]);

      if (!selectedJobData) {
        throw new Error("Selected job not found");
      }

      const jobTime = new Date(selectedJobData.time);
      const jobEndTime = new Date(jobTime.getTime() + (selectedJobData.numHours * 60 * 60 * 1000));

      // Get all allocations for each employee
      const employeeAvailability = await Promise.all(
        allEmployees.map(async (employee) => {
          // Check if employee has any conflicting allocations
          const hasConflict = existingAllocations.some(allocation => {
            const allocationTime = new Date(allocation.time);
            const allocationEndTime = new Date(allocationTime.getTime() + (allocation.hoursNeeded * 60 * 60 * 1000));
            
            return allocation.employeeID === employee.id && 
                   allocation.deleted === false &&
                   ((allocationTime >= jobTime && allocationTime < jobEndTime) ||
                    (allocationEndTime > jobTime && allocationEndTime <= jobEndTime));
          });

          return {
            ...employee,
            checked: false,
            available: !hasConflict
          };
        })
      );

      // Filter out employees who are not available
      const availableEmployees = employeeAvailability.filter(emp => emp.available);
      setAvailableEmployees(availableEmployees);
      
      // Update allocated employees list
      const allocatedEmployees = existingAllocations
        .filter(allocation => !allocation.deleted)
        .map(allocation => allEmployees.find(emp => emp.id === allocation.employeeID))
        .filter((emp): emp is Employee => emp !== undefined);
      
      setAllocatedEmployees(allocatedEmployees);

    } catch (error) {
      console.error("Failed to check employee availability:", error);
      message.error("Failed to check employee availability");
    } finally {
      setLoading(false);
    }
  };

  const handleJobChange = (value: string) => {
    setSelectedJob(value);
    checkEmployeeAvailability(value);
  };

  const handleCheckboxChange = (employee: Employee) => {
    const updatedAvailable = availableEmployees.map((emp) =>
      emp.id === employee.id ? { ...emp, checked: !emp.checked } : emp
    );
    setAvailableEmployees(updatedAvailable);
  };

  const allocateSelectedEmployees = () => {
    const selected = availableEmployees.filter((emp) => emp.checked);
    setAllocatedEmployees([...allocatedEmployees, ...selected]);
    setAvailableEmployees(availableEmployees.filter((emp) => !emp.checked));
  };

  const handleSubmit = async () => {
    if (!selectedJob) {
      message.error("Please select a job first");
      return;
    }

    const selectedJobData = jobs.find(j => j.id === selectedJob);
    if (!selectedJobData) {
      message.error("Selected job not found");
      return;
    }

    setSubmitting(true);
    try {
      // Create allocations for all employees in allocatedEmployees
      for (const employee of allocatedEmployees) {
        // Create the allocation with proper typing
        const allocation: Allocation = {
          name: `${employee.accountHolderName} - ${selectedJobData.jobName}`,
          description: "Standard Allocation",
          time: new Date(selectedJobData.time),  // Ensure proper Date object creation
          hoursNeeded: selectedJobData.numHours,
          jobID: selectedJob,
          employeeID: employee.id,
          deleted: false,  // Add deleted property
          job: selectedJobData,  // Include the full Job object
          employee: employee     // Include the full Employee object
        };
        
        await createJobAllocationAsync(allocation);
      }
      
      message.success("Job allocations created successfully");
    } catch (error) {
      console.error("Failed to create job allocations:", error);
      message.error("Failed to create job allocations. Please try again.");
    } finally {
      setSubmitting(false);
    }
  };

  const columns: ColumnsType<Employee> = [
    { title: "ID Number", dataIndex: "idNumber", key: "idNumber" },
    { title: "Contact", dataIndex: "contactNo", key: "contactNo" },
    { 
      title: "Name", 
      key: "name",
      render: (_, record) => `${record.accountHolderName}`
    },
  ];

  const availableColumns: ColumnsType<EmployeeWithCheck> = [
    ...columns,
    {
      title: "Select",
      key: "select",
      render: (_, record) => (
        <Checkbox 
          checked={record.checked} 
          onChange={() => handleCheckboxChange(record)}
          disabled={loading}
        />
      ),
    },
  ];

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", padding: "20px" }}>
      <Card className="responsive-card w-full max-w-[1200px]">
        <h2 className="page-title mb-4">Job Allocations</h2>
        
        <Select 
          placeholder="Select a job" 
          style={{ width: '100%', maxWidth: 300, marginBottom: 20 }} 
          onChange={handleJobChange}
          value={selectedJob}
          loading={loading}
        >
          {jobs.map(job => (
            <Option key={job.id} value={job.id}>
              {job.jobName} - {job.purchaseOrderNumber}
            </Option>
          ))}
        </Select>
        
        <div className="flex flex-col gap-4">
          <Card title="Available Employees">
            <div className="responsive-table">
              <Table
                dataSource={availableEmployees}
                columns={availableColumns}
                rowKey="id"
                pagination={{ 
                  pageSize: 8,
                  position: ['bottomCenter']
                }}
                loading={loading}
                scroll={{ x: 'max-content' }}
                size="middle"
                bordered
              />
            </div>
          </Card>

          <Card title="Allocated Employees">
            <div className="responsive-table">
              <Table 
                dataSource={allocatedEmployees} 
                columns={columns} 
                rowKey="id" 
                pagination={{ 
                  pageSize: 8,
                  position: ['bottomCenter']
                }}
                loading={loading}
                scroll={{ x: 'max-content' }}
                size="middle"
                bordered
              />
            </div>
          </Card>
        </div>

        <div style={{ display: "flex", justifyContent: "space-between", marginTop: 20 }}>
          <Button 
            type="primary" 
            onClick={allocateSelectedEmployees} 
            disabled={loading || submitting || !availableEmployees.some(emp => emp.checked)}
          >
            Allocate Selected
          </Button>
          <Button 
            type="primary"
            onClick={handleSubmit}
            loading={submitting}
            disabled={loading || submitting || allocatedEmployees.length === 0}
          >
            Submit Allocations
          </Button>
        </div>
      </Card>
    </div>
  );
};

export default AdminJobAllocations; 