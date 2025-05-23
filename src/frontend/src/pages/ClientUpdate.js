import React from "react";
import { Link } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";

const { Title, Text } = Typography;

const UpdateClient = () => {
  const handleRegister = (values) => {
    console.log("Updated Data:", values);
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
      <Card style={{ width: 400, textAlign: "center", padding: 20 }}>
        <Title level={2}>Update Details</Title>

        <Form layout="vertical" onFinish={handleRegister}>

           <Form.Item label="Company Name" name="companyname" rules={[{required: true, message: "Company name is required."}]} > 
            <Input placeholder="Company name"/>
           </Form.Item>

           <Form.Item label="Registration Number" name="registrationnumber" rules={[{required: true, message: "Registration number is required."}]} > 
            <Input placeholder="Company registration number"/>
           </Form.Item>

           <Form.Item label="Full Name" name="fullname" rules={[{required: true, message: "Full name is required."}]} > 
            <Input placeholder="Full name"/>
           </Form.Item>

           <Form.Item label="Address" name="address" rules={[{ required: true, message: "Please enter your address!" }]}>
            <Input.TextArea placeholder="Enter your address" rows={3} />
           </Form.Item>

          <Form.Item label="Contact Number" name="contactnumber" rules={[{ required: true, message: "Please enter your contact number!" }]}>
            <Input placeholder="Enter your contact number" />
          </Form.Item>

          <Form.Item label="Email" name="email" rules={[{ required: true, message: "Please enter your email!" }]}>
            <Input type="email" placeholder="Enter your email" />
          </Form.Item>

          <Form.Item label="Password" name="password" rules={[{ required: true, message: "Please enter your password!" }]}>
            <Input.Password placeholder="Enter your password" />
          </Form.Item>

          <Form.Item label="Confirm Password" name="confirm" dependencies={['password']} 
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
            Done
          </Button>
        </Form>

        <Text style={{ marginTop: 10, display: "block" }}>Already have an account?</Text>
        <Link to="/">Sign In</Link>
      </Card>
    </div>
  );
};

export default UpdateClient;
