import React, { useState, useEffect } from "react";
import { Form, Input, DatePicker, TimePicker, InputNumber, Button, Table, Select, message } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { getClientsAsync } from "../services/data/clients";
import { createJobAsync } from "../services/data/job";
import { GetLoggedInUserContextAsync } from "../services/data/backend";
import type { ClientData } from "../types";
import type { Job } from "../models/jobModels";

interface Worker {
  key: string;
  name: string;
  role: string;
}


const AdminJobRequest: React.FC = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm<Job>();
  const [clients, setClients] = useState<ClientData[]>([]);
  const [loading, setLoading] = useState(false);
  const [processing, setProcessing] = useState(false);

  // Fetch clients when component mounts
  useEffect(() => {
    const fetchClients = async () => {
      try {
        setLoading(true);
        const fetchedClients = await getClientsAsync();
        console.log('Fetched clients:', fetchedClients); // Debug clients data
        setClients(fetchedClients);
      } catch (error) {
        console.error("Error fetching clients:", error);
        if (error instanceof Error) {
          message.error(error.message);
        } else {
          message.error('Failed to fetch clients. Please try again later.');
        }
        // If there's no session, redirect to login
        if (error instanceof Error && error.message.includes('No active session')) {
          navigate('/');
        }
      } finally {
        setLoading(false);
      }
    };

    fetchClients();
  }, [navigate]);

  // Sample table data
  const columns: ColumnsType<Worker> = [
    { title: "Worker Name", dataIndex: "name", key: "name" },
    { title: "Role", dataIndex: "role", key: "role" },
  ];

  const dataSource: Worker[] = [
    { key: "1", name: "John Doe", role: "Electrician" },
    { key: "2", name: "Jane Smith", role: "Plumber" },
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

      console.log('User context:', userContext); // Debug user context

      const dateTime = new Date(
        values.date.year(),
        values.date.month(),
        values.date.date(),
        values.time.hour(),
        values.time.minute()
      );
      
      // Convert form values to JobRequest format
      const jobRequest: Job = {
        jobName: values.jobName,
        purchaseOrderNumber: values.purchaseOrderNumber,
        time: dateTime,
        location: values.location,
        numWorkers: values.numWorkers,
        numHours: values.numHours,
        approved: false,
        clientID: values.clientId  // Use the selected client ID from the form
      };

      console.log('Submitting job request:', jobRequest); // Debug logging
      await createJobAsync(jobRequest);
      message.success('Job request submitted successfully');
      navigate("/adminjobshome");
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
          name="clientId" 
          label="Client" 
          rules={[{ required: true, message: "Please select a client" }]}
          style={{ marginBottom: 20 }}
        >
          <Select 
            placeholder="Select client" 
            loading={loading}
            disabled={loading}
            onChange={(value) => {
              console.log('Selected client ID:', value); // Debug selected client
            }}
          >
            {clients.map(client => (
              <Select.Option key={client.id} value={client.id}>
                {client.company || client.name}
              </Select.Option>
            ))}
          </Select>
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
          onClick={() => navigate("/adminadditionalworkertype")}
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

export default AdminJobRequest; 