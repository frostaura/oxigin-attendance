import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import AppLayout from "./components/Layout";
import SignIn from "./pages/SignIn";
import Register from "./pages/Register";
import ClientHome from "./pages/ClientHome";
import ClientUpdate from "./pages/ClientUpdate";
import ClientJobRequest from "./pages/ClientJobRequest"; 
import AdminJobRequest from "./pages/AdminJobRequest"; 
import ClientAdditionalWorkerType from "./pages/ClientAdditionalWorkerType";
import AdminAdditionalWorkerType from "./pages/AdminAdditionalWorkerType";
import AdminHome from "./pages/AdminHome";
import AdminJobsHome from "./pages/AdminJobsHome";
import AdminEmployeesHome from "./pages/AdminEmployeesHome";
import AdminManageEmployees from "./pages/AdminManageEmployees";
import EmployeeRegister from "./pages/Register";
import AdminJobAllocations from "./pages/AdminJobAllocations";
import AdminClientHome from "./pages/AdminClientHome";
import AdminManageClients from "./pages/AdminManageClients";
import AdminUsers from "./pages/AdminUsers";
import EmployeeHome from "./pages/EmployeeHome";
import SiteManagerHome from "./pages/SiteManagerHome";
import SiteManagerCheckIn from "./pages/SiteManagerCheckIn";
import SiteManagerCheckOut from "./pages/SiteManagerCheckOut";
import BaseUserHome from "./pages/BaseUserHome";
import BaseUserTimesheets from "./pages/BaseUserTimesheets";
import EmployeeUpdate from "./pages/EmployeeUpdate";
import UnassignedUser from "./pages/UnassignedUser";
import "./App.css";
import { Routes as AppRoutes } from "./enums/routes";

// TODO: Refactor routes to be enums for reusability instea 
// TODO: Change clientsignin to just signin
// TODO: Sort out registration form to be one and allow settings after

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SignIn />} />
        <Route path= {AppRoutes.Register} element={<Register />} />
        
        <Route
          path="/*"
          element={
            <AppLayout>
              <Routes>
                <Route path={AppRoutes.UnassignedUser} element={<UnassignedUser/>} />
                <Route path={AppRoutes.ClientHome} element={<ClientHome />} />
                <Route path="/adminhome" element={<AdminHome />} />
                <Route path="/clientupdate" element={<ClientUpdate />} />
                <Route path="/adminjobrequest" element={<AdminJobRequest/>}/>
                <Route path="/clientjobrequest" element={<ClientJobRequest/>}/>
                <Route path="/clientadditionalworkertype" element={<ClientAdditionalWorkerType/>}/>
                <Route path="/adminadditionalworkertype" element={<AdminAdditionalWorkerType/>}/>
                <Route path="/adminjobshome" element={<AdminJobsHome/>}/>
                <Route path="/adminemployeeshome" element={<AdminEmployeesHome/>}/>
                <Route path="/adminmanageemployees" element={<AdminManageEmployees/>}/>
                <Route path="/employeeregister" element={<EmployeeRegister/>}/>
                <Route path="/adminjoballocations" element={<AdminJobAllocations/>}/>
                <Route path="/adminclienthome" element={<AdminClientHome/>}/>
                <Route path="/adminmanageclients" element={<AdminManageClients/>}/>
                <Route path="/adminusers" element={<AdminUsers/>}/>
                <Route path="/employeehome" element={<EmployeeHome/>}/>
                <Route path="/sitemanagerhome" element={<SiteManagerHome/>}/>
                <Route path="/sitemanagercheckin" element={<SiteManagerCheckIn/>}/>
                <Route path="/sitemanagercheckout" element={<SiteManagerCheckOut/>}/>
                <Route path="/baseuserhome" element={<BaseUserHome/>}/>
                <Route path="/baseusertimesheets" element={<BaseUserTimesheets/>}/>
                <Route path="/employeeupdate" element={<EmployeeUpdate/>}/>
              </Routes>
            </AppLayout>
          }
        />
      </Routes>
    </Router>
  );
};


export default App; 