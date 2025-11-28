using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Domain.Modules.Notifications.Exceptions;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;

namespace Staffinity.Personal.Api.Modules.Notifications.Controllers
{
    [ApiController]
    [Route("notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IGetAllNotificationsUseCase _getAllNotificationsUseCase;

        public NotificationsController(IGetAllNotificationsUseCase getAllNotificationsUseCase)
        {
            _getAllNotificationsUseCase = getAllNotificationsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return StatusCode(200, _getAllNotificationsUseCase.GetAllAsync());
            }
            catch (ResourceNotFoundException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
    }
}
