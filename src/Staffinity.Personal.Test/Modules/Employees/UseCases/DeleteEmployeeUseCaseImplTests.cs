using System;
using Moq;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Test.Modules.Employees.UseCases;

public class DeleteEmployeeUseCaseImplTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly DeleteEmployeeUseCaseImpl _sut;

    public DeleteEmployeeUseCaseImplTests()
    {
        _sut = new DeleteEmployeeUseCaseImpl(_employeeRepository.Object);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsRepositoryResult_WhenIdIsValid()
    {
        var employeeId = Guid.NewGuid();
        _employeeRepository
            .Setup(r => r.DeleteAsync(employeeId))
            .ReturnsAsync(true);

        var result = await _sut.DeleteAsync(employeeId);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsArgumentNullException_WhenIdIsEmpty()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.DeleteAsync(Guid.Empty));
    }
}
