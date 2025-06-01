import { Link, useNavigate } from "react-router-dom";
import { Form, Input, Button, Typography, Card } from "antd";
import { SignInAsync } from "../services/data/user";
import { useEffect, useState } from "react";
import { GetLoggedInUserContext } from "../services/data/backend";
import type { UserSigninResponse } from "../models/userModels";

const { Title, Text } = Typography;

// TODO: Add Oxigin icon to the round picture container.
// TODO: Add a loader for when the signing process is happening.

interface LoginFormValues {
  email: string;
  password: string;
}

const ClientSignIn: React.FC = () => {
  // Attempt to get the signed in user from localsotrage.
  const [userContext, setUserContext] = useState<UserSigninResponse | null>(GetLoggedInUserContext());
  const navigate = useNavigate();
  const [processing, setProcessing] = useState(false);
  
  useEffect(() => {
    if(!userContext) return;

    // If somebody is signed in, redirect to their home page.
    debugger;
    // TODO: Navigate to the correct home page based on the userContext userType.
    setProcessing(true);
    navigate("/clienthome");
  }, [userContext]);

  // Add form instance for reset functionality
  const [form] = Form.useForm();

  const handleLogin = async (values: LoginFormValues): Promise<void> => {
    try {
      setProcessing(true);
      const userContext = await SignInAsync(values.email, values.password);

      setUserContext(userContext);
    } catch (error) {
      const errorContext = JSON.parse((error as any).message || "{}");

      alert(errorContext.message || "An error occurred during sign-in. Please try again.");
      // Reset and focus the password field on login failure
      form.resetFields(["password"]);
      form.getFieldInstance("password")?.focus?.();
    } finally{
      setProcessing(false);
    }
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
      <Card style={{ width: 400, textAlign: "center", padding: 20 }}>
        <Title level={2}>Sign In</Title>

        {/* Logo Placeholder */}
        <div style={{ width: 80, height: 80, borderRadius: "50%", backgroundColor: "#ddd", margin: "10px auto" }}></div>

        <Form<LoginFormValues> form={form} layout="vertical" onFinish={handleLogin}>
          <Form.Item label="Email" name="email" rules={[{ required: true, message: "Please enter your email!" }]}>
            <Input type="email" placeholder="Enter your email" />
          </Form.Item>

          <Form.Item label="Password" name="password" rules={[{ required: true, message: "Please enter your password!" }]}>
            <Input.Password placeholder="Enter your password" />
          </Form.Item>

          <Button type="primary" htmlType="submit" block>
            {processing ? "Signing In..." : "Sign In"}
          </Button>
        </Form>

        <Text style={{ marginTop: 10, display: "block" }}>Don't have an account?</Text>
        <Link to="/clientregister">Register</Link>
      </Card>
    </div>
  );
};

export default ClientSignIn;