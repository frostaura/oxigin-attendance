using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> CreateRequest([FromBody] ClientRequest request)
        {
            var result = await _clientService.CreateRequestAsync(request);
            if (result)
            {
                return Ok(new { message = "Request created successfully" });
            }
            return BadRequest(new { message = "Failed to create request" });
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _clientService.GetRequestsAsync();
            return Ok(requests);
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveRequest([FromBody] int requestId)
        {
            var result = await _clientService.ApproveRequestAsync(requestId);
            if (result)
            {
                return Ok(new { message = "Request approved successfully" });
            }
            return BadRequest(new { message = "Failed to approve request" });
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectRequest([FromBody] int requestId)
        {
            var result = await _clientService.RejectRequestAsync(requestId);
            if (result)
            {
                return Ok(new { message = "Request rejected successfully" });
            }
            return BadRequest(new { message = "Failed to reject request" });
        }

        [HttpPost("log-job-request")]
        public async Task<IActionResult> LogJobRequest([FromBody] JobRequest jobRequest)
        {
            var result = await _clientService.LogJobRequestAsync(jobRequest);
            if (result)
            {
                return Ok(new { message = "Job request logged successfully" });
            }
            return BadRequest(new { message = "Failed to log job request" });
        }

        [HttpPost("collaborative-approval")]
        public async Task<IActionResult> CollaborativeApproval([FromBody] CollaborativeApprovalDto approvalDto)
        {
            var result = await _clientService.CollaborativeApprovalAsync(approvalDto);
            if (result)
            {
                return Ok(new { message = "Collaborative approval/rejection successful" });
            }
            return BadRequest(new { message = "Failed to perform collaborative approval/rejection" });
        }
    }
}
