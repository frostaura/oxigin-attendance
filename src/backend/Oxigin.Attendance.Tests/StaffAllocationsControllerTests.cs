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

public class StaffAllocationsControllerTests
{
    [Fact]
    public async Task GetAllocationsForEvent_ReturnsOk()
    {
        var mockManager = new Mock<IStaffAllocationManager>();
        mockManager.Setup(m => m.GetAllocationsForEventAsync(It.IsAny<System.Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<StaffAllocation>());
        var logger = Mock.Of<ILogger<StaffAllocationsController>>();
        var controller = new StaffAllocationsController(mockManager.Object, logger);
        var result = await controller.GetAllocationsForEvent(System.Guid.NewGuid(), CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }
}
