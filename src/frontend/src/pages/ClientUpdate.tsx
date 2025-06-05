import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Form, Input, Button, Typography, Card, message } from "antd";
import { GetLoggedInUserContextAsync } from "../services/data/backend";
import { getClientByIdAsync, updateClientAsync } from "../services/data/client";
import type { Client } from "../models/clientModels";

const { Title } = Typography;

interface ClientUpdateFormValues {
  companyName: string;
  registrationNumber: string;
  name: string;
  address: string;
  contactNumber: string;
  email: string;
}

const UpdateClient: React.FC = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm<ClientUpdateFormValues>();

  // Fetch client data when component mounts
  useEffect(() => {
    const fetchClientData = async () => {
      try {
        const userContext = await GetLoggedInUserContextAsync();
        if (!userContext?.user.clientID) {
          message.error("Client ID not found");
          navigate(-1);
          return;
        }

        const clientData = await getClientByIdAsync(userContext.user.clientID);
        if (clientData) {
          // Set form values - use the user's name from context
          form.setFieldsValue({
            companyName: clientData.companyName || "",
            registrationNumber: clientData.regNo || "",
            name: userContext.user.name || "", // Use the user's name from context
            address: clientData.address || "",
            contactNumber: clientData.contactNo || "",
            email: userContext.user.email || "", // Use the user's email from context
          });
        }
      } catch (error) {
        message.error("Failed to fetch client data");
        console.error("Error fetching client data:", error);
      }
    };

    fetchClientData();
  }, [form, navigate]);

  const handleSubmit = async (values: ClientUpdateFormValues) => {
    try {
      const userContext = await GetLoggedInUserContextAsync();
      if (!userContext?.user.clientID) {
        message.error("Client ID not found");
        return;
      }

      // Prepare the client data for update with correct property names and casing
      const clientData: Client = {
        id: userContext.user.clientID,
        companyName: values.companyName,
        regNo: values.registrationNumber,
        address: values.address,
        contactNo: values.contactNumber
      };

      console.log('Sending update request with data:', clientData);
      await updateClientAsync(clientData);
      message.success("Client details updated successfully");
      navigate(-1);
    } catch (error) {
      message.error("Failed to update client details");
      console.error("Error updating client:", error);
    }
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

        <Form form={form} layout="vertical" onFinish={handleSubmit}>
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
            label="Contact Person Name"
            name="name"
          >
            <Input disabled placeholder="Contact person name" />
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
            name="contactNumber"
            rules={[{ required: true, message: "Please enter your contact number!" }]}
          >
            <Input placeholder="Enter your contact number" />
          </Form.Item>

          <Form.Item
            label="Email"
            name="email"
          >
            <Input disabled type="email" placeholder="Email address" />
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