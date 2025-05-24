import React, { useState } from "react";
import { Layout, Card, Input, Button, Table } from "antd";
import { useNavigate } from "react-router-dom";

const { Header, Content } = Layout;

const SiteManagerCheckOut = () => {
  const navigate = useNavigate();
  const [jobId, setJobId] = useState("");
  const [employeeId, setEmployeeId] = useState("");
  const [checkOutHistory, setCheckOutHistory] = useState([
    { key: "1", jobId: "J001", employeeId: "E001", employeeName: "John Doe", timeOut: "2025-03-20 08:00" },
    { key: "2", jobId: "J002", employeeId: "E002", employeeName: "Jane Smith", timeOut: "2025-03-20 08:30" },
  ]);

  const handleCheckOut = () => {
    const newCheckOut = {
      key: `${checkOutHistory.length + 1}`,
      jobId: jobId,
      employeeId: employeeId,
      employeeName: "John Doe", // This can be dynamic if needed
      timeOut: new Date().toLocaleString(),
    };
    setCheckOutHistory([...checkOutHistory, newCheckOut]);
    setJobId("");
    setEmployeeId("");
  };

  const columns = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Employee ID", dataIndex: "employeeId", key: "employeeId" },
    { title: "Employee Name", dataIndex: "employeeName", key: "employeeName" },
    { title: "Time Out", dataIndex: "timeOut", key: "timeOut" },
  ];

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        {/* Page Header */}
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Check Out Page</h2>
        </Header>

        {/* Main Content */}
        <Content style={{ flex: 1, padding: 20 }}>
          {/* Job ID and Employee ID inputs with Check In button */}
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
            <div style={{ display: "flex", gap: 10, flex: 1 }}>
              <div style={{ flex: 1, minWidth: "70px" }}>
                <label>Job ID</label>
                <Input value={jobId} onChange={(e) => setJobId(e.target.value)} />
              </div>
              <div style={{ flex: 1, minWidth: "70px" }}>
                <label>Employee ID</label>
                <Input value={employeeId} onChange={(e) => setEmployeeId(e.target.value)} />
              </div>
            </div>
            <Button type="primary" onClick={handleCheckOut} style={{ marginLeft: "20px" }}>Check Out</Button>
          </div>

          {/* Check-in History Table */}
          <Card title="Check-out History">
            <Table columns={columns} dataSource={checkOutHistory} pagination={false} />
          </Card>
        </Content>
      </Card>
    </Layout>
  );
};

export default SiteManagerCheckOut;
