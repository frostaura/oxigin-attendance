import React, { useState } from "react";
import { Table, Form, Select, InputNumber, Button } from "antd";
import { PlusOutlined, MinusCircleOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

const { Option } = Select;

interface Worker {
  key: number;
}

interface Column {
  title: string;
  dataIndex: string;
  key: string;
  width: string;
  render?: (text: any, record: Worker) => JSX.Element;
}

const ClientAdditionalWorkerType: React.FC = () => {
  const [workers, setWorkers] = useState<Worker[]>([{ key: 0 }]);
  const [workerOptions, setWorkerOptions] = useState<string[]>(["Electrician", "Plumber", "Carpenter", "Painter"]); // Default options
  const navigate = useNavigate();

  // Function to add a new worker type row
  const addWorkerRow = () => {
    setWorkers([...workers, { key: workers.length }]);
  };

  // Function to remove a worker type row
  const removeWorkerRow = (key: number) => {
    setWorkers(workers.filter((worker) => worker.key !== key));
  };

  // Function to handle adding new options when typing in the dropdown
  const handleWorkerTypeChange = (value: string | string[]) => {
    if (typeof value === "string" && !workerOptions.includes(value)) {
      setWorkerOptions([...workerOptions, value]); // Add new worker type
    }
  };

  const columns: Column[] = [
    {
      title: "Worker Type",
      dataIndex: "workerType",
      key: "workerType",
      width: "50%",
      render: (_, record) => (
        <Form.Item name={`workerType_${record.key}`} rules={[{ required: true, message: "Select worker type" }]}>
          <Select
            mode="tags"
            placeholder="Select or type a worker type"
            style={{ width: "100%" }}
            onChange={handleWorkerTypeChange}
          >
            {workerOptions.map((option) => (
              <Option key={option} value={option}>
                {option}
              </Option>
            ))}
          </Select>
        </Form.Item>
      ),
    },
    {
      title: "Workers Needed",
      dataIndex: "workersNeeded",
      key: "workersNeeded",
      width: "20%",
      render: (_, record) => (
        <Form.Item name={`workersNeeded_${record.key}`} rules={[{ required: true, message: "Enter number" }]}>
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>
      ),
    },
    {
      title: "Hours Needed",
      dataIndex: "hoursNeeded",
      key: "hoursNeeded",
      width: "20%",
      render: (_, record) => (
        <Form.Item name={`hoursNeeded_${record.key}`} rules={[{ required: true, message: "Enter hours" }]}>
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>
      ),
    },
    {
      title: "Action",
      key: "action",
      width: "10%",
      render: (_, record) =>
        workers.length > 1 ? (
          <Button type="text" icon={<MinusCircleOutlined style={{ color: "red" }} />} onClick={() => removeWorkerRow(record.key)} />
        ) : null,
    },
  ];

  return (
    <div style={{ maxWidth: 800, margin: "0 auto", padding: 20 }}>
      <h2>Additional Workers</h2>
      <Form layout="vertical">
        <Table columns={columns} dataSource={workers} pagination={false} rowKey="key" />

        <Button type="dashed" icon={<PlusOutlined />} onClick={addWorkerRow} style={{ width: "100%", marginTop: 10 }}>
          Add Another Worker Type
        </Button>

        <Button type="primary" style={{ width: "100%", marginTop: 10 }} onClick={() => navigate("/clientjobrequest")}>
          Done
        </Button>
      </Form>
    </div>
  );
};

export default ClientAdditionalWorkerType;
