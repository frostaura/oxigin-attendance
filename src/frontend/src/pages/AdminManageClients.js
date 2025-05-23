import React, { useState } from "react";
import { Layout, Card, Table, Input, Button } from "antd";
import { EditOutlined, MinusCircleOutlined, PlusOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

const { Header, Content } = Layout;

const AdminManageClients = () => {
  const navigate = useNavigate();
  const [editingKey, setEditingKey] = useState(null);
  const [clients, setClients] = useState([
    { key: "1", companyId: "C001", companyName: "Tech Solutions", registrationNo: "123456789", address: "123 Tech St, Silicon Valley", contact: "555-1234", email: "contact@techsolutions.com" },
    { key: "2", companyId: "C002", companyName: "Creative Designs", registrationNo: "987654321", address: "456 Design Ave, NYC", contact: "555-5678", email: "contact@creativedesigns.com" },
  ]);

  const [newClient, setNewClient] = useState(null);
  const [searchValue, setSearchValue] = useState("");

  const handleEdit = (key) => {
    setEditingKey(key);
  };

  const handleSave = (key) => {
    if (newClient?.key === key) {
      setClients([...clients, newClient]);
      setNewClient(null);
    }
    setEditingKey(null);
  };

  const handleDelete = (key) => {
    setClients(clients.filter((client) => client.key !== key));
  };

  const handleAddClient = () => {
    if (!newClient) {
      const newKey = (clients.length + 1).toString();
      setNewClient({
        key: newKey,
        companyId: "",
        companyName: "",
        registrationNo: "",
        address: "",
        contact: "",
        email: "",
      });
      setEditingKey(newKey);
    }
  };

  const handleInputChange = (key, field, value) => {
    if (editingKey === key) {
      if (newClient?.key === key) {
        setNewClient({ ...newClient, [field]: value });
      } else {
        setClients((prev) =>
          prev.map((client) => (client.key === key ? { ...client, [field]: value } : client))
        );
      }
    }
  };

  const handleSearchChange = (e) => {
    setSearchValue(e.target.value);
  };

  const filteredClients = clients.filter(client =>
    client.companyName.toLowerCase().includes(searchValue.toLowerCase()) ||
    client.companyId.toLowerCase().includes(searchValue.toLowerCase())
  );

  const columns = [
  
    {
      title: "Company Name",
      dataIndex: "companyName",
      key: "companyName",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.companyName} onChange={(e) => handleInputChange(record.key, "companyName", e.target.value)} />
        ) : (
          text
        ),
    },
    {
      title: "Registration No.",
      dataIndex: "registrationNo",
      key: "registrationNo",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.registrationNo} onChange={(e) => handleInputChange(record.key, "registrationNo", e.target.value)} />
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
      title: "Email Address",
      dataIndex: "email",
      key: "email",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.email} onChange={(e) => handleInputChange(record.key, "email", e.target.value)} />
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
            <Button type="text" icon={<CloseOutlined style={{ color: "red" }} />} onClick={() => setNewClient(null)} />
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
      <Card style={{ width: "80%", padding: 20, display: "flex", flexDirection: "column", alignItems: "center" }}>
        {/* Page Header */}
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px", width: "100%" }}>
          <h2 style={{ margin: 0, textAlign: "center", width: "100%" }}>Manage Clients</h2>
        </Header>

        {/* Main Content */}
        <Content style={{ flex: 1, padding: 20, width: "100%" }}>
          <Card title="All Clients" style={{ width: "100%" }}>
            <Table columns={columns} dataSource={newClient ? [...filteredClients, newClient] : filteredClients} pagination={false} />

            {/* Bottom section */}
            <div style={{ display: "flex", justifyContent: "space-between", marginTop: 20 }}>
              {/* Add Client Button (Bottom Left) */}
              <Button type="dashed" icon={<PlusOutlined />} onClick={handleAddClient} disabled={!!newClient}>
                Add Client
              </Button>

              {/* Search Box (Bottom Right) */}
              <Input
                placeholder="Search by Company ID or Name"
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

export default AdminManageClients;
