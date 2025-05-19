import React, { useState } from "react";
import { Table, Select, Button, Checkbox, Card } from "antd";
import { useNavigate } from "react-router-dom";
import Employee from "../models/Employee";

const { Option } = Select;

const AdminJobAllocations: React.FC = () => {
  const navigate = useNavigate();
  const [selectedJob, setSelectedJob] = useState<string | null>(null);
  const [availableEmployees, setAvailableEmployees] = useState<Employee[]>([
    { id: "E001", name: "John Doe", contact: "555-1234", checked: false },
    { id: "E002", name: "Jane Smith", contact: "555-5678", checked: false },
  ]);
  const [allocatedEmployees, setAllocatedEmployees] = useState<Employee[]>([]);

  const handleJobChange = (value: string) => {
    setSelectedJob(value);
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

  const columns = [
    { title: "Employee ID", dataIndex: "id", key: "id" },
    { title: "Name", dataIndex: "name", key: "name" },
    { title: "Contact", dataIndex: "contact", key: "contact" },
  ];

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        <h2 style={{ textAlign: "center" }}>Job Allocations</h2>
        
        <Select placeholder="Select a job" style={{ width: 200, marginBottom: 20 }} onChange={handleJobChange}>
          <Option value="Job1">Job 1</Option>
          <Option value="Job2">Job 2</Option>
        </Select>
        
        <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
  <Card title="Available Employees" style={{ width: "100%" }}>
    <Table
      dataSource={availableEmployees}
      columns={[
        ...columns,
        {
          title: "Select",
          key: "select",
          render: (_: any, record: Employee) => (
            <Checkbox checked={record.checked} onChange={() => handleCheckboxChange(record)} />
          ),
        },
      ]}
      rowKey="id"
      pagination={false}
    />
  </Card>

  <Card title="Allocated Employees" style={{ width: "100%" }}>
    <Table dataSource={allocatedEmployees} columns={columns} rowKey="id" pagination={false} />
  </Card>
</div>

        <div style={{ display: "flex", justifyContent: "space-between", marginTop: 20 }}>
          <Button type="primary" onClick={allocateSelectedEmployees} disabled={!availableEmployees.some(emp => emp.checked)}>
            Allocate Selected
          </Button>
          <Button type="default" onClick={() => navigate(-1)}>
            Submit
          </Button>
        </div>
      </Card>
    </div>
  );
};

export default AdminJobAllocations;
