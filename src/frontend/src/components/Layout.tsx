import React, { useEffect, useState } from 'react';
import { Button, Layout, Avatar, Menu, Dropdown } from "antd";
import { LeftOutlined, RobotOutlined, LogoutOutlined, EditOutlined } from "@ant-design/icons";
import { useNavigate, useLocation } from "react-router-dom";
import { GetLoggedInUserContextAsync } from '../services/data/backend';

const { Header, Content } = Layout;

interface AppLayoutProps {
  children: React.ReactNode;
}
// TODO: Take out banner from registration page

const AppLayout: React.FC<AppLayoutProps> = ({ children }) => {
  const navigate = useNavigate();
  const location = useLocation();

  const hideBackButtonOn = ["/", "/register"];
  const showBackButton = !hideBackButtonOn.includes(location.pathname) && !location.pathname.includes("home");
  const [userInitials, setUserInitials] = useState<string>("");

  // Infer the card title from the currently-signed-in user, if any.
  useEffect(() => {
    setTimeout(async () => {
      var userContext = await GetLoggedInUserContextAsync();

      if (!!userContext) {
        setUserInitials(userContext.user.name.split(" ").map((n: string) => n[0].toUpperCase()).join(""));
      }
    });
  }, []);

  const menu = (
    <Menu>
      <Menu.Item key="edit" icon={<EditOutlined />} onClick={() => navigate("/clientupdate")}>
        Update Details
      </Menu.Item>
      <Menu.Item key="logout" icon={<LogoutOutlined />} onClick={() => navigate("/")}>
        Log Out
      </Menu.Item>
    </Menu>
  );

  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Header
        style={{
          background: "#fff",
          padding: "10px 20px",
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          height: "60px",
        }}
      >
        {/* Back Button */}
        {showBackButton && (
          <Button
            type="text"
            icon={<LeftOutlined />}
            onClick={() => navigate(-1)}
            style={{ marginRight: "auto" }}
          />
        )}

        {/* Centered Title */}
        <h2 style={{ margin: 0, lineHeight: "1", textAlign: "center", flex: 1 }}>Oxigin Attendance</h2>

        {/* Right Section - Robot Button & Profile Dropdown */}
        <div style={{ display: "flex", alignItems: "center", gap: 10 }}>
          {/* Robot Button */}
          <Button shape="circle" icon={<RobotOutlined />} />

          {/* Profile Dropdown */}
          <Dropdown overlay={menu} trigger={["click"]}>
            <Avatar style={{ backgroundColor: "#1890ff", cursor: "pointer" }}>{userInitials}</Avatar>
          </Dropdown>
        </div>
      </Header>

      <Content style={{ padding: 20 }}>{children}</Content>
    </Layout>
  );
};

export default AppLayout;