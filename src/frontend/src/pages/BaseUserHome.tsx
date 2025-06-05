import React from "react";
import { Layout, Card, Button } from "antd";
import { useNavigate } from "react-router-dom";

const { Content } = Layout;

const BaseUserHome: React.FC = () => {
  const navigate = useNavigate();

  return (
    <Layout className="min-h-screen flex justify-center items-center">
      <Card className="responsive-card w-full" style={{ maxWidth: '1200px' }}>
        <h2 className="page-title">Home Page</h2>

        {/* Content Layout */}
        <Content className="flex flex-col items-center gap-4 p-4">
          <Button type="primary" block onClick={() => navigate("/baseusertimesheets" )}>Timesheets</Button>
        </Content>
      </Card>
    </Layout>
  );
};

export default BaseUserHome; 