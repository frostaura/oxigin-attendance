import React from "react";
import { Layout, Card, Button } from "antd";
import { useNavigate } from "react-router-dom";
import { ClockCircleOutlined } from "@ant-design/icons";

const { Content } = Layout;

const SiteManagerHome: React.FC = () => {
  const navigate = useNavigate();

  const buttonStyle = {
    width: '300px',
    height: '100px',
    fontSize: '18px'
  };

  return (
    <Layout className="min-h-screen flex justify-center items-center">
      <Card className="responsive-card w-full" style={{ maxWidth: '600px' }}>
        <h2 className="page-title">Home Page</h2>

        {/* Content Layout */}
        <Content className="flex flex-col items-center gap-4 p-4">
          <Button 
            type="primary" 
            block 
            icon={<ClockCircleOutlined />} 
            onClick={() => navigate("/sitemanagercheckin")} 
            style={buttonStyle}
          >
            Check In
          </Button>
          <Button 
            type="primary" 
            block 
            icon={<ClockCircleOutlined />} 
            onClick={() => navigate("/sitemanagercheckout")} 
            style={buttonStyle}
          >
            Check Out
          </Button>
        </Content>
      </Card>
    </Layout>
  );
};

export default SiteManagerHome; 