using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Staffinity.Personal.Api.Modules.Employees.Controllers;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;

namespace Staffinity.Personal.Test.Modules.Employees.Controller;

public class EmployeesControllerTests
{
    [Fact]
    public async Task Create_ReturnsBadRequest_WhenRequestIsNull()
    {
        var controller = BuildController();

        var result = await controller.Create(null!);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Request body is required.", badRequest.Value);
    }

    [Fact]
    public async Task Create_ReturnsValidationProblem_WhenModelStateIsInvalid()
    {
        var controller = BuildController();
        controller.ModelState.AddModelError("Code", "required");

        var request = EmployeeTestData.CreateEmployeeRequest();

        var result = await controller.Create(request);

        var problem = Assert.IsType<ObjectResult>(result);
        Assert.IsType<ValidationProblemDetails>(problem.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenPasswordIsMissing()
    {
        var controller = BuildController();
        var request = EmployeeTestData.CreateEmployeeRequest(password: string.Empty);

        var result = await controller.Create(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Password is required.", badRequest.Value);
    }

    [Fact]
    public async Task Create_ReturnsServerError_WhenUseCaseFails()
    {
        var createUseCase = new Mock<ICreateEmployeeUseCase>();
        var controller = BuildController(createUseCase: createUseCase);
        var request = EmployeeTestData.CreateEmployeeRequest();
        createUseCase
            .Setup(u => u.CreateAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee?)null);

        var result = await controller.Create(request);

        var serverError = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, serverError.StatusCode);
        Assert.Equal("Employee could not be created.", serverError.Value);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenUseCaseSucceeds()
    {
        var createUseCase = new Mock<ICreateEmployeeUseCase>();
        var controller = BuildController(createUseCase: createUseCase);
        var request = EmployeeTestData.CreateEmployeeRequest();
        createUseCase
            .Setup(u => u.CreateAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee e) => e);

        var result = await controller.Create(request);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<EmployeeResponse>(created.Value);
        Assert.Equal(request.Email, response.Email);
        var expectedHash = HashPassword(request.Password ?? string.Empty);
        createUseCase.Verify(u => u.CreateAsync(It.Is<Employee>(e => e.PasswordHash == expectedHash)), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithEmployees()
    {
        var getAllUseCase = new Mock<IGetAllEmployeesUseCase>();
        var employees = new[]
        {
            EmployeeTestData.CreateEmployee(),
            EmployeeTestData.CreateEmployee()
        };
        getAllUseCase.Setup(u => u.GetAllAsync()).ReturnsAsync(employees);
        var controller = BuildController(getAllUseCase: getAllUseCase);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var values = Assert.IsType<EmployeeResponse[]>(ok.Value);
        Assert.Equal(employees.Length, values.Length);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        var getByIdUseCase = new Mock<IGetEmployeeByIdUseCase>();
        var controller = BuildController(getByIdUseCase: getByIdUseCase);
        var id = Guid.NewGuid();
        getByIdUseCase.Setup(u => u.GetByIdAsync(id)).ReturnsAsync((Employee?)null);

        var result = await controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var getByIdUseCase = new Mock<IGetEmployeeByIdUseCase>();
        var employee = EmployeeTestData.CreateEmployee();
        getByIdUseCase.Setup(u => u.GetByIdAsync(employee.Id)).ReturnsAsync(employee);
        var controller = BuildController(getByIdUseCase: getByIdUseCase);

        var result = await controller.GetById(employee.Id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<EmployeeResponse>(ok.Value);
        Assert.Equal(employee.Id, response.Id);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenRequestIsNull()
    {
        var controller = BuildController();
        var id = Guid.NewGuid();

        var result = await controller.Update(id, null!);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenEmployeeMissing()
    {
        var getByIdUseCase = new Mock<IGetEmployeeByIdUseCase>();
        var controller = BuildController(getByIdUseCase: getByIdUseCase);
        var request = EmployeeTestData.CreateUpdateEmployeeRequest();
        var id = Guid.NewGuid();
        getByIdUseCase.Setup(u => u.GetByIdAsync(id)).ReturnsAsync((Employee?)null);

        var result = await controller.Update(id, request);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenOperationSucceeds()
    {
        var getByIdUseCase = new Mock<IGetEmployeeByIdUseCase>();
        var updateUseCase = new Mock<IUpdateEmployeeUseCase>();
        var existing = EmployeeTestData.CreateEmployee();
        var request = EmployeeTestData.CreateUpdateEmployeeRequest();
        getByIdUseCase.Setup(u => u.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
        updateUseCase
            .Setup(u => u.UpdateAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee e) => e);
        var controller = BuildController(getByIdUseCase: getByIdUseCase, updateUseCase: updateUseCase);

        var result = await controller.Update(existing.Id, request);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<EmployeeResponse>(ok.Value);
        Assert.Equal(request.Email, response.Email);
        updateUseCase.Verify(u => u.UpdateAsync(It.Is<Employee>(e => e.Id == existing.Id)), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var deleteUseCase = new Mock<IDeleteEmployeeUseCase>();
        var id = Guid.NewGuid();
        deleteUseCase.Setup(u => u.DeleteAsync(id)).ReturnsAsync(true);
        var controller = BuildController(deleteUseCase: deleteUseCase);

        var result = await controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenMissing()
    {
        var deleteUseCase = new Mock<IDeleteEmployeeUseCase>();
        var id = Guid.NewGuid();
        deleteUseCase.Setup(u => u.DeleteAsync(id)).ReturnsAsync(false);
        var controller = BuildController(deleteUseCase: deleteUseCase);

        var result = await controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }

    private static EmployeesController BuildController(
        Mock<ICreateEmployeeUseCase>? createUseCase = null,
        Mock<IGetAllEmployeesUseCase>? getAllUseCase = null,
        Mock<IGetEmployeeByIdUseCase>? getByIdUseCase = null,
        Mock<IUpdateEmployeeUseCase>? updateUseCase = null,
        Mock<IDeleteEmployeeUseCase>? deleteUseCase = null)
    {
        var controller = new EmployeesController(
            createUseCase?.Object ?? new Mock<ICreateEmployeeUseCase>().Object,
            getAllUseCase?.Object ?? new Mock<IGetAllEmployeesUseCase>().Object,
            getByIdUseCase?.Object ?? new Mock<IGetEmployeeByIdUseCase>().Object,
            updateUseCase?.Object ?? new Mock<IUpdateEmployeeUseCase>().Object,
            deleteUseCase?.Object ?? new Mock<IDeleteEmployeeUseCase>().Object);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        return controller;
    }

    private static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
