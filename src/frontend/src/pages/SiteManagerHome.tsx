import React from "react";
import { Layout, Card, Button } from "antd";
import { useNavigate } from "react-router-dom";
import { ClockCircleOutlined } from "@ant-design/icons";

const { Header, Content } = Layout;

const SiteManagerHome: React.FC = () => {
  const navigate = useNavigate();

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "30%", padding: 20 }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, alignItems: "center"}}>Home Page</h2>
        </Header>

        {/* Content Layout */}
        <Content style={{ flex: 1, padding: 20, display: "flex", flexDirection: "column", alignItems: "center", gap: 20 }}>
          {/* Vertical Buttons */}
          <Button type="primary" block icon={<ClockCircleOutlined />} onClick={() => navigate("/sitemanagercheckin")} style={{ width: "300px", height: "100px", fontSize: "18px" }} >Check In</Button>
          <Button type="primary" block icon={<ClockCircleOutlined />} onClick={() => navigate("/sitemanagercheckout" )} style={{ width: "300px", height: "100px", fontSize: "18px" }} >Check Out</Button>
        </Content>
      </Card>
    </Layout>
  );
};

export default SiteManagerHome; 