import React from "react";
import { Form, Input, Button, Typography, Card } from "antd";
import type { ClientRegistrationFormValues } from "../types";

const { Title } = Typography;

const UpdateClient: React.FC = () => {
  const [form] = Form.useForm<ClientRegistrationFormValues>();

  const handleRegister = (values: ClientRegistrationFormValues) => {
    console.log("Updated Data:", values);
  };

  return (
    <div
      style={{
        backgroundColor: "#f0f2f5",
        minHeight: "100vh", 
        display: "flex",
        justifyContent: "center",
        paddingTop: 24, 
        paddingBottom: 40,
      }}
    >
      <Card
        style={{
          width: 400,
          textAlign: "center",
        }}
      >
        <Title level={4} style={{ textAlign: "center", marginBottom: 20 }}>
          Update Details
        </Title>

        <Form form={form} layout="vertical" onFinish={handleRegister}>
          <Form.Item
            label="Company Name"
            name="companyName"
            rules={[{ required: true, message: "Company name is required." }]}
          >
            <Input placeholder="Company name" />
          </Form.Item>

          <Form.Item
            label="Registration Number"
            name="registrationNumber"
            rules={[{ required: true, message: "Registration number is required." }]}
          >
            <Input placeholder="Company registration number" />
          </Form.Item>

          <Form.Item
            label="First Name"
            name="firstName"
            rules={[{ required: true, message: "First name is required." }]}
          >
            <Input placeholder="First name" />
          </Form.Item>

          <Form.Item
            label="Last Name"
            name="lastName"
            rules={[{ required: true, message: "Last name is required." }]}
          >
            <Input placeholder="Last name" />
          </Form.Item>

          <Form.Item
            label="Company Address"
            name="address"
            rules={[{ required: true, message: "Please enter your address!" }]}
          >
            <Input.TextArea placeholder="Enter your address" rows={3} />
          </Form.Item>

          <Form.Item
            label="Contact Number"
            name="phone"
            rules={[{ required: true, message: "Please enter your contact number!" }]}
          >
            <Input placeholder="Enter your contact number" />
          </Form.Item>

          <Form.Item
            label="Email"
            name="email"
            rules={[
              { required: true, message: "Please enter your email!" },
              { type: "email", message: "Please enter a valid email!" },
            ]}
          >
            <Input type="email" placeholder="Enter your email" />
          </Form.Item>

          <Button type="primary" htmlType="submit" block>
            Done
          </Button>
        </Form>

      
      </Card>
    </div>
  );
};

export default UpdateClient;