import React, { useState, useEffect } from "react";
import type { ChangeEvent } from "react";
import { Layout, Card, Table, Input, Button, message } from "antd";
import { EditOutlined, MinusCircleOutlined, PlusOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import { addEmployeeAsync, getEmployeesAsync, removeEmployeeAsync, updateEmployeeAsync } from "../services/data/employee";
import type { Employee } from "../models/employeeModels";

const { Header, Content } = Layout;

interface EditableEmployee extends Employee {
  key: string;
}

const ManageEmployees: React.FC = () => {
  const navigate = useNavigate();
  const [editingKey, setEditingKey] = useState<string | null>(null);
  const [employees, setEmployees] = useState<EditableEmployee[]>([]);
  const [loading, setLoading] = useState(false);
  const [newEmployee, setNewEmployee] = useState<EditableEmployee | null>(null);
  const [searchValue, setSearchValue] = useState<string>("");

  // Fetch employees on component mount
  useEffect(() => {
    fetchEmployees();
  }, []);

  const fetchEmployees = async () => {
    try {
      setLoading(true);
      const fetchedEmployees = await getEmployeesAsync();
      const formattedEmployees = fetchedEmployees.map((emp) => ({
        ...emp,
        key: emp.id
      }));
      setEmployees(formattedEmployees);
    } catch (error) {
      message.error("Failed to fetch employees");
      console.error("Error fetching employees:", error);
    } finally {
      setLoading(false);
    }
  };

  // Start editing an existing employee
  const handleEdit = (key: string) => {
    setEditingKey(key);
  };

  // Save changes to existing or new employee
  const handleSave = async (key: string) => {
    try {
      setLoading(true);
      const employeeToSave = newEmployee?.key === key ? newEmployee : employees.find(emp => emp.key === key);
      
      if (!employeeToSave) {
        throw new Error("Employee data not found");
      }

      if (newEmployee?.key === key) {
        // For new employee, don't include the id
        const employeeData: Partial<Employee> = {
          idNumber: employeeToSave.idNumber,
          address: employeeToSave.address,
          contactNo: employeeToSave.contactNo || '',
          bankName: employeeToSave.bankName || '',
          accountHolderName: employeeToSave.accountHolderName || '',
          branchCode: employeeToSave.branchCode || '',
          accountNumber: employeeToSave.accountNumber || ''
        };
        await addEmployeeAsync(employeeData);
        message.success("Employee added successfully");
        setNewEmployee(null);
      } else {
        // For updates, make sure to include the existing ID
        const employeeData: Employee = {
          id: employeeToSave.id, // Include the existing ID
          idNumber: employeeToSave.idNumber,
          address: employeeToSave.address,
          contactNo: employeeToSave.contactNo || '',
          bankName: employeeToSave.bankName || '',
          accountHolderName: employeeToSave.accountHolderName || '',
          branchCode: employeeToSave.branchCode || '',
          accountNumber: employeeToSave.accountNumber || ''
        };
        await updateEmployeeAsync(employeeData);
        message.success("Employee updated successfully");
      }

      await fetchEmployees();
    } catch (error) {
      message.error("Failed to save employee");
      console.error("Error saving employee:", error);
    } finally {
      setLoading(false);
      setEditingKey(null);
    }
  };

  // Delete an employee row
  const handleDelete = async (key: string) => {
    try {
      setLoading(true);
      await removeEmployeeAsync(key);
      message.success("Employee removed successfully");
      await fetchEmployees(); // Refresh the list
    } catch (error) {
      message.error("Failed to delete employee");
      console.error("Error deleting employee:", error);
    } finally {
      setLoading(false);
    }
  };

  // Add a new employee row
  const handleAddEmployee = () => {
    if (!newEmployee) {
      const newKey = crypto.randomUUID();
      setNewEmployee({
        key: newKey,
        id: newKey,
        idNumber: "",
        address: "",
        contactNo: "",
        bankName: "",
        accountHolderName: "",
        branchCode: "",
        accountNumber: ""
      });
      setEditingKey(newKey);
    }
  };

  const handleInputChange = (key: string, field: keyof EditableEmployee, value: string) => {
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
    employee.idNumber?.toLowerCase().includes(searchValue.toLowerCase())
  );

  const columns: ColumnsType<EditableEmployee> = [
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
      dataIndex: "contactNo",
      key: "contactNo",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.contactNo} onChange={(e) => handleInputChange(record.key, "contactNo", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Bank Name",
      dataIndex: "bankName",
      key: "bankName",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.bankName} onChange={(e) => handleInputChange(record.key, "bankName", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Account Holder",
      dataIndex: "accountHolderName",
      key: "accountHolderName",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.accountHolderName} onChange={(e) => handleInputChange(record.key, "accountHolderName", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Branch Code",
      dataIndex: "branchCode",
      key: "branchCode",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.branchCode} onChange={(e) => handleInputChange(record.key, "branchCode", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Account Number",
      dataIndex: "accountNumber",
      key: "accountNumber",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.accountNumber} onChange={(e) => handleInputChange(record.key, "accountNumber", e.target.value)} />
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
            <Button type="text" icon={<CloseOutlined style={{ color: "red" }} />} onClick={() => setEditingKey(null)} />
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
            <Table 
              columns={columns} 
              dataSource={newEmployee ? [...filteredEmployees, newEmployee] : filteredEmployees} 
              pagination={false}
              loading={loading}
            />
            
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