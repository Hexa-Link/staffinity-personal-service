using FluentAssertions;
using Staffinity.Personal.Domain.Modules.Vacations.Exceptions;
using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Xunit;

namespace Staffinity.Personal.Tests.Domain
{
    public class VacationRequestTests
    {
        [Fact]
        public void Create_ShouldThrowException_WhenStartDateIsInPast()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddDays(-1);
            var endDate = DateTime.UtcNow.AddDays(5);

            // Act
            Action act = () =>
                new VacationRequest(
                    new VacationRequestId(Guid.NewGuid()),
                    Guid.NewGuid(),
                    pastDate,
                    endDate,
                    "Vacaciones"
                );

            // Assert
            act.Should().Throw<InvalidVacationDateException>();
        }

        [Fact]
        public void Approve_ShouldChangeStatusToApproved()
        {
            // Arrange
            var request = new VacationRequest(
                new VacationRequestId(Guid.NewGuid()),
                Guid.NewGuid(),
                DateTime.UtcNow.AddDays(10),
                DateTime.UtcNow.AddDays(15),
                "Reason"
            );

            // Act
            request.Approve();

            // Assert
            request.Status.Should().Be(VacationStatus.Approved);
        }

        [Fact]
        public void Reject_ShouldChangeStatusToRejected()
        {
            // Arrange
            var request = new VacationRequest(
                new VacationRequestId(Guid.NewGuid()),
                Guid.NewGuid(),
                DateTime.UtcNow.AddDays(10),
                DateTime.UtcNow.AddDays(15),
                "Reason"
            );

            // Act
            request.Reject();

            // Assert
            request.Status.Should().Be(VacationStatus.Rejected);
        }
    }
}
