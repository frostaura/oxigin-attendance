using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OxiginAttendance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _adminService.GetEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost("employees")]
        public async Task<ActionResult> AddEmployee([FromBody] Employee employee)
        {
            await _adminService.AddEmployeeAsync(employee);
            return Ok();
        }

        [HttpPut("employees/{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            await _adminService.UpdateEmployeeAsync(id, employee);
            return Ok();
        }

        [HttpDelete("employees/{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            await _adminService.DeleteEmployeeAsync(id);
            return Ok();
        }

        [HttpGet("clients")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _adminService.GetClientsAsync();
            return Ok(clients);
        }

        [HttpPost("clients")]
        public async Task<ActionResult> AddClient([FromBody] Client client)
        {
            await _adminService.AddClientAsync(client);
            return Ok();
        }

        [HttpPut("clients/{id}")]
        public async Task<ActionResult> UpdateClient(int id, [FromBody] Client client)
        {
            await _adminService.UpdateClientAsync(id, client);
            return Ok();
        }

        [HttpDelete("clients/{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            await _adminService.DeleteClientAsync(id);
            return Ok();
        }

        [HttpGet("job-requests")]
        public async Task<ActionResult<IEnumerable<JobRequest>>> GetJobRequests()
        {
            var jobRequests = await _adminService.GetJobRequestsAsync();
            return Ok(jobRequests);
        }

        [HttpPost("job-requests")]
        public async Task<ActionResult> AddJobRequest([FromBody] JobRequest jobRequest)
        {
            await _adminService.AddJobRequestAsync(jobRequest);
            return Ok();
        }

        [HttpPut("job-requests/{id}")]
        public async Task<ActionResult> UpdateJobRequest(int id, [FromBody] JobRequest jobRequest)
        {
            await _adminService.UpdateJobRequestAsync(id, jobRequest);
            return Ok();
        }

        [HttpDelete("job-requests/{id}")]
        public async Task<ActionResult> DeleteJobRequest(int id)
        {
            await _adminService.DeleteJobRequestAsync(id);
            return Ok();
        }

        [HttpPost("allocate-staff")]
        public async Task<ActionResult> AllocateStaff([FromBody] StaffAllocation allocation)
        {
            await _adminService.AllocateStaffAsync(allocation);
            return Ok();
        }

        [HttpGet("attendance")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance()
        {
            var attendance = await _adminService.GetAttendanceAsync();
            return Ok(attendance);
        }

        [HttpGet("employee-checkins")]
        public async Task<ActionResult<IEnumerable<CheckInRecord>>> GetEmployeeCheckIns()
        {
            var checkIns = await _adminService.GetEmployeeCheckInsAsync();
            return Ok(checkIns);
        }

        [HttpPost("job-allocations")]
        public async Task<ActionResult> ManageJobAllocations([FromBody] JobAllocation jobAllocation)
        {
            await _adminService.ManageJobAllocationsAsync(jobAllocation);
            return Ok();
        }
    }
}
