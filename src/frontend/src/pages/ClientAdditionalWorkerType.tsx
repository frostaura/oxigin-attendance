import React, { useState, useEffect } from "react";
import { Table, Form, Input, InputNumber, Button, message } from "antd";
import { PlusOutlined, MinusCircleOutlined } from "@ant-design/icons";
import { useNavigate, useLocation } from "react-router-dom";

interface Worker {
  key: number;
  workerType: string;
  workersNeeded: number;
  hoursNeeded: number;
}

const ClientAdditionalWorkerType: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const location = useLocation();
  const [workers, setWorkers] = useState<Worker[]>([{ key: 0, workerType: '', workersNeeded: 1, hoursNeeded: 1 }]);

  // Load existing workers if any
  useEffect(() => {
    const state = location.state as { existingWorkers?: Worker[] };
    if (state?.existingWorkers?.length) {
      setWorkers(state.existingWorkers);
      // Set form values for existing workers
      const formValues: Record<string, any> = {};
      state.existingWorkers.forEach((worker) => {
        formValues[`workerType_${worker.key}`] = worker.workerType;
        formValues[`workersNeeded_${worker.key}`] = worker.workersNeeded;
        formValues[`hoursNeeded_${worker.key}`] = worker.hoursNeeded;
      });
      form.setFieldsValue(formValues);
    }
  }, [location.state, form]);

  const addWorkerRow = (): void => {
    setWorkers([...workers, { 
      key: workers.length,
      workerType: '',
      workersNeeded: 1,
      hoursNeeded: 1
    }]);
  };

  const removeWorkerRow = (key: number): void => {
    setWorkers(workers.filter((worker) => worker.key !== key));
  };

  const handleDone = async () => {
    try {
      const values = await form.validateFields();
      
      // Transform form values into worker objects
      const formattedWorkers = workers.map((worker) => ({
        key: worker.key,
        workerType: values[`workerType_${worker.key}`],
        workersNeeded: values[`workersNeeded_${worker.key}`],
        hoursNeeded: values[`hoursNeeded_${worker.key}`]
      }));

      // Navigate back with the workers data
      navigate("/clientjobrequest", {
        state: { additionalWorkers: formattedWorkers }
      });
    } catch (error) {
      message.error('Please fill in all required fields');
    }
  };

  const columns = [
    {
      title: "Worker Type",
      dataIndex: "workerType",
      key: "workerType",
      width: "50%",
      render: (_: unknown, record: Worker) => (
        <Form.Item 
          name={`workerType_${record.key}`} 
          rules={[{ required: true, message: "Enter worker type" }]}
          initialValue={record.workerType}
        >
          <Input placeholder="Enter worker type" />
        </Form.Item>
      ),
    },
    {
      title: "Workers Needed",
      dataIndex: "workersNeeded",
      key: "workersNeeded",
      width: "20%",
      render: (_: unknown, record: Worker) => (
        <Form.Item 
          name={`workersNeeded_${record.key}`} 
          rules={[{ required: true, message: "Enter number" }]}
          initialValue={record.workersNeeded}
        >
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>
      ),
    },
    {
      title: "Hours Needed",
      dataIndex: "hoursNeeded",
      key: "hoursNeeded",
      width: "20%",
      render: (_: unknown, record: Worker) => (
        <Form.Item 
          name={`hoursNeeded_${record.key}`} 
          rules={[{ required: true, message: "Enter hours" }]}
          initialValue={record.hoursNeeded}
        >
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>
      ),
    },
    {
      title: "Action",
      key: "action",
      width: "10%",
      render: (_: unknown, record: Worker) =>
        workers.length > 1 ? (
          <Button 
            type="text" 
            icon={<MinusCircleOutlined style={{ color: "red" }} />} 
            onClick={() => removeWorkerRow(record.key)} 
          />
        ) : null,
    },
  ];

  return (
    <div style={{ maxWidth: 800, margin: "0 auto", padding: 20 }}>
      <h2>Additional Workers</h2>
      <Form form={form} layout="vertical">
        <Table columns={columns} dataSource={workers} pagination={false} rowKey="key" />

        <Button type="dashed" icon={<PlusOutlined />} onClick={addWorkerRow} style={{ width: "100%", marginTop: 10 }}>
          Add Another Worker Type
        </Button>

        <Button type="primary" style={{ width: "100%", marginTop: 10 }} onClick={handleDone}>
          Done
        </Button>
      </Form>
    </div>
  );
};

export default ClientAdditionalWorkerType;