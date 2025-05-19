using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(int id, Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<IEnumerable<Client>> GetClientsAsync();
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(int id, Client client);
        Task DeleteClientAsync(int id);
        Task<IEnumerable<JobRequest>> GetJobRequestsAsync();
        Task AddJobRequestAsync(JobRequest jobRequest);
        Task UpdateJobRequestAsync(int id, JobRequest jobRequest);
        Task DeleteJobRequestAsync(int id);
        Task AllocateStaffAsync(StaffAllocation allocation);
        Task<IEnumerable<Attendance>> GetAttendanceAsync();
    }

    public class AdminService : IAdminService
    {
        private readonly List<Employee> _employees = new List<Employee>();
        private readonly List<Client> _clients = new List<Client>();
        private readonly List<JobRequest> _jobRequests = new List<JobRequest>();
        private readonly List<StaffAllocation> _staffAllocations = new List<StaffAllocation>();
        private readonly List<Attendance> _attendanceRecords = new List<Attendance>();

        public Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return Task.FromResult<IEnumerable<Employee>>(_employees);
        }

        public Task AddEmployeeAsync(Employee employee)
        {
            _employees.Add(employee);
            return Task.CompletedTask;
        }

        public Task UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = _employees.Find(e => e.Id == id);
            if (existingEmployee != null)
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Email = employee.Email;
                existingEmployee.Password = employee.Password;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.Role = employee.Role;
                existingEmployee.DateOfBirth = employee.DateOfBirth;
                existingEmployee.Address = employee.Address;
                existingEmployee.NotificationPreference = employee.NotificationPreference;
                existingEmployee.CheckInTime = employee.CheckInTime;
                existingEmployee.Latitude = employee.Latitude;
                existingEmployee.Longitude = employee.Longitude;
            }
            return Task.CompletedTask;
        }

        public Task DeleteEmployeeAsync(int id)
        {
            var employee = _employees.Find(e => e.Id == id);
            if (employee != null)
            {
                _employees.Remove(employee);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Client>> GetClientsAsync()
        {
            return Task.FromResult<IEnumerable<Client>>(_clients);
        }

        public Task AddClientAsync(Client client)
        {
            _clients.Add(client);
            return Task.CompletedTask;
        }

        public Task UpdateClientAsync(int id, Client client)
        {
            var existingClient = _clients.Find(c => c.Id == id);
            if (existingClient != null)
            {
                existingClient.CompanyName = client.CompanyName;
                existingClient.RegistrationNumber = client.RegistrationNumber;
                existingClient.FullName = client.FullName;
                existingClient.Address = client.Address;
                existingClient.ContactNumber = client.ContactNumber;
                existingClient.Email = client.Email;
                existingClient.Password = client.Password;
            }
            return Task.CompletedTask;
        }

        public Task DeleteClientAsync(int id)
        {
            var client = _clients.Find(c => c.Id == id);
            if (client != null)
            {
                _clients.Remove(client);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<JobRequest>> GetJobRequestsAsync()
        {
            return Task.FromResult<IEnumerable<JobRequest>>(_jobRequests);
        }

        public Task AddJobRequestAsync(JobRequest jobRequest)
        {
            _jobRequests.Add(jobRequest);
            return Task.CompletedTask;
        }

        public Task UpdateJobRequestAsync(int id, JobRequest jobRequest)
        {
            var existingJobRequest = _jobRequests.Find(j => j.Id == id);
            if (existingJobRequest != null)
            {
                existingJobRequest.EventDate = jobRequest.EventDate;
                existingJobRequest.EventTime = jobRequest.EventTime;
                existingJobRequest.NumberOfStaff = jobRequest.NumberOfStaff;
                existingJobRequest.Role = jobRequest.Role;
                existingJobRequest.Location = jobRequest.Location;
                existingJobRequest.ApprovalStatus = jobRequest.ApprovalStatus;
                existingJobRequest.ClientInformation = jobRequest.ClientInformation;
            }
            return Task.CompletedTask;
        }

        public Task DeleteJobRequestAsync(int id)
        {
            var jobRequest = _jobRequests.Find(j => j.Id == id);
            if (jobRequest != null)
            {
                _jobRequests.Remove(jobRequest);
            }
            return Task.CompletedTask;
        }

        public Task AllocateStaffAsync(StaffAllocation allocation)
        {
            _staffAllocations.Add(allocation);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Attendance>> GetAttendanceAsync()
        {
            return Task.FromResult<IEnumerable<Attendance>>(_attendanceRecords);
        }
    }
}
