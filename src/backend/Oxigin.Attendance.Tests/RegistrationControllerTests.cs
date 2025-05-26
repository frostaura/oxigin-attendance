using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.API.Controllers;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

public class RegistrationControllerTests
{
    [Fact]
    public async Task Register_Employee_ReturnsOk()
    {
        var mockEmp = new Mock<IEmployeeManager>();
        mockEmp.Setup(m => m.RegisterAsync(It.IsAny<EmployeeRegistration>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Employee());
        var mockAdmin = new Mock<IAdminManager>();
        var logger = Mock.Of<ILogger<RegistrationController>>();
        var controller = new RegistrationController(mockEmp.Object, mockAdmin.Object, logger);
        var reg = new UserRegistration { UserType = UserType.Employee, FirstName = "A", LastName = "B", Email = "a@b.com", Password = "pw" };
        var result = await controller.Register(reg, CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task Register_Client_ReturnsOk()
    {
        var mockEmp = new Mock<IEmployeeManager>();
        var mockAdmin = new Mock<IAdminManager>();
        mockAdmin.Setup(m => m.CreateOrUpdateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Client());
        var logger = Mock.Of<ILogger<RegistrationController>>();
        var controller = new RegistrationController(mockEmp.Object, mockAdmin.Object, logger);
        var reg = new UserRegistration { UserType = UserType.Client, FirstName = "C", LastName = "D", Email = "c@d.com", Password = "pw" };
        var result = await controller.Register(reg, CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }
}
