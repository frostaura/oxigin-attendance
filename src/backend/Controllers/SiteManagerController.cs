using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OxiginAttendance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteManagerController : ControllerBase
    {
        private readonly ISiteManagerService _siteManagerService;

        public SiteManagerController(ISiteManagerService siteManagerService)
        {
            _siteManagerService = siteManagerService;
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveRequest([FromBody] int requestId)
        {
            var result = await _siteManagerService.ApproveRequestAsync(requestId);
            if (result)
            {
                return Ok(new { message = "Request approved successfully" });
            }
            return BadRequest(new { message = "Failed to approve request" });
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectRequest([FromBody] int requestId)
        {
            var result = await _siteManagerService.RejectRequestAsync(requestId);
            if (result)
            {
                return Ok(new { message = "Request rejected successfully" });
            }
            return BadRequest(new { message = "Failed to reject request" });
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendance()
        {
            var attendance = await _siteManagerService.GetAttendanceAsync();
            return Ok(attendance);
        }

        [HttpPost("track-attendance")]
        public async Task<IActionResult> TrackAttendance([FromBody] Attendance attendance)
        {
            var result = await _siteManagerService.TrackAttendanceAsync(attendance);
            if (result)
            {
                return Ok(new { message = "Attendance tracked successfully" });
            }
            return BadRequest(new { message = "Failed to track attendance" });
        }
    }
}
