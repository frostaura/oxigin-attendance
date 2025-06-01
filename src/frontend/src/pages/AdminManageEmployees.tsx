import React, { useState, ChangeEvent } from "react";
import { Layout, Card, Table, Input, Button } from "antd";
import { EditOutlined, MinusCircleOutlined, PlusOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { EmployeeData } from "../types";

const { Header, Content } = Layout;

interface EditableEmployeeData extends Omit<EmployeeData, 'phone'> {
  surname: string;
  idNumber: string;
  address: string;
  contact: string;
}

const ManageEmployees: React.FC = () => {
  const navigate = useNavigate();
  const [editingKey, setEditingKey] = useState<string | null>(null);
  const [employees, setEmployees] = useState<EditableEmployeeData[]>([
    { key: "1", id: "E001", name: "John", surname: "Doe", idNumber: "123456789", address: "123 Street, NY", contact: "555-1234", email: "", role: "employee", status: "active" },
    { key: "2", id: "E002", name: "Jane", surname: "Smith", idNumber: "987654321", address: "456 Avenue, LA", contact: "555-5678", email: "", role: "employee", status: "active" },
  ]);

  const [newEmployee, setNewEmployee] = useState<EditableEmployeeData | null>(null);
  const [searchValue, setSearchValue] = useState<string>("");

  // Start editing an existing employee
  const handleEdit = (key: string) => {
    setEditingKey(key);
  };

  // Save changes to existing or new employee
  const handleSave = (key: string) => {
    if (newEmployee?.key === key) {
      setEmployees([...employees, newEmployee]); // Persist the new employee
      setNewEmployee(null);
    }
    setEditingKey(null);
  };

  // Delete an employee row
  const handleDelete = (key: string) => {
    setEmployees(employees.filter((emp) => emp.key !== key));
  };

  // Add a new employee row
  const handleAddEmployee = () => {
    if (!newEmployee) {
      const newKey = (employees.length + 1).toString();
      setNewEmployee({
        key: newKey,
        id: "",
        name: "",
        surname: "",
        idNumber: "",
        address: "",
        contact: "",
        email: "",
        role: "employee",
        status: "pending",
      });
      setEditingKey(newKey);
    }
  };

  // Handle input change for both new and existing employees
  const handleInputChange = (key: string, field: keyof EditableEmployeeData, value: string) => {
    if (editingKey === key) {
      if (newEmployee?.key === key) {
        setNewEmployee({ ...newEmployee, [field]: value });
      } else {
        setEmployees((prev) =>
          prev.map((emp) => (emp.key === key ? { ...emp, [field]: value } : emp))
        );
      }
    }
  };

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchValue(e.target.value);
  };

  const filteredEmployees = employees.filter(employee =>
    employee.id.toLowerCase().includes(searchValue.toLowerCase()) ||
    employee.name.toLowerCase().includes(searchValue.toLowerCase())
  );

  const columns: ColumnsType<EditableEmployeeData> = [
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.name} onChange={(e) => handleInputChange(record.key, "name", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Surname",
      dataIndex: "surname",
      key: "surname",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.surname} onChange={(e) => handleInputChange(record.key, "surname", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "ID Number",
      dataIndex: "idNumber",
      key: "idNumber",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.idNumber} onChange={(e) => handleInputChange(record.key, "idNumber", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Address",
      dataIndex: "address",
      key: "address",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.address} onChange={(e) => handleInputChange(record.key, "address", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Contact Number",
      dataIndex: "contact",
      key: "contact",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.contact} onChange={(e) => handleInputChange(record.key, "contact", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Actions",
      key: "actions",
      render: (_, record) =>
        editingKey === record.key ? (
          <div style={{ display: "flex", gap: 10 }}>
            <Button type="text" icon={<CheckOutlined style={{ color: "green" }} />} onClick={() => handleSave(record.key)} />
            <Button type="text" icon={<CloseOutlined style={{ color: "red" }} />} onClick={() => setNewEmployee(null)} />
          </div>
        ) : (
          <div style={{ display: "flex", gap: 10 }}>
            <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record.key)} />
            <Button type="text" icon={<MinusCircleOutlined style={{ color: "red" }} />} onClick={() => handleDelete(record.key)} />
          </div>
        ),
    },
  ];

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        {/* Page Header */}
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Manage Employees</h2>
        </Header>

        {/* Main Content */}
        <Content style={{ flex: 1, padding: 20 }}>
          <Card title="All Employees">
            <Table columns={columns} dataSource={newEmployee ? [...filteredEmployees, newEmployee] : filteredEmployees} pagination={false} />
            
            {/* Bottom section */}
            <div style={{ display: "flex", justifyContent: "space-between", marginTop: 20 }}>
              <div style={{ display: "flex", gap: 10 }}>
                {/* Back Button */}
                <Button onClick={() => navigate(-1)}>Back</Button>
                {/* Add Employee Button */}
                <Button type="dashed" icon={<PlusOutlined />} onClick={handleAddEmployee} disabled={!!newEmployee}>
                  Add Employee
                </Button>
              </div>

              {/* Search Box (Bottom Right) */}
              <Input 
                placeholder="Search Employee ID or Name" 
                style={{ borderColor: "#1890ff", borderWidth: 2, width: 200, padding: "8px" }}
                value={searchValue}
                onChange={handleSearchChange}
              />
            </div>
          </Card>
        </Content>
      </Card>
    </Layout>
  );
};

export default ManageEmployees; 