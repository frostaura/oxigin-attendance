import React, { useState, useEffect } from "react";
//import type { ChangeEvent } from "react";
import { Layout, Card, Table, Input, Button, message, Modal } from "antd";
import { EditOutlined, MinusCircleOutlined, PlusOutlined, CheckOutlined, CloseOutlined, SearchOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import type { ColumnsType } from "antd/es/table";
import type { ClientData } from "../models/clientModels";
import { getClientsAsync, addClientAsync, updateClientAsync, deleteClientAsync } from "../services/data/clients";

const { Content } = Layout;
const { Search } = Input;

const AdminManageClients: React.FC = () => {
  const navigate = useNavigate();
  const [editingKey, setEditingKey] = useState<string | null>(null);
  const [clients, setClients] = useState<ClientData[]>([]);
  const [loading, setLoading] = useState(false);
  const [newClient, setNewClient] = useState<ClientData | null>(null);
  const [searchValue, setSearchValue] = useState<string>("");

  useEffect(() => {
    fetchClients();
  }, []);

  const fetchClients = async () => {
    try {
      setLoading(true);
      const fetchedClients = await getClientsAsync();
      console.log('Fetched clients:', fetchedClients);
      setClients(fetchedClients);
    } catch (error) {
      message.error("Failed to fetch clients");
      console.error("Error fetching clients:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (key: string) => {
    setEditingKey(key);
  };

  const handleSave = async (key: string) => {
    try {
      setLoading(true);
      const clientToSave = newClient?.key === key ? newClient : clients.find(client => client.key === key);
      
      if (!clientToSave) {
        throw new Error("Client data not found");
      }

      // Validate required fields
      if (!clientToSave.registrationNo) {
        message.error("Registration number is required");
        return;
      }

      if (!clientToSave.company) {
        message.error("Company name is required");
        return;
      }

      console.log('Saving client:', clientToSave);

      if (newClient?.key === key) {
        // For new client
        await addClientAsync(clientToSave);
        message.success("Client added successfully");
        setNewClient(null);
      } else {
        // For updates
        console.log('Updating existing client:', clientToSave);
        await updateClientAsync(clientToSave);
        message.success("Client updated successfully");
      }

      await fetchClients();
      setEditingKey(null);
    } catch (error) {
      console.error('Full error details:', error);
      if (error instanceof Error) {
        message.error(error.message);
      } else {
        message.error("Failed to save client");
      }
      console.error("Error saving client:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (key: string) => {
    Modal.confirm({
      title: 'Are you sure you want to delete this client?',
      content: 'This action cannot be undone.',
      okText: 'Yes',
      okType: 'danger',
      cancelText: 'No',
      onOk: async () => {
        try {
          setLoading(true);
          await deleteClientAsync(key);
          message.success("Client removed successfully");
          await fetchClients();
        } catch (error) {
          message.error("Failed to delete client");
          console.error("Error deleting client:", error);
        } finally {
          setLoading(false);
        }
      },
    });
  };

  const handleAddClient = () => {
    if (!newClient) {
      const newKey = crypto.randomUUID();
      setNewClient({
        key: newKey,
        id: newKey,
        name: "",
        email: "",
        company: "",
        phone: "",
        registrationNo: "",
        address: ""
      });
      setEditingKey(newKey);
    }
  };

  const handleInputChange = (key: string, field: keyof ClientData, value: string) => {
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

  const handleSearch = (value: string) => {
    setSearchValue(value.toLowerCase());
  };

  const filteredClients = clients.filter(client =>
    (client.company?.toLowerCase() || '').includes(searchValue) ||
    (client.registrationNo?.toLowerCase() || '').includes(searchValue) ||
    (client.address?.toLowerCase() || '').includes(searchValue)
  );

  const columns: ColumnsType<ClientData> = [
    {
      title: "Company Name",
      dataIndex: "company",
      key: "company",
      sorter: (a, b) => (a.company || '').localeCompare(b.company || ''),
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.company} onChange={(e) => handleInputChange(record.key, "company", e.target.value)} />
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
      title: "Phone",
      dataIndex: "phone",
      key: "phone",
      render: (text, record) =>
        editingKey === record.key ? (
          <Input value={record.phone} onChange={(e) => handleInputChange(record.key, "phone", e.target.value)} />
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
            <Button type="text" icon={<CloseOutlined style={{ color: "red" }} />} onClick={() => {
              setEditingKey(null);
              if (newClient?.key === record.key) {
                setNewClient(null);
              }
            }} />
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
    <Layout className="min-h-screen flex justify-center items-center p-4">
      <Card className="responsive-card w-full max-w-[1200px]">
        <h2 className="page-title mb-4">Manage Clients</h2>

        <Content className="flex flex-col gap-4">
          <div className="flex justify-between items-center gap-4 mb-4">
            <Button type="primary" icon={<PlusOutlined />} onClick={handleAddClient} disabled={!!newClient}>
              Add Client
            </Button>
            <Search
              placeholder="Search by Company Name, Reg No, or Address"
              allowClear
              enterButton={<SearchOutlined />}
              style={{ width: '100%', maxWidth: 300 }}
              onChange={(e) => handleSearch(e.target.value)}
            />
          </div>

          <div className="responsive-table">
            <Table 
              columns={columns} 
              dataSource={newClient ? [...filteredClients, newClient] : filteredClients} 
              pagination={{ 
                pageSize: 8,
                position: ['bottomCenter']
              }}
              loading={loading}
              scroll={{ x: 'max-content' }}
              size="middle"
              bordered
            />
          </div>

          <div className="mt-4">
            <Button onClick={() => navigate(-1)}>Back</Button>
          </div>
        </Content>
      </Card>
    </Layout>
  );
};

export default AdminManageClients; 