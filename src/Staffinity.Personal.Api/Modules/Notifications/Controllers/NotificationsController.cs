using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Domain.Modules.Notifications.Exceptions;
using Staffinity.Personal.Domain.Modules.Notifications.Model;
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
        [ProducesResponseType(typeof(Notification[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _getAllNotificationsUseCase.GetAllAsync();
                return Ok(data);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
