using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.API.Controllers;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public class EmployeesControllerTests
{
    [Fact]
    public async Task Authenticate_ReturnsOk()
    {
        var mockManager = new Mock<IEmployeeManager>();
        mockManager.Setup(m => m.AuthenticateAsync(It.IsAny<EmployeeLogin>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Employee());
        var logger = Mock.Of<ILogger<EmployeesController>>();
        var controller = new EmployeesController(mockManager.Object, logger);
        var result = await controller.Authenticate(new EmployeeLogin(), CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }
}
