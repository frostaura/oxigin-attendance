using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface ISiteManagerService
    {
        Task<bool> ApproveRequestAsync(int requestId);
        Task<bool> RejectRequestAsync(int requestId);
        Task<IEnumerable<Attendance>> GetAttendanceAsync();
        Task<bool> TrackAttendanceAsync(Attendance attendance);
    }

    public class SiteManagerService : ISiteManagerService
    {
        private readonly List<ClientRequest> _clientRequests = new List<ClientRequest>();
        private readonly List<Attendance> _attendanceRecords = new List<Attendance>();

        public Task<bool> ApproveRequestAsync(int requestId)
        {
            var request = _clientRequests.Find(r => r.Id == requestId);
            if (request != null)
            {
                request.ApprovalStatus = "Approved";
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RejectRequestAsync(int requestId)
        {
            var request = _clientRequests.Find(r => r.Id == requestId);
            if (request != null)
            {
                request.ApprovalStatus = "Rejected";
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Attendance>> GetAttendanceAsync()
        {
            return Task.FromResult<IEnumerable<Attendance>>(_attendanceRecords);
        }

        public Task<bool> TrackAttendanceAsync(Attendance attendance)
        {
            _attendanceRecords.Add(attendance);
            return Task.FromResult(true);
        }
    }
}
