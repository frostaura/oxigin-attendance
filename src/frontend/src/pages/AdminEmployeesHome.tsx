import React from "react";
import { Layout, Button, Card } from "antd";
import { useNavigate } from "react-router-dom";
import { TeamOutlined, ScheduleOutlined } from "@ant-design/icons";

const { Header } = Layout;


const AdminEmployeesHome: React.FC = () => {
  const navigate = useNavigate();

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center", width: "100%" }}>Employee Home Page</h2>
        </Header>

        {/* Bottom Buttons */}
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 20, marginTop: 20 }}>
          <Button 
            type="primary" 
            icon={<TeamOutlined />}
            onClick={() => navigate("/adminmanageemployees")} 
            style={{ width: "300px", height: "100px", fontSize: "18px" }}
            block
          >
            Manage Employees
          </Button>
          <Button 
            type="primary" 
            icon={<ScheduleOutlined />}
            onClick={() => navigate("/adminjoballocations")} 
            style={{ width: "300px", height: "100px", fontSize: "18px" }}
            block
          >
            Allocate Jobs
          </Button>
        </div>
      </Card>
    </Layout>
  );
};

export default AdminEmployeesHome;
