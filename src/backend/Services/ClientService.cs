using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IClientService
    {
        Task<bool> CreateRequestAsync(ClientRequest request);
        Task<IEnumerable<ClientRequest>> GetRequestsAsync();
        Task<bool> ApproveRequestAsync(int requestId);
        Task<bool> RejectRequestAsync(int requestId);
    }

    public class ClientService : IClientService
    {
        private readonly List<ClientRequest> _requests = new List<ClientRequest>();

        public async Task<bool> CreateRequestAsync(ClientRequest request)
        {
            _requests.Add(request);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<ClientRequest>> GetRequestsAsync()
        {
            return await Task.FromResult(_requests);
        }

        public async Task<bool> ApproveRequestAsync(int requestId)
        {
            var request = _requests.Find(r => r.Id == requestId);
            if (request != null)
            {
                request.ApprovalStatus = "Approved";
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> RejectRequestAsync(int requestId)
        {
            var request = _requests.Find(r => r.Id == requestId);
            if (request != null)
            {
                request.ApprovalStatus = "Rejected";
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
