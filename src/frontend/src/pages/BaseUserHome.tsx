import React from "react";
import { Layout as AntLayout, Card } from "antd";

const { Header, Content } = AntLayout;

const BaseUserHome: React.FC = () => {
  return (
    <AntLayout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "30%", padding: 20 }}>
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", borderBottom: "1px solid #ddd", padding: "0 20px" }}>
          <h2 style={{ margin: 0, alignItems: "center"}}>User Home</h2>
        </Header>
        <Content style={{ flex: 1, padding: 20, display: "flex", flexDirection: "column", alignItems: "center", gap: 20 }}>
          {/* Add your content here */}
        </Content>
      </Card>
    </AntLayout>
  );
};

export default BaseUserHome;
