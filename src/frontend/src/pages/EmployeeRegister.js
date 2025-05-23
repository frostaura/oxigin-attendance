import React from "react";
import { useNavigate } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";

const { Title } = Typography;

const EmployeeRegister = () => {
  const navigate = useNavigate();

  const handleSubmit = (values) => {
    console.log("Employee Data:", values);
    navigate(-1); // Go back one page
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: 450, textAlign: "center", padding: "20px 24px" }}>
        <Title level={2} style={{ marginBottom: 20 }}>Employee Register</Title>

        <Form layout="vertical" onFinish={handleSubmit}>
          {/* Employee Information */}
          <Form.Item label="Employee Number" name="employeeNumber" rules={[{ required: true, message: "Employee Number is required." }]}>
            <Input placeholder="Employee Number" />
          </Form.Item>

          <Form.Item label="Name" name="name" rules={[{ required: true, message: "Name is required." }]}>
            <Input placeholder="Enter Name" />
          </Form.Item>

          <Form.Item label="Surname" name="surname" rules={[{ required: true, message: "Surname is required." }]}>
            <Input placeholder="Enter Surname" />
          </Form.Item>

          <Form.Item label="ID Number" name="idNumber" rules={[{ required: true, message: "ID Number is required." }]}>
            <Input placeholder="Enter ID Number" />
          </Form.Item>

          <Form.Item label="Address" name="address" rules={[{ required: true, message: "Please enter an Address." }]}>
            <Input.TextArea placeholder="Enter Address" rows={2} />
          </Form.Item>

          <Form.Item label="Contact Number" name="contactNumber" rules={[{ required: true, message: "Please enter a Contact Number." }]}>
            <Input placeholder="Enter Contact Number" />
          </Form.Item>

          {/* Banking Details */}
          <Title level={4} style={{ textAlign: "left", marginTop: 20 }}>Banking Details</Title>

          <Form.Item label="Bank Name" name="bankName" rules={[{ required: true, message: "Bank Name is required." }]}>
            <Input placeholder="Enter Bank Name" />
          </Form.Item>

          <Form.Item label="Account Holder Name" name="accountHolderName" rules={[{ required: true, message: "Account Holder Name is required." }]}>
            <Input placeholder="Enter Account Holder Name" />
          </Form.Item>

          <Form.Item label="Branch Code" name="branchCode" rules={[{ required: true, message: "Branch Code is required." }]}>
            <Input placeholder="Enter Branch Code" />
          </Form.Item>

          <Form.Item label="Account Number" name="accountNumber" rules={[{ required: true, message: "Account Number is required." }]}>
            <Input placeholder="Enter Account Number" />
          </Form.Item>

          {/* Done Button */}
          <Button type="primary" htmlType="submit" block>
            Done
          </Button>
        </Form>
      </Card>
    </div>
  );
};

export default EmployeeRegister;
