import React from "react";
import { Layout, Card, Typography } from "antd";

const { Title, Paragraph } = Typography;

const UnassignedUser: React.FC = () => {
  return (
    <Layout
      style={{
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: "#f0f2f5",
        position: "relative",
      }}
    >
      <Card
        style={{
          width: "400px",
          padding: "24px",
          boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
          textAlign: "center",
          position: "relative",
          top: "-60px", // Moves the card upward
        }}
      >
        <Title level={3} style={{ marginBottom: "16px" }}>
          Unassigned User
        </Title>

        <Paragraph>Please wait for admin approval...</Paragraph>
      </Card>
    </Layout>
  );
};

export default UnassignedUser;