import React, { useState, useEffect } from "react";
import { Layout, Card, Table, Button, Input, message, Form, Select, Modal } from "antd";
import { EditOutlined, MinusCircleOutlined, PlusOutlined, CheckOutlined, CloseOutlined, SearchOutlined, EyeOutlined, EyeInvisibleOutlined } from "@ant-design/icons";

import { getUsersAsync, updateUserAsync, deleteUserAsync, CreateUserAsAdmin } from "../services/data/user";
import { getClientsAsync } from "../services/data/client";
import { addEmployeeAsync } from "../services/data/employee";
import type { User } from "../models/userModels";
import type { Client } from "../models/clientModels";
import type { Employee } from "../models/employeeModels";
import type { ColumnType } from "antd/es/table";
import { UserType } from "../enums/userTypes";

const { Content } = Layout;
const { Search } = Input;

interface EditableUser extends User {
  key: string;
  client?: Client | null;
  employee?: Employee | null;
}

const AdminUsers: React.FC = () => {
  const [form] = Form.useForm();
  const [users, setUsers] = useState<EditableUser[]>([]);
  const [clients, setClients] = useState<Client[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchValue, setSearchValue] = useState<string>("");
  const [editingKey, setEditingKey] = useState('');
  const [newUser, setNewUser] = useState<EditableUser | null>(null);

  useEffect(() => {
    fetchUsers();
    fetchClients();
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

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const fetchedUsers = await getUsersAsync();
      const formattedUsers = fetchedUsers.map((user) => ({
        ...user,
        key: user.id,
        userType: typeof user.userType === 'string' ? parseInt(user.userType) : user.userType
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
    setSearchValue(value.toLowerCase());
  };

  const filteredUsers = users.filter(user => 
    user.name?.toLowerCase().includes(searchValue.toLowerCase()) ||
    user.email?.toLowerCase().includes(searchValue.toLowerCase()) ||
    user.contactNr?.toLowerCase().includes(searchValue.toLowerCase())
  );

  const handleEdit = (record: EditableUser) => {
    const userTypeValue = typeof record.userType === 'string' ? 
      parseInt(record.userType) : 
      record.userType;
    
    form.setFieldsValue({ 
      id: record.id,
      name: record.name?.trim(),
      email: record.email?.trim(),
      contactNr: record.contactNr?.trim(),
      userType: userTypeValue,
      clientID: record.clientID,
      password: record.password
    });
    setEditingKey(record.key);
  };

  const handleUserTypeChange = (value: number) => {
    // If the user type is not client, clear the client selection
    if (value !== UserType.Client) {
      form.setFieldValue('clientID', null);
    }
    // Force a re-render to update the client dropdown state
    setUsers([...users]);
  };

  const handleSave = async (key: string) => {
    try {
      console.log('Validating form fields...');
      const values = await form.validateFields();
      console.log('Form values:', values);
      
      // Find the existing user to get current values
      const existingUser = users.find(u => u.key === key);
      if (!existingUser && !newUser) {
        message.error("User not found");
        return;
      }

      // Use existing userType if not changed
      const userType = values.userType !== undefined ? Number(values.userType) : existingUser?.userType ?? UserType.BaseUser;
      console.log('UserType converted to:', userType);

      if (isNaN(userType)) {
        message.error("Invalid user type selected");
        return;
      }

      // Validate required fields
      if (!values.name?.trim() || !values.email?.trim() || !values.contactNr?.trim()) {
        message.error("Name, email and contact number are required");
        return;
      }

      // Validate email format
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(values.email.trim())) {
        message.error("Please enter a valid email address");
        return;
      }

      // Validate client selection for client users
      if (userType === UserType.Client && !values.clientID) {
        message.error("Please select a client for client users");
        return;
      }

      let employeeID = values.employeeID;

      // If user type is Employee or SiteManager, create a basic employee record
      if ((userType === UserType.Employee || userType === UserType.SiteManager) && !employeeID) {
        try {
          // Create new employee with minimal required information
          const newEmployee = await addEmployeeAsync({
            name: values.name.trim(),
            contactNo: values.contactNr.trim(), // Use the user's contact number
            // Other fields will be null/empty and can be edited later
          });

          employeeID = newEmployee.id;
        } catch (error) {
          console.error('Error creating employee:', error);
          message.error('Failed to create employee record');
          return;
        }
      }

      const userData = {
        name: values.name.trim(),
        email: values.email.toLowerCase().trim(),
        contactNr: values.contactNr.trim(),
        userType: userType,
        clientID: userType === UserType.Client ? values.clientID : null,
        employeeID: userType === UserType.Employee || userType === UserType.SiteManager ? employeeID : null,
        password: values.password
      };

      if (newUser && key === newUser.key) {
        console.log('Creating new user with data:', userData);
        try {
          await CreateUserAsAdmin(
            userData.name,
            userData.contactNr,
            userData.email,
            userData.password,
            userData.userType,
            userData.clientID,
            userData.employeeID
          );

          message.success('User created successfully');
          setNewUser(null);
        } catch (error) {
          console.error('Error creating user:', error);
          message.error('Failed to create user. Please check if the email is already in use.');
          return;
        }
      } else if (existingUser) {
        console.log('Updating existing user with data:', { ...userData, id: existingUser.id });
        try {
          const updatedUser = await updateUserAsync({
            id: existingUser.id,
            ...userData
          });
          
          console.log('User updated successfully:', updatedUser);
          message.success('User updated successfully');
          
          // Update the local users state with the updated user
          setUsers(prevUsers => 
            prevUsers.map(u => 
              u.id === updatedUser.id ? { ...updatedUser, key: updatedUser.id } : u
            )
          );
        } catch (error) {
          console.error('Error updating user:', error);
          if (error instanceof Error) {
            message.error(`Failed to update user: ${error.message}`);
          } else {
            message.error('Failed to update user');
          }
          return;
        }
      }

      setEditingKey('');
      form.resetFields();
      // Refresh the users list to get the latest data
      await fetchUsers();
    } catch (error) {
      console.error('Error saving user:', error);
      if (error instanceof Error) {
        message.error(`Failed to save user: ${error.message}`);
      } else {
        message.error('Failed to save user');
      }
    }
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
    const newUserData: EditableUser = {
      key: newKey,
      id: newKey,
      name: "",
      email: "",
      contactNr: "",
      password: "",
      userType: UserType.BaseUser,
      clientID: null,
      employeeID: null,
      client: null,
      employee: null
    };
    
    setNewUser(newUserData);
    setUsers(prev => [newUserData, ...prev]); // Add the new user to the beginning of the array
    
    // Initialize form with empty values and default user type
    form.setFieldsValue({
      name: "",
      email: "",
      contactNr: "",
      password: "",
      userType: UserType.BaseUser,
      clientID: null,
      employeeID: null
    });
    
    setEditingKey(newKey);
  };

  const columns: ColumnType<EditableUser>[] = [
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      width: '20%',
      sorter: (a, b) => (a.name || '').localeCompare(b.name || ''),
      render: (_, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="name"
            style={{ margin: 0 }}
            rules={[{ required: true, message: 'Name is required' }]}
            initialValue={record.name}
          >
            <Input />
          </Form.Item>
        ) : (
          record.name
        ),
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
      width: '20%',
      sorter: (a, b) => (a.email || '').localeCompare(b.email || ''),
      render: (_, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="email"
            style={{ margin: 0 }}
            rules={[
              { required: true, message: 'Email is required' },
              { type: 'email', message: 'Please enter a valid email' }
            ]}
            initialValue={record.email}
          >
            <Input />
          </Form.Item>
        ) : (
          record.email
        ),
    },
    {
      title: "Contact",
      dataIndex: "contactNr",
      key: "contactNr",
      width: '15%',
      render: (_, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="contactNr"
            style={{ margin: 0 }}
            rules={[{ required: true, message: 'Contact number is required' }]}
            initialValue={record.contactNr}
          >
            <Input />
          </Form.Item>
        ) : (
          record.contactNr
        ),
    },
    {
      title: "Client",
      dataIndex: "clientID",
      key: "clientID",
      width: '15%',
      render: (_text, record) => {
        // Get the current form values to check the current user type
        const currentUserType = form.getFieldValue('userType');
        const isClientType = currentUserType === UserType.Client;
        const client = clients.find(c => c.id === record.clientID);
        
        return editingKey === record.key ? (
          <Form.Item
            name="clientID"
            style={{ margin: 0 }}
            rules={[{ required: isClientType, message: 'Client is required for client users' }]}
            initialValue={record.clientID}
          >
            <Select
              allowClear
              placeholder="Select client"
              style={{ width: '100%' }}
              disabled={!isClientType}
              onChange={(value) => {
                // Update the form value when selection changes
                form.setFieldsValue({ clientID: value });
              }}
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
      title: "Password",
      dataIndex: "password",
      key: "password",
      width: '15%',
      render: (_, record) =>
        editingKey === record.key ? (
          <Form.Item
            name="password"
            style={{ margin: 0 }}
            rules={[
              {
                required: record.id === newUser?.id,
                message: 'Password is required for new users'
              }
            ]}
          >
            <Input.Password 
              placeholder="Update"
              iconRender={(visible) => (visible ? <EyeOutlined /> : <EyeInvisibleOutlined />)}
              style={{ width: '100%' }}
            />
          </Form.Item>
        ) : (
          <span style={{ color: '#999' }}>••••••••</span>
        ),
    },
    {
      title: "Type",
      dataIndex: "userType",
      key: "userType",
      width: '10%',
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
      render: (_, record) => {
        const numericType = Number(record.userType);
        if (editingKey === record.key) {
          return (
            <Form.Item
              name="userType"
              style={{ margin: 0 }}
              rules={[{ required: true, message: 'User type is required' }]}
              initialValue={numericType}
            >
              <Select
                style={{ width: '100%' }}
                onChange={handleUserTypeChange}
                options={Object.entries(UserType)
                  .filter(([key]) => isNaN(Number(key)))
                  .map(([key, value]) => ({
                    label: key,
                    value: Number(value)
                  }))}
              />
            </Form.Item>
          );
        }
        return UserType[numericType] || 'Unknown';
      },
    },
    {
      title: "Actions",
      key: "actions",
      width: '5%',
      render: (_, record) => {
        const editing = editingKey === record.key;
        return editing ? (
          <div style={{ display: "flex", gap: 4 }}>
            <Button 
              type="text" 
              size="small"
              icon={<CheckOutlined style={{ color: "green" }} />} 
              onClick={() => handleSave(record.key)}
            />
            <Button 
              type="text" 
              size="small"
              icon={<CloseOutlined style={{ color: "red" }} />} 
              onClick={handleCancel}
            />
          </div>
        ) : (
          <div style={{ display: "flex", gap: 4 }}>
            <Button 
              type="text" 
              size="small"
              icon={<EditOutlined />} 
              disabled={editingKey !== ''} 
              onClick={() => handleEdit(record)}
            />
            <Button 
              type="text" 
              size="small"
              icon={<MinusCircleOutlined style={{ color: "red" }} />} 
              onClick={() => handleDelete(record.key)}
            />
          </div>
        );
      },
    },
  ];

  return (
    <Layout className="min-h-screen flex justify-center items-center p-4">
      <Card className="responsive-card w-full max-w-[1200px]">
        <h2 className="page-title mb-4">Users</h2>

        <Content className="flex flex-col gap-4">
          <div className="flex justify-between items-center gap-4 mb-4">
            <Search
              placeholder="Search users..."
              onChange={(e) => handleSearch(e.target.value)}
              value={searchValue}
              allowClear
              style={{ width: '100%', maxWidth: 300 }}
              prefix={<SearchOutlined />}
            />
            <Button
              type="primary"
              icon={<PlusOutlined />}
              onClick={handleAddUser}
              disabled={editingKey !== ''}
            >
              Add User
            </Button>
          </div>

          <Form form={form}>
            <div className="responsive-table">
              <Table
                loading={loading}
                dataSource={filteredUsers}
                columns={columns}
                pagination={{ 
                  pageSize: 8,
                  position: ['bottomCenter']
                }}
                size="small"
                bordered
              />
            </div>
          </Form>
        </Content>
      </Card>
    </Layout>
  );
};

export default AdminUsers;