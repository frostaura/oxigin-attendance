import React from "react";
import { Layout, Card, Button } from "antd";
import { useNavigate } from "react-router-dom";
import { AppstoreAddOutlined, TeamOutlined, CustomerServiceOutlined } from "@ant-design/icons";

const { Header, Content } = Layout;

const AdminHome: React.FC = () => {
  const navigate = useNavigate();

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "30%", padding: 20 }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, alignItems: "center"}}>Admin Home</h2>
        </Header>

        <Content style={{ flex: 1, padding: 20, display: "flex", flexDirection: "column", alignItems: "center", gap: 20 }}>
          <Button type="primary" block icon={<AppstoreAddOutlined />} onClick={() => navigate("/adminjobshome")} style={{ width: "300px", height: "100px", fontSize: "18px" }}>Jobs</Button>
          <Button type="primary" block icon={<TeamOutlined />} onClick={() => navigate("/adminemployeeshome")} style={{ width: "300px", height: "100px", fontSize: "18px" }}>Employees</Button>
          <Button type="primary" block icon={<CustomerServiceOutlined />} onClick={() => navigate("/adminclienthome")} style={{ width: "300px", height: "100px", fontSize: "18px" }}>Clients</Button>
        </Content>
      </Card>
    </Layout>
  );
};

export default AdminHome;
