import React from "react";
import { useNavigate } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";
import type { EmployeeRegistrationFormValues } from "../types";

const { Title } = Typography;

interface ExtendedEmployeeRegistrationFormValues extends EmployeeRegistrationFormValues {
  surname: string;
  idNumber: string;
  bankName: string;
  accountHolderName: string;
  branchCode: string;
  accountNumber: string;
}

const EmployeeRegister: React.FC = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm<ExtendedEmployeeRegistrationFormValues>();

  const handleSubmit = (values: ExtendedEmployeeRegistrationFormValues) => {
    console.log("Employee Data:", values);
    navigate(-1); // Go back one page
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: 450, textAlign: "center", padding: "20px 24px" }}>
        <Title level={2} style={{ marginBottom: 20 }}>Employee Register</Title>

        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          {/* Employee Information */}
          <Form.Item label="Employee ID" name="employeeId" rules={[{ required: true, message: "Employee ID is required." }]}>
            <Input placeholder="Employee ID" />
          </Form.Item>

          <Form.Item label="First Name" name="firstName" rules={[{ required: true, message: "First name is required." }]}>
            <Input placeholder="Enter First Name" />
          </Form.Item>

          <Form.Item label="Last Name" name="lastName" rules={[{ required: true, message: "Last name is required." }]}>
            <Input placeholder="Enter Last Name" />
          </Form.Item>

          <Form.Item label="ID Number" name="idNumber" rules={[{ required: true, message: "ID Number is required." }]}>
            <Input placeholder="Enter ID Number" />
          </Form.Item>

          <Form.Item label="Address" name="address" rules={[{ required: true, message: "Please enter an Address." }]}>
            <Input.TextArea placeholder="Enter Address" rows={2} />
          </Form.Item>

          <Form.Item label="Phone" name="phone" rules={[{ required: true, message: "Please enter a Contact Number." }]}>
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