import React from "react";
import { Form, Input, DatePicker, TimePicker, InputNumber, Button, Table } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

interface Worker {
  key: string;
  name: string;
  role: string;
}

const ClientJobRequest: React.FC = () => {
  const navigate = useNavigate();

  // Sample table data
  const columns = [
    { title: "Worker Name", dataIndex: "name", key: "name" },
    { title: "Role", dataIndex: "role", key: "role" },
  ];

  const dataSource: Worker[] = [
    { key: "1", name: "John Doe", role: "Electrician" },
    { key: "2", name: "Jane Smith", role: "Plumber" },
  ];

  return (
    <div style={{ maxWidth: 600, margin: "0 auto", padding: 20 }}>
      <h2 style={{ textAlign: "left", marginBottom: 40 }}>Request a Job</h2>

      <Form layout="vertical">
        <Form.Item label="Job Name" style={{ marginBottom: 20 }}>
          <Input placeholder="Enter job name" />
        </Form.Item>

        <Form.Item label="Requestor Name" style={{ marginBottom: 20 }}>
          <Input placeholder="Enter requestor name" />
        </Form.Item>

        <Form.Item label="Purchase Order Number" style={{ marginBottom: 20 }}>
          <Input placeholder="Enter purchase order number" />
        </Form.Item>

        <Form.Item label="Date" style={{ marginBottom: 20 }}>
          <DatePicker style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item label="Time" style={{ marginBottom: 20 }}>
          <TimePicker style={{ width: "100%" }} use12Hours format="h:mm A" />
        </Form.Item>

        <Form.Item label="Location of Job" style={{ marginBottom: 20 }}>
          <Input placeholder="Enter job location" />
        </Form.Item>

        <Form.Item label="Number of Workers Needed" style={{ marginBottom: 20 }}>
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item label="Number of Hours Needed" style={{ marginBottom: 20 }}>
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
        <Button type="primary" style={{ marginTop: 20, width: "100%" }} onClick={() => navigate("/adminjobshome")}>
          Submit Request
        </Button>
      </Form>
    </div>
  );
};

export default ClientJobRequest;
