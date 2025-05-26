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

public class NotificationsControllerTests
{
    [Fact]
    public async Task GetNotificationHistory_ReturnsOk()
    {
        var mockManager = new Mock<INotificationManager>();
        mockManager.Setup(m => m.GetNotificationHistoryAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<NotificationRecord>());
        var logger = Mock.Of<ILogger<NotificationsController>>();
        var controller = new NotificationsController(mockManager.Object, logger);
        var result = await controller.GetNotificationHistory(CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }
}
