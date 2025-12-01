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
        private readonly IGetNotificationByIdUseCase _getNotificationByIdUseCase;

        public NotificationsController(IGetAllNotificationsUseCase getAllNotificationsUseCase, IGetNotificationByIdUseCase getNotificationByIdUseCase)
        {
            _getAllNotificationsUseCase = getAllNotificationsUseCase;
            _getNotificationByIdUseCase = getNotificationByIdUseCase;
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            if (!Guid.TryParse(id, out var notificationId))
                return BadRequest("The provided id is not a valid Guid");

            try
            {
                var data = await _getNotificationByIdUseCase.GetByIdAsync(notificationId);
                return Ok(data);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
