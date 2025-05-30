import React from "react";
import { Link } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";

const { Title, Text } = Typography;

const AdminSignIn = () => {
  const handleLogin = (values) => {
    console.log("Login Data:", values);
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
      <Card style={{ width: 400, textAlign: "center", padding: 20 }}>
        <Title level={2}>Sign In</Title>

        {/* Logo Placeholder */}
        <div style={{ width: 80, height: 80, borderRadius: "50%", backgroundColor: "#ddd", margin: "10px auto" }}></div>

        <Form layout="vertical" onFinish={handleLogin}>
          <Form.Item label="Email" name="email" rules={[{ required: true, message: "Please enter your email!" }]}>
            <Input type="email" placeholder="Enter your email" />
          </Form.Item>

          <Form.Item label="Password" name="password" rules={[{ required: true, message: "Please enter your password!" }]}>
            <Input.Password placeholder="Enter your password" />
          </Form.Item>

          <Button type="primary" htmlType="submit" block>
            Login
          </Button>
        </Form>
      </Card>
    </div>
  );
};

export default AdminSignIn;
