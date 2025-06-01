import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";
import { signIn } from "../authAPI";

const { Title, Text } = Typography;

interface LoginFormValues {
  email: string;
  password: string;
}

const ClientSignIn: React.FC = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleLogin = async (values: LoginFormValues): Promise<void> => {
    try {
      const { signIn } = await import("../authAPI");
      const user = await signIn(values.email, values.password);
      console.log("Logged in user:", user);
    } catch (error) {
      console.error("Login failed:", error);
    }
  };
  
  


  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
      <Card style={{ width: 400, textAlign: "center", padding: 20 }}>
        <Title level={2}>Sign In</Title>

        {/* Logo Placeholder */}
        <div style={{ width: 80, height: 80, borderRadius: "50%", backgroundColor: "#ddd", margin: "10px auto" }}></div>

        <Form<LoginFormValues> layout="vertical" onFinish={handleLogin}>
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

        <Text style={{ marginTop: 10, display: "block" }}>Don't have an account?</Text>
        <Link to="/clientregister">Register</Link>
      </Card>
    </div>
  );
};

export default ClientSignIn; 