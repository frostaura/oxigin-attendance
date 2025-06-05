import React from "react";
import { Layout, Card, Button } from "antd";
import { useNavigate } from "react-router-dom";
import { AppstoreAddOutlined, TeamOutlined, CustomerServiceOutlined, UserOutlined } from "@ant-design/icons";

const { Content } = Layout;

const AdminHome: React.FC = () => {
  const navigate = useNavigate();

  const buttonStyle = {
    height: '100px',
    fontSize: '18px',
    maxWidth: '300px',
    width: '100%'
  };

  return (
    <Layout className="min-h-screen flex justify-center items-center">
      <Card className="responsive-card w-full" style={{ maxWidth: '600px' }}>
        <h2 className="page-title mb-8">Admin Home</h2>

        {/* Content Layout */}
        <Content className="flex flex-col items-center gap-4">
          {/* Vertical Buttons */}
          <Button 
            type="primary" 
            block 
            icon={<AppstoreAddOutlined/>} 
            onClick={() => navigate("/adminjobshome")} 
            style={buttonStyle}
          >
            Jobs
          </Button>
          <Button 
            type="primary" 
            block 
            icon={<TeamOutlined/>} 
            onClick={() => navigate("/adminemployeeshome")} 
            style={buttonStyle}
          >
            Employees
          </Button>
          <Button 
            type="primary" 
            block 
            icon={<CustomerServiceOutlined/>} 
            onClick={() => navigate("/adminmanageclients")} 
            style={buttonStyle}
          >
            Clients
          </Button>
          <Button 
            type="primary" 
            block 
            icon={<UserOutlined/>} 
            onClick={() => navigate("/adminusers")} 
            style={buttonStyle}
          >
            Users
          </Button>
        </Content>
      </Card>
    </Layout>
  );
};

export default AdminHome; 