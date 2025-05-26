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

public class AdminControllerTests
{
    [Fact]
    public async Task GetEmployees_ReturnsOk()
    {
        var mockAdmin = new Mock<IAdminManager>();
        mockAdmin.Setup(m => m.GetEmployeesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Employee>());
        var logger = Mock.Of<ILogger<AdminController>>();
        var controller = new AdminController(mockAdmin.Object, logger);
        var result = await controller.GetEmployees(CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }
}
