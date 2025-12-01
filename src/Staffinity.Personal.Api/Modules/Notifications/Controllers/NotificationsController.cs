using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Application.Modules.Notifications.Dto;
using Staffinity.Personal.Domain.Modules.Notifications.Exceptions;
using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;

namespace Staffinity.Personal.Api.Modules.Notifications.Controllers
{
    [ApiController]
    [Route("notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IGetAllNotificationsUseCase _getAllNotificationsUseCase;
        private readonly IGetNotificationByIdUseCase _getNotificationByIdUseCase;
        private readonly ICreateNotificationUseCase _createNotificationUseCase;
        private readonly IUpdateNotificationUseCase _updateNotificationUseCase;
        private readonly IDeleteNotificationUseCase _deleteNotificationUseCase;

        public NotificationsController(
            IGetAllNotificationsUseCase getAllNotificationsUseCase,
            IGetNotificationByIdUseCase getNotificationByIdUseCase,
            ICreateNotificationUseCase createNotificationUseCase,
            IUpdateNotificationUseCase updateNotificationUseCase,
            IDeleteNotificationUseCase deleteNotificationUseCase)
        {
            _getAllNotificationsUseCase = getAllNotificationsUseCase;
            _getNotificationByIdUseCase = getNotificationByIdUseCase;
            _createNotificationUseCase = createNotificationUseCase;
            _updateNotificationUseCase = updateNotificationUseCase;
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

        // Create notification
        [HttpPost]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
        {
            try
            {
                var data = await _createNotificationUseCase.CreateAsync(NotificationMapper.CreateRequestToModel(request));
                return Ok(data);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ResourceNotCreatedException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update notification
        [HttpPut]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateNotificationRequest request)
        {
            try
            {
                var data = await _updateNotificationUseCase.EditAsync(NotificationMapper.UpdateRequestToModel(request));
                return Ok(data);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ResourceNotCreatedException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete notification by id
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out var notificationId))
                return BadRequest("The provided id is not a valid Guid");

            try
            {
                var data = await _deleteNotificationUseCase.DeleteAsync(notificationId);

                if (data)
                {
                    return Ok("Notification deleted sucessfully");
                }

                return Conflict("We could not delete that notification");
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
