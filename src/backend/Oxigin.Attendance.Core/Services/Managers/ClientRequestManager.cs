using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

public class ClientRequestManager : IClientRequestManager
{
    public Task<ClientRequest> CreateRequestAsync(ClientRequest request, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new ClientRequest());
    }
    public Task<List<ClientRequest>> GetRequestsForClientAsync(CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new List<ClientRequest>());
    }
    public Task<ClientRequest> ApproveRequestAsync(Guid id, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new ClientRequest());
    }
    public Task<ClientRequest> RejectRequestAsync(Guid id, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new ClientRequest());
    }
    public Task<List<ClientRequest>> GetPendingApprovalsAsync(CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new List<ClientRequest>());
    }
    public Task<ClientRequest> ApproveRequestByManagerAsync(Guid id, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new ClientRequest());
    }
    public Task<ClientRequest> RejectRequestByManagerAsync(Guid id, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new ClientRequest());
    }
}
