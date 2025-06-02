import React, { useState } from "react";
import { Form, Input, DatePicker, TimePicker, InputNumber, Button, Table, message } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import type { Dayjs } from "dayjs";
import { createJobsAsync } from "../services/data/jobRequests";
import type { Job } from "../types";

interface Worker {
  key: string;
  name: string;
  role: string;
}

interface JobRequestForm {
  jobName: string;
  requestorName: string;
  purchaseOrderNumber: string;
  date: Dayjs;
  time: Dayjs;
  location: string;
  numberOfWorkers: number;
  numberOfHours: number;
}

const ClientJobRequest: React.FC = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm<JobRequestForm>();
  const [processing, setProcessing] = useState(false);

  // Sample table data
  const columns: ColumnsType<Worker> = [
    { title: "Worker Name", dataIndex: "name", key: "name" },
    { title: "Role", dataIndex: "role", key: "role" },
  ];

  const dataSource: Worker[] = [
    { key: "1", name: "John Doe", role: "Electrician" },
    { key: "2", name: "Jane Smith", role: "Plumber" },
  ];

  const handleSubmit = async (values: JobRequestForm) => {
    try {
      setProcessing(true);

      const dateTime = new Date(
        values.date.year(),
        values.date.month(),
        values.date.date(),
        values.time.hour(),
        values.time.minute());
      
      // Convert form values to JobRequest format
      const jobRequest: Job = {
        jobName: values.jobName,
        requestorName: values.requestorName,
        purchaseOrderNumber: values.purchaseOrderNumber,
        time: dateTime,
        location: values.location,
        numberOfWorkers: values.numberOfWorkers,
        numberOfHours: values.numberOfHours,
        approved: false
      };

      await createJobsAsync(jobRequest);
      message.success('Job request submitted successfully');
      navigate("/clienthome");
    } catch (error) {
      console.error('Failed to submit job request:', error);
      message.error('Failed to submit job request. Please try again.');
    } finally {
      setProcessing(false);
    }
  };

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
          name="numberOfWorkers" 
          label="Number of Workers Needed" 
          rules={[{ required: true, message: "Please enter number of workers" }]}
          style={{ marginBottom: 20 }}
        >
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item 
          name="numberOfHours" 
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
          onClick={() => navigate("/clientadditionalworkertype")}
          style={{ width: "100%", marginBottom: 20 }}
        >
          Additional Workers Needed
        </Button>

        {/* Small Table at the Bottom */}
        <Table columns={columns} dataSource={dataSource} size="small" pagination={false} />

        {/* Submit Request Button */}
        <Button type="primary" htmlType="submit" style={{ marginTop: 20, width: "100%" }} loading={processing}>
          {processing ? "Submitting Request..." : "Submit Request"}
        </Button>
      </Form>
    </div>
  );
};

export default ClientJobRequest; 