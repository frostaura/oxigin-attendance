using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.API.Controllers;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ClientRequestsControllerTests
{
    [Fact]
    public async Task GetRequests_ReturnsOk()
    {
        var mockManager = new Mock<IClientRequestManager>();
        mockManager.Setup(m => m.GetRequestsForClientAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ClientRequest>());
        var logger = Mock.Of<ILogger<ClientRequestsController>>();
        var controller = new ClientRequestsController(mockManager.Object, logger);
        var result = await controller.GetRequests(CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }
}
