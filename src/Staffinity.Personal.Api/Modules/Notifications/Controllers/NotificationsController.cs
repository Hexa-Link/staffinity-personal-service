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
        private readonly IDeleteNotificationUseCase _deleteNotificationUseCase;

        public NotificationsController(
            IGetAllNotificationsUseCase getAllNotificationsUseCase,
            IGetNotificationByIdUseCase getNotificationByIdUseCase,
            IDeleteNotificationUseCase deleteNotificationUseCase)
        {
            _getAllNotificationsUseCase = getAllNotificationsUseCase;
            _getNotificationByIdUseCase = getNotificationByIdUseCase;
            _deleteNotificationUseCase = deleteNotificationUseCase;
        }

        //Get all notifications
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

        // Get notification by id
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

        // Delete notification by id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out var notificationId))
                return BadRequest("The provided id is not a valid Guid");

            try
            {
                var data = await _deleteNotificationUseCase.DeleteAsync(notificationId);
                return Ok(data);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
