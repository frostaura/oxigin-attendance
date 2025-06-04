import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";
import { SignUpAsync } from "../services/data/user";
import type { UserSignUpValues } from "../models/userModels";

const { Title } = Typography;


const Register: React.FC = () => {
  // TODO: We should have only one register and name it something like Register.
  const navigate = useNavigate();
  const [form] = Form.useForm<UserSignUpValues>();
  const [processing, setProcessing] = useState(false);


  const handleSignUp = async (values: UserSignUpValues) => {
    try {
      setProcessing(true); 
      // Call the SignUpAsync function with the form values
      await SignUpAsync(
        values.name,
        values.contactNr,
        values.email,
        values.password
      );
      console.log("Registration successful!");
      navigate("/"); 
    } catch (error) {
      console.error("Registration failed:", error);
      alert(`Registration Failed: ${JSON.stringify(error, null, 2)}`);
      setProcessing(false);
    }
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: 450, textAlign: "center", padding: "20px 24px" }}>
        <Title level={2} style={{ marginBottom: 20 }}>Register</Title>

        <Form form={form} layout="vertical" onFinish={handleSignUp}>
          {/* Employee Information */}

          <Form.Item label="Full Name" name="name" rules={[{ required: true, message: "First name is required." }]}>
            <Input placeholder="Enter Full Name" />
          </Form.Item>

          <Form.Item label="Contact Number" name="contactNr" rules={[{ required: true, message: "ID Number is required." }]}>
            <Input placeholder="Enter Contact Number" />
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
          {/* Done Button */}
          <Button type="primary" htmlType="submit" block>
          {processing ? "Registering you..." : "Done"}
          </Button>
        </Form>
      </Card>
    </div>
  );
};

export default Register; 
