import React, { useState, useEffect } from "react";
import { Layout, Card, Table, Button, Input, message, Form, Select, Modal } from "antd";
import { EditOutlined, MinusCircleOutlined, PlusOutlined, CheckOutlined, CloseOutlined, SearchOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import { getUsersAsync, updateUserAsync, deleteUserAsync, CreateUserAsAdmin } from "../services/data/user";
import { getClientsAsync } from "../services/data/client";
import { getEmployeesAsync, addEmployeeAsync } from "../services/data/employee";
import type { User } from "../models/userModels";
import type { Client } from "../models/clientModels";
import type { Employee } from "../models/employeeModels";
import type { ColumnType } from "antd/es/table";
import { UserType } from "../enums/userTypes";
import { hashString } from "../utils/crypto";

const { Header, Content } = Layout;
const { Search } = Input;

interface EditableUser extends User {
  key: string;
  client?: Client | null;
  employee?: Employee | null;
}

const AdminUsers: React.FC = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const [employeeForm] = Form.useForm();
  const [users, setUsers] = useState<EditableUser[]>([]);
  const [clients, setClients] = useState<Client[]>([]);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState("");
  const [editingKey, setEditingKey] = useState('');
  const [newUser, setNewUser] = useState<EditableUser | null>(null);
  const [isEmployeeModalVisible, setIsEmployeeModalVisible] = useState(false);

  useEffect(() => {
    fetchUsers();
    fetchClients();
    fetchEmployees();
  }, []);

  const fetchClients = async () => {
    try {
      const fetchedClients = await getClientsAsync();
      setClients(fetchedClients);
    } catch (error) {
      message.error("Failed to fetch clients");
      console.error("Error fetching clients:", error);
    }
  };

  const fetchEmployees = async () => {
    try {
      const fetchedEmployees = await getEmployeesAsync();
      setEmployees(fetchedEmployees);
    } catch (error) {
      message.error("Failed to fetch employees");
      console.error("Error fetching employees:", error);
    }
  };

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const fetchedUsers = await getUsersAsync();
      const formattedUsers = fetchedUsers.map((user) => ({
        ...user,
        key: user.id,
        userType: typeof user.userType === 'string' ? parseInt(user.userType) : user.userType // Ensure userType is number
      }));
      setUsers(formattedUsers);
    } catch (error) {
      message.error("Failed to fetch users");
      console.error("Error fetching users:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (value: string) => {
    setSearchText(value.toLowerCase());
  };

  const filteredUsers = users.filter(user => 
    user.name?.toLowerCase().includes(searchText.toLowerCase()) ||
    user.email?.toLowerCase().includes(searchText.toLowerCase()) ||
    user.contactNr?.toLowerCase().includes(searchText.toLowerCase())
  );

  const handleEdit = (record: EditableUser) => {
    // Ensure userType is properly set in form
    const userTypeValue = typeof record.userType === 'string' ? 
      parseInt(record.userType) : 
      record.userType;
    
    // Initialize form with current values
    form.setFieldsValue({ 
      name: record.name,
      email: record.email,
      contactNr: record.contactNr,
      userType: userTypeValue,
      clientID: record.clientID,
      password: '' // Empty password field when editing
    });
    setEditingKey(record.key);
  };

  const handleInputChange = (key: string, field: keyof EditableUser, value: string | number) => {
    if (editingKey === key) {
      // Convert userType to number if it's the userType field
      const processedValue = field === 'userType' ? Number(value) : value;
      
      // Update form value
      form.setFieldValue(field, processedValue);

      // Update local state
      if (newUser?.key === key) {
        setNewUser(prev => prev ? { ...prev, [field]: processedValue } : null);
      } else {
        setUsers(prev =>
          prev.map(user => 
            user.key === key 
              ? { ...user, [field]: processedValue }
              : user
          )
        );
      }
    }
  };

  const handleSave = async (key: string) => {
    try {
      console.log('Validating form fields...');
      const values = await form.validateFields();
      console.log('Form values:', values);
      
      const userType = Number(values.userType);
      console.log('UserType converted to:', userType);

      if (newUser && key === newUser.key) {
        console.log('Creating new user...');
        if (!values.password || !values.email || !values.name || !values.contactNr) {
          message.error("All fields are required for new users");
          return;
        }

        try {
          // For all user types, proceed with user creation directly
          await createUser(values, userType);
        } catch (error) {
          console.error('Error creating user:', error);
          if (error instanceof Error) {
            message.error(error.message);
          } else {
            message.error("Failed to create user");
          }
        }
      } else {
        // Handle existing user update
        console.log('Updating existing user...');
        const existingUser = users.find(u => u.key === key);
        if (!existingUser) {
          message.error("User not found");
          return;
        }

        try {
          const { client, employee, ...cleanUser } = existingUser;
          const updateData = {
            ...cleanUser,
            name: values.name,
            email: values.email,
            contactNr: values.contactNr,
            userType: userType,
            clientID: userType === UserType.Client ? values.clientID : null,
            employeeID: (userType === UserType.Employee || userType === UserType.SiteManager) ? values.employeeID : null,
            ...(values.password ? { password: values.password } : {})
          };

          await updateUserAsync(updateData);
          message.success("User updated successfully");
          setEditingKey('');
          form.resetFields();
          await fetchUsers();
        } catch (error) {
          console.error('Error updating user:', error);
          if (error instanceof Error) {
            message.error(error.message);
          } else {
            message.error("Failed to update user");
          }
        }
      }
    } catch (error) {
      console.error('Form validation or general error:', error);
      message.error("Failed to save user");
    }
  };

  const createUser = async (values: any, userType: UserType) => {
    const hashedPassword = await hashString(values.password || 'defaultPassword123'); // Default password if not provided
    
    // Generate a random string for unique email
    const randomString = Math.random().toString(36).substring(2, 8);
    
    // Set default values for required fields
    const defaultValues = {
      name: 'New User',  // Default name
      contactNr: values.contactNr || '0000000000',  // Default contact number
      email: values.email?.toLowerCase() || `user_${Date.now()}_${randomString}@oxigin.com`,  // Unique email with timestamp and random string
    };

    // If creating an employee user, first create the employee record
    if (userType === UserType.Employee || userType === UserType.SiteManager) {
      try {
        // Create employee record with minimal required fields
        const newEmployee = await addEmployeeAsync({
          name: defaultValues.name,
          contactNo: defaultValues.contactNr
        });

        // Create user with all required fields
        await CreateUserAsAdmin(
          defaultValues.name,
          defaultValues.contactNr,
          defaultValues.email,
          hashedPassword,
          userType,
          null, // No client ID for employee
          newEmployee.id // Link to the newly created employee
        );

        message.success("Employee user created successfully. Please update details later.");
      } catch (error) {
        console.error('Error creating employee user:', error);
        throw error;
      }
    } else {
      // Create regular user with all required fields
      await CreateUserAsAdmin(
        defaultValues.name,
        defaultValues.contactNr,
        defaultValues.email,
        hashedPassword,
        userType,
        values.clientID
      );
      message.success("User created successfully. Please update details later.");
    }

    setNewUser(null);
    setEditingKey('');
    form.resetFields();
    await fetchUsers();
  };

  const handleEmployeeModalCancel = () => {
    setIsEmployeeModalVisible(false);
    employeeForm.resetFields();
  };

  const handleCancel = () => {
    setEditingKey('');
    setNewUser(null);
    form.resetFields();
  };

  const handleDelete = async (key: string) => {
    const userToDelete = users.find(u => u.key === key);
    if (!userToDelete) {
      message.error("User not found");
      return;
    }

    Modal.confirm({
      title: 'Are you sure you want to delete this user?',
      content: `This will permanently delete the user "${userToDelete.name}". This action cannot be undone.`,
      okText: 'Yes',
      okType: 'danger',
      cancelText: 'No',
      onOk: async () => {
        try {
          console.log('Attempting to delete user:', userToDelete);
          setLoading(true);
          await deleteUserAsync(userToDelete.id);
          console.log('User deleted successfully');
          message.success("User deleted successfully");
          await fetchUsers();
        } catch (error) {
          console.error('Error deleting user:', error);
          if (error instanceof Error) {
            message.error(`Failed to delete user: ${error.message}`);
          } else {
            message.error("Failed to delete user");
          }
        } finally {
          setLoading(false);
        }
      },
    });
  };

  const handleAddUser = () => {
    const newKey = crypto.randomUUID();
    const newUserData = {
      key: newKey,
      id: newKey,
      name: "",
      email: "",
      contactNr: "",
      password: "",
      userType: UserType.BaseUser, // Default to BaseUser
      clientID: null
    };
    setNewUser(newUserData);
    
    // Initialize form with empty values and default user type
    form.setFieldsValue({
      name: "",
      email: "",
      contactNr: "",
      password: "",
      userType: UserType.BaseUser,
      clientID: null
    });
    
    setEditingKey(newKey);
  };

  const columns: ColumnType<EditableUser>[] = [
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      sorter: (a, b) => (a.name || '').localeCompare(b.name || ''),
      render: (text, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="name"
            style={{ margin: 0 }}
            rules={[{ required: true, message: 'Name is required' }]}
          >
            <Input 
              onChange={(e) => handleInputChange(record.key, "name", e.target.value)}
              placeholder="Enter name"
            />
          </Form.Item>
        ) : (
          text
        ),
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
      sorter: (a, b) => (a.email || '').localeCompare(b.email || ''),
      render: (text, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="email"
            style={{ margin: 0 }}
            rules={[
              { required: true, message: 'Email is required' },
              { type: 'email', message: 'Please enter a valid email' }
            ]}
          >
            <Input 
              onChange={(e) => handleInputChange(record.key, "email", e.target.value)}
              placeholder="Enter email"
            />
          </Form.Item>
        ) : (
          text
        ),
    },
    {
      title: "Contact Number",
      dataIndex: "contactNr",
      key: "contactNr",
      render: (text, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="contactNr"
            style={{ margin: 0 }}
            rules={[{ required: true, message: 'Contact number is required' }]}
          >
            <Input 
              onChange={(e) => handleInputChange(record.key, "contactNr", e.target.value)}
              placeholder="Enter contact number"
            />
          </Form.Item>
        ) : (
          text
        ),
    },
    {
      title: "Client",
      dataIndex: "clientID",
      key: "clientID",
      render: (_text, record) => {
        // Don't show this column for employee users
        if (record.userType === UserType.Employee || record.userType === UserType.SiteManager) {
          return null;
        }

        const client = clients.find(c => c.id === record.clientID);
        return editingKey === record.key && record.userType === UserType.Client ? (
          <Form.Item
            name="clientID"
            style={{ margin: 0 }}
            rules={[{ required: record.userType === UserType.Client, message: 'Client is required for client users' }]}
          >
            <Select
              allowClear
              placeholder="Select client"
              onChange={(value) => handleInputChange(record.key, "clientID", value)}
              style={{ width: '100%' }}
            >
              {clients.map(client => (
                <Select.Option key={client.id} value={client.id}>
                  {client.companyName}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
        ) : (
          client?.companyName || '-'
        );
      },
    },
    {
      title: "Employee",
      dataIndex: "employeeID",
      key: "employeeID",
      render: (_text, record) => {
        // Don't show this column for new users or non-employee users
        if (record.id === newUser?.id || 
            (record.userType !== UserType.Employee && record.userType !== UserType.SiteManager)) {
          return null;
        }

        const employee = employees.find(e => e.id === record.employeeID);
        return editingKey === record.key ? (
          <Form.Item
            name="employeeID"
            style={{ margin: 0 }}
            rules={[{ required: true, message: 'Employee is required for employee users' }]}
          >
            <Select
              allowClear
              placeholder="Select employee"
              onChange={(value) => handleInputChange(record.key, "employeeID", value)}
              style={{ width: '100%' }}
            >
              {employees.map(employee => (
                <Select.Option key={employee.id} value={employee.id}>
                  {employee.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
        ) : (
          employee?.name || '-'
        );
      },
    },
    {
      title: "Password",
      dataIndex: "password",
      key: "password",
      render: (text, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="password"
            style={{ margin: 0 }}
            rules={[{ required: record.id === newUser?.id, message: 'Password is required for new users' }]}
          >
            <Input.Password 
              placeholder={record.id === newUser?.id ? "Enter password" : "Leave blank to keep current password"}
              onChange={(e) => handleInputChange(record.key, "password", e.target.value)}
            />
          </Form.Item>
        ) : (
          text ? "••••••••" : ""
        ),
    },
    {
      title: "User Type",
      dataIndex: "userType",
      key: "userType",
      filters: Object.entries(UserType)
        .filter(([key]) => isNaN(Number(key)))
        .map(([key, value]) => ({
          text: key,
          value: Number(value)
        })),
      onFilter: (value, record) => {
        const recordType = Number(record.userType);
        const filterValue = Number(value);
        return recordType === filterValue;
      },
      render: (type: UserType, record) => {
        const numericType = Number(type);
        if (editingKey === record.key) {
          return (
            <Form.Item
              name="userType"
              style={{ margin: 0 }}
              rules={[{ required: true, message: 'User type is required' }]}
            >
              <Select
                onChange={(value) => handleInputChange(record.key, "userType", value)}
                style={{ width: '100%' }}
              >
                {Object.entries(UserType)
                  .filter(([key]) => isNaN(Number(key)))
                  .map(([key, value]) => (
                    <Select.Option key={value} value={Number(value)}>
                      {key}
                    </Select.Option>
                  ))}
              </Select>
            </Form.Item>
          );
        }
        return UserType[numericType] || 'Unknown';
      },
    },
    {
      title: "Actions",
      key: "actions",
      render: (_, record) => {
        const editing = editingKey === record.key;
        return editing ? (
          <div style={{ display: "flex", gap: 10 }}>
            <Button 
              type="text" 
              icon={<CheckOutlined style={{ color: "green" }} />} 
              onClick={() => handleSave(record.key)}
            />
            <Button 
              type="text" 
              icon={<CloseOutlined style={{ color: "red" }} />} 
              onClick={handleCancel}
            />
          </div>
        ) : (
          <div style={{ display: "flex", gap: 10 }}>
            <Button 
              type="text" 
              icon={<EditOutlined />} 
              disabled={editingKey !== ''} 
              onClick={() => handleEdit(record)}
            />
            <Button 
              type="text" 
              icon={<MinusCircleOutlined style={{ color: "red" }} />} 
              onClick={() => handleDelete(record.key)}
            />
          </div>
        );
      },
    },
  ];

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20, display: "flex", flexDirection: "column", alignItems: "center" }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px", width: "100%" }}>
          <h2 style={{ margin: 0, textAlign: "center", width: "100%" }}>System Users</h2>
        </Header>

        <Content style={{ flex: 1, padding: 20, width: "100%" }}>
          <Card title="All Users" style={{ width: "100%" }}>
            <div style={{ marginBottom: 16, display: "flex", justifyContent: "space-between" }}>
              <Button 
                type="primary" 
                icon={<PlusOutlined />} 
                onClick={handleAddUser} 
                disabled={editingKey !== ''} // Only disable when editing, not when newUser exists
              >
                Add User
              </Button>
              <Search
                placeholder="Search by name, email, or contact number"
                allowClear
                enterButton={<SearchOutlined />}
                style={{ width: 300 }}
                onChange={(e) => handleSearch(e.target.value)}
              />
            </div>

            <Form form={form} component={false}>
              <Table
                columns={columns}
                dataSource={[...filteredUsers, ...(newUser ? [newUser] : [])] /* Add new user at bottom */}
                rowKey="id"
                loading={loading}
                pagination={{ 
                  pageSize: 10,
                  position: ['bottomCenter']
                }}
              />
            </Form>

            <div style={{ marginTop: 16 }}>
              <Button onClick={() => navigate(-1)}>Back</Button>
            </div>
          </Card>
        </Content>
      </Card>

      <Modal
        title="Employee Details"
        open={isEmployeeModalVisible}
        onOk={() => handleSave(newUser?.key || '')}
        onCancel={handleEmployeeModalCancel}
        width={600}
      >
        <Form
          form={employeeForm}
          layout="vertical"
        >
          <Form.Item
            name="idNumber"
            label="ID Number"
            rules={[{ required: true, message: 'ID Number is required' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="address"
            label="Address"
            rules={[{ required: true, message: 'Address is required' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="contactNo"
            label="Contact Number"
            rules={[{ required: true, message: 'Contact number is required' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="bankName"
            label="Bank Name (Optional)"
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="accountHolderName"
            label="Account Holder Name (Optional)"
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="branchCode"
            label="Branch Code (Optional)"
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="accountNumber"
            label="Account Number (Optional)"
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </Layout>
  );
};

export default AdminUsers;