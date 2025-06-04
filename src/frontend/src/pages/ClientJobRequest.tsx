import React, { useState } from "react";
import { Form, Input, DatePicker, TimePicker, InputNumber, Button, Table, message, Card } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import type { Dayjs } from "dayjs";
import { createJobAsync } from "../services/data/job";
import type { Job } from "../models/jobModels";

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
        purchaseOrderNumber: values.purchaseOrderNumber,
        time: dateTime,
        location: values.location,
        numWorkers: values.numberOfWorkers,
        numHours: values.numberOfHours,
        approved: false,
      };

      await createJobAsync(jobRequest);
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
    <div className="form-container">
      <Card className="form-card" style={{ maxWidth: 600 }}>
        <h3 style={{ marginBottom: 16 }}>Request a Job</h3>

        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          <Form.Item 
            name="jobName" 
            label="Job Name" 
            rules={[{ required: true, message: "Please enter job name" }]}
          >
            <Input placeholder="Enter job name" />
          </Form.Item>

          <Form.Item 
            name="purchaseOrderNumber" 
            label="Purchase Order Number" 
            rules={[{ required: true, message: "Please enter purchase order number" }]}
          >
            <Input placeholder="Enter purchase order number" />
          </Form.Item>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '12px' }}>
            <Form.Item 
              name="date" 
              label="Date" 
              rules={[{ required: true, message: "Please select date" }]}
            >
              <DatePicker style={{ width: "100%" }} />
            </Form.Item>

            <Form.Item 
              name="time" 
              label="Time" 
              rules={[{ required: true, message: "Please select time" }]}
            >
              <TimePicker style={{ width: "100%" }} use12Hours format="h:mm A" />
            </Form.Item>
          </div>

          <Form.Item 
            name="location" 
            label="Location of Job" 
            rules={[{ required: true, message: "Please enter job location" }]}
          >
            <Input placeholder="Enter job location" />
          </Form.Item>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '12px' }}>
            <Form.Item 
              name="numberOfWorkers" 
              label="Number of Workers" 
              rules={[{ required: true, message: "Please enter number of workers" }]}
            >
              <InputNumber min={1} style={{ width: "100%" }} />
            </Form.Item>

            <Form.Item 
              name="numberOfHours" 
              label="Number of Hours" 
              rules={[{ required: true, message: "Please enter number of hours" }]}
            >
              <InputNumber min={1} style={{ width: "100%" }} />
            </Form.Item>
          </div>

          <Button
            type="dashed"
            icon={<PlusOutlined />}
            onClick={() => navigate("/clientadditionalworkertype")}
            style={{ width: "100%", marginBottom: 12 }}
          >
            Additional Workers Needed
          </Button>

          <Table 
            columns={columns} 
            dataSource={dataSource} 
            size="small" 
            pagination={false}
            style={{ marginBottom: 12 }}
          />

          <Button type="primary" htmlType="submit" block loading={processing}>
            {processing ? "Submitting Request..." : "Submit Request"}
          </Button>
        </Form>
      </Card>
    </div>
  );
};

export default ClientJobRequest; 