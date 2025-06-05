import React, { useState, useEffect } from "react";
import { Form, Input, DatePicker, TimePicker, InputNumber, Button, Table, message } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useNavigate, useLocation } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { createJobAsync } from "../services/data/job";
import { GetLoggedInUserContextAsync } from "../services/data/backend";
import { addAdditionalWorker } from "../services/data/additionalWorker";
import type { AdditionalWorker } from "../models/additionalWorkerModels";
import type { Job } from "../models/jobModels";
import dayjs from "dayjs";

interface Worker {
  key: string;
  workerType: string;
  workersNeeded: number;
  hoursNeeded: number;
}

const ClientJobRequest: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [form] = Form.useForm<Job>();
  const [processing, setProcessing] = useState(false);
  const [additionalWorkers, setAdditionalWorkers] = useState<Worker[]>([]);

  // Effect to handle returning from AdditionalWorkerType page
  useEffect(() => {
    const state = location.state as { additionalWorkers?: Worker[] };
    if (state?.additionalWorkers) {
      setAdditionalWorkers(state.additionalWorkers);
    }
  }, [location]);

  // Update the columns for the workers table
  const columns: ColumnsType<Worker> = [
    { 
      title: "Worker Type", 
      dataIndex: "workerType", 
      key: "workerType" 
    },
    { 
      title: "Workers Needed", 
      dataIndex: "workersNeeded", 
      key: "workersNeeded" 
    },
    { 
      title: "Hours Needed", 
      dataIndex: "hoursNeeded", 
      key: "hoursNeeded" 
    }
  ];

  const handleSubmit = async (values: any) => {
    try {
      setProcessing(true);

      // Check if user is logged in
      const userContext = await GetLoggedInUserContextAsync();
      if (!userContext) {
        message.error('You must be logged in to submit a job request');
        navigate('/');
        return;
      }

      if (!userContext.user.clientID) {
        message.error('You must be associated with a client to submit job requests.');
        navigate('/');
        return;
      }
      
      const dateTime = new Date(
        values.date.year(),
        values.date.month(),
        values.date.date(),
        values.time.hour(),
        values.time.minute()
      );
      
      // Convert form values to JobRequest format
      const jobRequest = {
        jobName: values.jobName,
        purchaseOrderNumber: values.purchaseOrderNumber,
        time: dateTime,
        location: values.location,
        numWorkers: values.numWorkers,
        numHours: values.numHours,
        approved: false,
        clientID: userContext.user.clientID, // Use the logged-in user's client ID
        request: values.requestorName
      };

      console.log('Submitting job request:', jobRequest);
      const createdJob = await createJobAsync(jobRequest as Job);

      // If we have additional workers, add them to the job
      if (additionalWorkers.length > 0) {
        try {
          // Add each additional worker with minimal data
          const additionalWorkerPromises = additionalWorkers.map(worker => {
            const additionalWorkerRequest: AdditionalWorker = {
              workerType: worker.workerType,
              numWorkers: worker.workersNeeded,
              numHours: worker.hoursNeeded,
              jobID: createdJob.id || '', // Ensure jobID is set
            };
            return addAdditionalWorker(additionalWorkerRequest);
          });

          await Promise.all(additionalWorkerPromises);
          console.log('Additional workers added successfully');
        } catch (error) {
          console.error('Error adding additional workers:', error);
          message.warning('Job created but failed to add some additional workers. Please contact support.');
        }
      }
      
      // Clear the saved form data after successful submission
      localStorage.removeItem('clientJobRequestFormData');
      
      message.success('Job request submitted successfully');
      navigate("/clienthome");
    } catch (error) {
      console.error('Failed to submit job request:', error);
      if (error instanceof Error) {
        message.error(error.message);
      } else {
        message.error('Failed to submit job request. Please try again.');
      }
    } finally {
      setProcessing(false);
    }
  };

  const handleNavigateToAdditionalWorkers = () => {
    // Save current form values to localStorage before navigating
    const currentFormValues = form.getFieldsValue();
    localStorage.setItem('clientJobRequestFormData', JSON.stringify(currentFormValues));
    navigate("/clientadditionalworkertype", { 
      state: { existingWorkers: additionalWorkers } 
    });
  };

  // Effect to restore form data when component mounts
  useEffect(() => {
    const savedFormData = localStorage.getItem('clientJobRequestFormData');
    if (savedFormData) {
      const parsedData = JSON.parse(savedFormData);
      // Convert string dates back to moment objects if needed
      if (parsedData.date) {
        parsedData.date = dayjs(parsedData.date);
      }
      if (parsedData.time) {
        parsedData.time = dayjs(parsedData.time);
      }
      form.setFieldsValue(parsedData);
    }
  }, [form]);

  return (
    <div style={{ maxWidth: 600, margin: "0 auto", padding: 20 }}>
      <h2 style={{ textAlign: "left", marginBottom: 40 }}>Request a Job</h2>

      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item 
          name="jobName" 
          label="Job Name" 
          rules={[{ required: true, message: "Please enter job name" }]}
          style={{ marginBottom: 20 }}
        >
          <Input placeholder="Enter job name" />
        </Form.Item>

        <Form.Item 
          name="requestorName" 
          label="Requestor Name" 
          rules={[{ required: true, message: "Please enter requestor name" }]}
          style={{ marginBottom: 20 }}
        >
          <Input placeholder="Enter requestor name" />
        </Form.Item>

        <Form.Item 
          name="purchaseOrderNumber" 
          label="Purchase Order Number" 
          rules={[{ required: true, message: "Please enter purchase order number" }]}
          style={{ marginBottom: 20 }}
        >
          <Input placeholder="Enter purchase order number" />
        </Form.Item>

        <Form.Item 
          name="date" 
          label="Date" 
          rules={[{ required: true, message: "Please select date" }]}
          style={{ marginBottom: 20 }}
        >
          <DatePicker style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item 
          name="time" 
          label="Time" 
          rules={[{ required: true, message: "Please select time" }]}
          style={{ marginBottom: 20 }}
        >
          <TimePicker style={{ width: "100%" }} use12Hours format="h:mm A" />
        </Form.Item>

        <Form.Item 
          name="location" 
          label="Location of Job" 
          rules={[{ required: true, message: "Please enter job location" }]}
          style={{ marginBottom: 20 }}
        >
          <Input placeholder="Enter job location" />
        </Form.Item>

        <Form.Item 
          name="numWorkers" 
          label="Number of Workers Needed" 
          rules={[{ required: true, message: "Please enter number of workers" }]}
          style={{ marginBottom: 20 }}
        >
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item 
          name="numHours" 
          label="Number of Hours Needed" 
          rules={[{ required: true, message: "Please enter number of hours" }]}
          style={{ marginBottom: 20 }}
        >
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>

        {/* Button to Add Additional Workers */}
        <Button
          type="dashed"
          icon={<PlusOutlined />}
          onClick={handleNavigateToAdditionalWorkers}
          style={{ width: "100%", marginBottom: 20 }}
        >
          Additional Workers Needed
        </Button>

        {/* Table showing additional workers */}
        {additionalWorkers.length > 0 && (
          <Table 
            columns={columns} 
            dataSource={additionalWorkers} 
            size="small" 
            pagination={false}
            style={{ marginBottom: 20 }}
          />
        )}

        {/* Submit Request Button */}
        <Button type="primary" htmlType="submit" style={{ marginTop: 20, width: "100%" }} loading={processing}>
          {processing ? "Submitting Request..." : "Submit Request"}
        </Button>
      </Form>
    </div>
  );
};

export default ClientJobRequest; 