import { Link, useNavigate } from "react-router-dom";
import { Form, Input, Button, Typography, Card, message } from "antd";
import { SignInAsync } from "../services/data/user";
import { useEffect, useState } from "react";
import type { UserSigninResponse } from "../models/userModels";
import { UserType } from "../enums/userTypes";
import { Routes as AppRoutes } from "../enums/routes";
import { GetLoggedInUserContextAsync } from "../services/data/backend";

const { Title, Text } = Typography;

interface LoginFormValues {
  email: string;
  password: string;
}

const SignIn: React.FC = () => {
  // TODO: We should only have one sign in so renaming this to SignIn would be better.
  // Attempt to get the signed in user from localsotrage.
  const [userContext, setUserContext] = useState<UserSigninResponse | null>();
  const navigate = useNavigate();
  const [processing, setProcessing] = useState(false);

  useEffect(() => {
    setTimeout(async () => {
      setUserContext(await GetLoggedInUserContextAsync());
    });
  }, []);

  useEffect(() => {
    if(!userContext) return;

    setProcessing(true);

    switch(userContext.user.userType){
      case UserType.Admin:
        navigate(AppRoutes.AdminHome);
        break;
      case UserType.Employee:
        navigate(AppRoutes.EmployeeHome);
        break;
      case UserType.Client:
        navigate(AppRoutes.ClientHome);
        break;
      case UserType.SiteManager:
        navigate(AppRoutes.SiteManagerHome);
        break;
      case UserType.BaseUser:
        navigate(AppRoutes.BaseUserTimesheets);
        break;
      case UserType.Unassigned:
      default:
        navigate(AppRoutes.UnassignedUser);
        break;
    }

    setProcessing(false);
  }, [userContext]);

  // Add form instance for reset functionality
  const [form] = Form.useForm();

  const onFinish = async (values: any) => {
    try {
      await SignInAsync(values.email, values.password);
      // Get the user context after successful sign in
      const context = await GetLoggedInUserContextAsync();
      setUserContext(context);
    } catch (error) {
      let errorMessage = 'Failed to sign in';
      if (error instanceof Error) {
        try {
          const errorData = JSON.parse(error.message);
          errorMessage = errorData.message || errorMessage;
        } catch {
          errorMessage = error.message;
        }
      }
      message.error(errorMessage);
    }
  };

  return (
    <div className="form-container">
      <Card className="form-card">
        <Title level={3} className="text-center" style={{ marginBottom: 16 }}>Sign In</Title>

        <div className="img-fluid" style={{ 
          width: '80px', 
          height: '80px', 
          borderRadius: '50%', 
          backgroundImage: 'url(icon.JPG)', 
          margin: '8px auto', 
          backgroundRepeat: 'no-repeat', 
          backgroundPosition: 'center', 
          backgroundSize: 'contain' 
        }}></div>

        <Form<LoginFormValues> 
          form={form} 
          layout="vertical" 
          onFinish={onFinish}
          className="w-full"
        >
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

        <div className="flex flex-col items-center mt-4">
          <Text>Don't have an account?</Text>
          <Link to={AppRoutes.Register}>Register</Link>
        </div>
      </Card>
    </div>
  );
};

export default SignIn;