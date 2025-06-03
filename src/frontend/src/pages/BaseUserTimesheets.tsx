import React, { useState, useEffect } from "react";
import { Layout, Card, Select, Table, Button, message } from "antd";
import type { ColumnsType } from "antd/es/table";
import { getJobsAsync } from "../services/data/job";
import { getTimesheetsForJobAsync } from "../services/data/timesheet";
import type { Job } from "../models/jobModels";
import { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';

const { Header, Content } = Layout;
const { Option } = Select;

// Custom type for jsPDF with previousTableEndY
type CustomJsPDF = jsPDF & { previousTableEndY?: number };

// Add autoTable to jsPDF prototype
(jsPDF as any).API.autoTable = autoTable;

interface JobOnDate {
  key: string;
  jobId: string;
  purchaseOrderNo: string;
  jobName: string;
  companyName: string;
}

interface TimesheetEntry {
  key: string;
  employeeId: string;
  employeeName: string;
  timeIn: string;
  timeOut: string;
  hoursWorked: string;
}

const BaseUserTimesheets: React.FC = () => {
  const [selectedJob, setSelectedJob] = useState<string | null>(null);
  const [jobs, setJobs] = useState<Job[]>([]);
  const [loading, setLoading] = useState(false);
  const [jobsOnDate, setJobsOnDate] = useState<JobOnDate[]>([]);
  const [timesheetData, setTimesheetData] = useState<TimesheetEntry[]>([]);
  const [selectedJobDate, setSelectedJobDate] = useState<string>("");

  // Fetch all jobs when component mounts
  useEffect(() => {
    const fetchJobs = async () => {
      try {
        setLoading(true);
        const jobsData = await getJobsAsync();
        setJobs(jobsData);
      } catch (error) {
        console.error("Failed to fetch jobs:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchJobs();
  }, []);

  const handleJobChange = async (value: string | null) => {
    setSelectedJob(value);
    if (value) {
      const selectedJobData = jobs.find(job => job.id === value);
      if (selectedJobData) {
        // Set job details
        setJobsOnDate([{
          key: selectedJobData.id || '1',
          jobId: selectedJobData.id || '',
          purchaseOrderNo: selectedJobData.purchaseOrderNumber || '',
          jobName: selectedJobData.jobName || '',
          companyName: selectedJobData.clientID || '',
        }]);

        // Set job date
        const jobDate = new Date(selectedJobData.time).toLocaleDateString();
        setSelectedJobDate(jobDate);

        // Fetch timesheets for the selected job
        try {
          const timesheets = await getTimesheetsForJobAsync(selectedJobData.id || '');
          const formattedTimesheets = timesheets.map(timesheet => ({
            key: timesheet.id || '',
            employeeId: timesheet.employeeID,
            employeeName: timesheet.employee?.name || 'Unknown',
            timeIn: new Date(timesheet.timeIn).toLocaleTimeString(),
            timeOut: timesheet.timeOut && timesheet.timeOut !== "0001-01-01T00:00:00" 
              ? new Date(timesheet.timeOut).toLocaleTimeString() 
              : '',
            hoursWorked: calculateHoursWorked(timesheet.timeIn, timesheet.timeOut)
          }));
          setTimesheetData(formattedTimesheets);
        } catch (error) {
          console.error("Failed to fetch timesheets:", error);
        }
      }
    } else {
      setJobsOnDate([]);
      setTimesheetData([]);
      setSelectedJobDate("");
    }
  };

  const calculateHoursWorked = (timeIn: string, timeOut?: string): string => {
    if (!timeOut || timeOut === "0001-01-01T00:00:00") return "0";
    const start = new Date(timeIn);
    const end = new Date(timeOut);
    const diff = end.getTime() - start.getTime();
    const hours = Math.round((diff / (1000 * 60 * 60)) * 10) / 10;
    return hours.toString();
  };

  const columnsJobsOnDate: ColumnsType<JobOnDate> = [
    { title: "Job ID", dataIndex: "jobId", key: "jobId" },
    { title: "Purchase Order Number", dataIndex: "purchaseOrderNo", key: "purchaseOrderNo" },
    { title: "Job Name", dataIndex: "jobName", key: "jobName" },
    { title: "Company Name", dataIndex: "companyName", key: "companyName" }
  ];

  const columnsTimesheet: ColumnsType<TimesheetEntry> = [
    { title: "Employee Name", dataIndex: "employeeName", key: "employeeName" },
    { title: "Employee ID", dataIndex: "employeeId", key: "employeeId" },
    { title: "Time In", dataIndex: "timeIn", key: "timeIn" },
    { title: "Time Out", dataIndex: "timeOut", key: "timeOut" },
    { title: "Hours Worked", dataIndex: "hoursWorked", key: "hoursWorked" }
  ];

  const handlePrint = () => {
    if (!selectedJob || jobsOnDate.length === 0) {
      message.error("Please select a job first");
      return;
    }

    try {
      // Create new PDF document
      const doc = new jsPDF() as CustomJsPDF;
      const pageWidth = doc.internal.pageSize.getWidth();
      const pageHeight = doc.internal.pageSize.getHeight();

      // Add title
      doc.setFontSize(18);
      doc.text("Oxigin Timesheet", pageWidth / 2, 15, { align: "center" });

      // Add job details
      doc.setFontSize(12);
      const jobDetails = jobsOnDate[0];
      const selectedJobData = jobs.find(job => job.id === selectedJob);
      
      // Job Details Section
      doc.setFontSize(14);
      doc.text("Job Details", 14, 30);
      doc.setFontSize(12);
      
      const jobDetailsData = [
        ["Purchase Order Number:", jobDetails.purchaseOrderNo],
        ["Job Name:", jobDetails.jobName],
        ["Client Name:", selectedJobData?.client?.companyName || "N/A"],
        ["Date:", selectedJobDate]
      ];

      // Add job details table
      autoTable(doc, {
        startY: 35,
        head: [],
        body: jobDetailsData,
        theme: 'plain',
        styles: { fontSize: 10 },
        columnStyles: {
          0: { fontStyle: 'bold', cellWidth: 50 },
          1: { cellWidth: 100 }
        },
        didDrawPage: function(data: any) {
          // Save the final Y position after the table is drawn
          doc.previousTableEndY = data.cursor.y;
        }
      });

      // Add timesheet table title
      doc.setFontSize(14);
      const timesheetTitleY = doc.previousTableEndY ? doc.previousTableEndY + 15 : 50;
      doc.text("Timesheet Details", 14, timesheetTitleY);

      // Convert timesheet data for the table
      const timesheetTableData = timesheetData.map(entry => [
        entry.employeeName,
        entry.timeIn,
        entry.timeOut || 'Not checked out',
        entry.hoursWorked
      ]);

      // Add timesheet table
      autoTable(doc, {
        startY: timesheetTitleY + 5,
        head: [["Employee Name", "Time In", "Time Out", "Hours Worked"]],
        body: timesheetTableData,
        theme: 'striped',
        styles: { fontSize: 10 },
        headStyles: { fillColor: [66, 66, 66] }
      });

      // Add footer with date and page number
      const pageCount = (doc as any).internal.getNumberOfPages();
      for (let i = 1; i <= pageCount; i++) {
        doc.setPage(i);
        doc.setFontSize(10);
        doc.text(
          `Generated on: ${new Date().toLocaleDateString()} ${new Date().toLocaleTimeString()}`,
          14,
          pageHeight - 10
        );
        doc.text(
          `Page ${i} of ${pageCount}`,
          pageWidth - 14,
          pageHeight - 10,
          { align: "right" }
        );
      }

      // Save the PDF
      doc.save(`Oxigin Timesheet - ${jobDetails.jobName}.pdf`);
      message.success("PDF generated successfully");
    } catch (error) {
      console.error("Error generating PDF:", error);
      message.error("Failed to generate PDF");
    }
  };

  return (
    <Layout style={{ minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", backgroundColor: "#f0f2f5" }}>
      <Card style={{ width: "80%", padding: 20 }}>
        {/* Page Header */}
        <Header style={{ display: "flex", justifyContent: "center", alignItems: "center", background: "none", padding: "0 20px" }}>
          <h2 style={{ margin: 0, textAlign: "center" }}>Base User Timesheets</h2>
        </Header>

        {/* Main Content */}
        <Content style={{ flex: 1, padding: 20 }}>
          <div style={{ marginBottom: 20 }}>
            <label style={{ display: "block", fontWeight: "bold", marginBottom: 5, fontSize: "16px" }}>
              Select Job:
            </label>
            <Select
              placeholder="Select a job"
              value={selectedJob}
              onChange={handleJobChange}
              style={{ width: "100%", padding: 8 }}
              loading={loading}
              allowClear
            >
              {jobs.map((job) => (
                <Option key={job.id} value={job.id}>
                  {job.jobName}
                </Option>
              ))}
            </Select>
          </div>

          {/* Tables Container */}
          <Card style={{ display: "flex", justifyContent: "space-between", gap: "20px", marginTop: 20 }}>
            {/* Left Side Table - Jobs on Date */}
            <div style={{ flex: 1 }}>
              <Card title="Selected Job Details">
                <Table
                  columns={columnsJobsOnDate}
                  dataSource={jobsOnDate}
                  pagination={false}
                  rowKey="key"
                  scroll={{ y: 300 }}
                />
              </Card>
            </div>

            {/* Right Side Table - Timesheet */}
            <div style={{ flex: 1 }}>
              <Card 
                title={
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <span>Timesheet</span>
                    {selectedJobDate && <span>Date: {selectedJobDate}</span>}
                  </div>
                }
              >
                <Table
                  columns={columnsTimesheet}
                  dataSource={timesheetData}
                  pagination={false}
                  rowKey="key"
                  scroll={{ y: 300 }}
                />
              </Card>
            </div>
          </Card>

          {/* Print Button */}
          <div style={{ display: "flex", justifyContent: "flex-end", marginTop: 20 }}>
            <Button 
              type="primary" 
              onClick={handlePrint}
              disabled={!selectedJob || jobsOnDate.length === 0}
            >
              Generate PDF
            </Button>
          </div>
        </Content>
      </Card>
    </Layout>
  );
};

export default BaseUserTimesheets; 