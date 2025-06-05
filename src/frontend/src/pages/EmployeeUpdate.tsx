import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Form, Input, Button, Typography, Card, message } from "antd";
import { GetLoggedInUserContextAsync } from "../services/data/backend";
import { getEmployeeByIdAsync, updateEmployeeAsync } from "../services/data/employee";
import type { Employee } from "../models/employeeModels";

const { Title } = Typography;

interface EmployeeUpdateFormValues {
  name: string;
  idNumber: string;
  address: string;
  contactNumber: string;
  bankName: string;
  accountHolderName: string;
  branchCode: string;
  accountNumber: string;
}

const EmployeeUpdate: React.FC = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm<EmployeeUpdateFormValues>();

  // Fetch employee data when component mounts
  useEffect(() => {
    const fetchEmployeeData = async () => {
      try {
        const userContext = await GetLoggedInUserContextAsync();
        if (!userContext?.user.employeeID) {
          message.error("Employee ID not found");
          navigate(-1);
          return;
        }

        const employeeData = await getEmployeeByIdAsync(userContext.user.employeeID);
        if (employeeData) {
          // Set form values - use the user's name from the context
          form.setFieldsValue({
            name: userContext.user.name || "", // Use the user's name from context
            idNumber: employeeData.idNumber || "",
            address: employeeData.address || "",
            contactNumber: employeeData.contactNo || "",
            bankName: employeeData.bankName || "",
            accountHolderName: employeeData.accountHolderName || "",
            branchCode: employeeData.branchCode || "",
            accountNumber: employeeData.accountNumber || "",
          });
        }
      } catch (error) {
        message.error("Failed to fetch employee data");
        console.error("Error fetching employee data:", error);
      }
    };

    fetchEmployeeData();
  }, [form, navigate]);

  const handleSubmit = async (values: EmployeeUpdateFormValues) => {
    try {
      const userContext = await GetLoggedInUserContextAsync();
      if (!userContext?.user.employeeID) {
        message.error("Employee ID not found");
        return;
      }

      // Prepare the employee data for update
      const employeeData: Employee = {
        id: userContext.user.employeeID,
        name: userContext.user.name || "", // Always use the user's name from context
        idNumber: values.idNumber,
        address: values.address,
        contactNo: values.contactNumber,
        bankName: values.bankName,
        accountHolderName: values.accountHolderName,
        branchCode: values.branchCode,
        accountNumber: values.accountNumber,
      };

      await updateEmployeeAsync(employeeData);
      message.success("Employee details updated successfully");
      navigate(-1);
    } catch (error) {
      message.error("Failed to update employee details");
      console.error("Error updating employee:", error);
    }
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh", padding: "20px" }}>
      <Card style={{ width: 450, textAlign: "center", padding: "20px 24px" }}>
        <Title level={4} style={{ marginBottom: 20 }}>Update Details</Title>

        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          {/* Employee Information */}
          <Form.Item 
            label="Name" 
            name="name"
          >
            <Input disabled placeholder="Name" />
          </Form.Item>

          <Form.Item 
            label="ID Number" 
            name="idNumber" 
            rules={[{ required: true, message: "ID Number is required." }]}
          >
            <Input placeholder="Enter ID Number" />
          </Form.Item>

          <Form.Item 
            label="Address" 
            name="address" 
            rules={[{ required: true, message: "Please enter an Address." }]}
          >
            <Input.TextArea placeholder="Enter Address" rows={2} />
          </Form.Item>

          <Form.Item 
            label="Contact Number" 
            name="contactNumber" 
            rules={[{ required: true, message: "Please enter a Contact Number." }]}
          >
            <Input placeholder="Enter Contact Number" />
          </Form.Item>

          {/* Banking Details */}
          <Title level={4} style={{ textAlign: "left", marginTop: 20 }}>Banking Details</Title>

          <Form.Item 
            label="Bank Name" 
            name="bankName"
          >
            <Input placeholder="Enter Bank Name" />
          </Form.Item>

          <Form.Item 
            label="Account Holder Name" 
            name="accountHolderName"
          >
            <Input placeholder="Enter Account Holder Name" />
          </Form.Item>

          <Form.Item 
            label="Branch Code" 
            name="branchCode"
          >
            <Input placeholder="Enter Branch Code" />
          </Form.Item>

          <Form.Item 
            label="Account Number" 
            name="accountNumber"
          >
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

export default EmployeeUpdate; 