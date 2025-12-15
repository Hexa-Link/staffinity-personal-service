using Moq;
using Staffinity.Personal.Application.Modules.Vacations.UseCases;
using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;
using Xunit;

namespace Staffinity.Personal.Tests.Application
{
    public class RejectVacationUseCaseTests
    {
        [Fact]
        public async Task RejectAsync_ShouldCallUpdate_WhenRequestExists()
        {
            // Arrange: Preparamos el Mock (simulacro del repositorio)
            var mockRepo = new Mock<IVacationRequestRepository>();
            var useCase = new RejectVacationUseCase(mockRepo.Object);

            var id = new VacationRequestId(Guid.NewGuid());
            var request = new VacationRequest(
                id,
                Guid.NewGuid(),
                DateTime.UtcNow.AddDays(5),
                DateTime.UtcNow.AddDays(6),
                "Test"
            );

            // Configuramos el Mock para que devuelva nuestra solicitud cuando la busquen
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(request);

            // Act: Ejecutamos el caso de uso
            await useCase.RejectAsync(id);

            // Assert: Verificamos resultados
            Assert.Equal(VacationStatus.Rejected, request.Status); // El estado debió cambiar
            mockRepo.Verify(r => r.UpdateAsync(request), Times.Once); // Se debió llamar a Update
        }
    }
}
