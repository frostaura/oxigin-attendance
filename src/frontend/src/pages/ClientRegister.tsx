import React from "react";
import { Link } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";
import { ClientRegistrationFormValues } from "../types";

const { Title, Text } = Typography;

const ClientRegister: React.FC = () => {
  const [form] = Form.useForm<ClientRegistrationFormValues>();

  const handleRegister = (values: ClientRegistrationFormValues) => {
    console.log("Registration Data:", values);
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: 400, textAlign: "center", padding: "20px 24px"}}>
        <Title level={2} style={{ marginBottom: 20 }}>Register</Title>
  
        <Form form={form} layout="vertical" onFinish={handleRegister}>
          <Form.Item label="Company Name" name="companyName" rules={[{ required: true, message: "Company name is required." }]}>
            <Input placeholder="Company name" />
          </Form.Item>
  
          <Form.Item label="Registration Number" name="registrationNumber" rules={[{ required: true, message: "Registration number is required." }]}>
            <Input placeholder="Company registration number" />
          </Form.Item>
  
          <Form.Item label="First Name" name="firstName" rules={[{ required: true, message: "First name is required." }]}>
            <Input placeholder="First name" />
          </Form.Item>

          <Form.Item label="Last Name" name="lastName" rules={[{ required: true, message: "Last name is required." }]}>
            <Input placeholder="Last name" />
          </Form.Item>
  
          <Form.Item label="Address" name="address" rules={[{ required: true, message: "Please enter your address!" }]}>
            <Input.TextArea placeholder="Enter your address" rows={3} />
          </Form.Item>
  
          <Form.Item label="Phone" name="phone" rules={[{ required: true, message: "Please enter your contact number!" }]}>
            <Input placeholder="Enter your contact number" />
          </Form.Item>
  
          <Form.Item label="Email" name="email" rules={[
            { required: true, message: "Please enter your email!" },
            { type: "email", message: "Please enter a valid email!" }
          ]}>
            <Input type="email" placeholder="Enter your email" />
          </Form.Item>
  
          <Form.Item label="Password" name="password" rules={[{ required: true, message: "Please enter your password!" }]}>
            <Input.Password placeholder="Enter your password" />
          </Form.Item>
  
          <Form.Item label="Confirm Password" name="confirmPassword" dependencies={['password']} 
            rules={[
              { required: true, message: "Please confirm your password!" },
              ({ getFieldValue }) => ({
                validator(_, value) {
                  if (!value || getFieldValue('password') === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject(new Error("Passwords do not match!"));
                },
              }),
            ]}
          >
            <Input.Password placeholder="Confirm your password" />
          </Form.Item>
  
          <Button type="primary" htmlType="submit" block>
            Register
          </Button>
        </Form>
  
        <Text style={{ marginTop: 10, display: "block" }}>Already have an account?</Text>
        <Link to="/">Sign In</Link>
      </Card>
    </div>
  );
}
  
export default ClientRegister; 