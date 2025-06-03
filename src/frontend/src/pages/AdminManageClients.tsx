import React, { useState } from "react";
import type { ChangeEvent } from "react";
import { Layout, Card, Table, Input, Button } from "antd";
import { EditOutlined, MinusCircleOutlined, PlusOutlined, CheckOutlined, CloseOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import type { ClientData } from "../models/clientModels";

const { Header, Content } = Layout;

interface EditableClientData extends Omit<ClientData, 'phone'> {
  registrationNo: string;
  address: string;
  contact: string;
}

const AdminManageClients: React.FC = () => {
  const navigate = useNavigate();
  const [editingKey, setEditingKey] = useState<string | null>(null);
  const [clients, setClients] = useState<EditableClientData[]>([
    { key: "1", id: "C001", name: "Tech Solutions", registrationNo: "123456789", address: "123 Tech St, Silicon Valley", contact: "555-1234", email: "contact@techsolutions.com", company: "Tech Solutions" },
    { key: "2", id: "C002", name: "Creative Designs", registrationNo: "987654321", address: "456 Design Ave, NYC", contact: "555-5678", email: "contact@creativedesigns.com", company: "Creative Designs" },
  ]);

  const [newClient, setNewClient] = useState<EditableClientData | null>(null);
  const [searchValue, setSearchValue] = useState<string>("");

  const handleEdit = (key: string) => {
    setEditingKey(key);
  };

  const handleSave = (key: string) => {
    if (newClient?.key === key) {
      setClients([...clients, newClient]);
      setNewClient(null);
    }
    setEditingKey(null);
  };

  const handleDelete = (key: string) => {
    setClients(clients.filter((client) => client.key !== key));
  };

  const handleAddClient = () => {
    if (!newClient) {
      const newKey = (clients.length + 1).toString();
      setNewClient({
        key: newKey,
        id: "",
        name: "",
        registrationNo: "",
        address: "",
        contact: "",
        email: "",
        company: "",
      });
      setEditingKey(newKey);
    }
  };

  const handleInputChange = (key: string, field: keyof EditableClientData, value: string) => {
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

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchValue(e.target.value);
  };

  const filteredClients = clients.filter(client =>
    client.name.toLowerCase().includes(searchValue.toLowerCase()) ||
    client.id.toLowerCase().includes(searchValue.toLowerCase())
  );

  const columns: ColumnsType<EditableClientData> = [
    {
      title: "Company Name",
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
              <div style={{ display: "flex", gap: 10 }}>
                {/* Back Button */}
                <Button onClick={() => navigate(-1)}>Back</Button>
                {/* Add Client Button */}
                <Button type="dashed" icon={<PlusOutlined />} onClick={handleAddClient} disabled={!!newClient}>
                  Add Client
                </Button>
              </div>

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