using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YamSoft.API.Dtos;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(IDatabaseService databaseService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNotificationsByUserId(int userId)
    {
        try
        {
            var notifications = await databaseService.GetNotificationsByUserIdAsync(userId);
            var notificationDtos = mapper.Map<IEnumerable<NotificationDto>>(notifications);
            return Ok(notificationDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Route("user/{userId}/unread")]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUnreadNotificationsByUserId(int userId)
    {
        try
        {
            var notifications = await databaseService.GetUnreadNotificationsByUserIdAsync(userId);
            var notificationDtos = mapper.Map<IEnumerable<NotificationDto>>(notifications);
            return Ok(notificationDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotification(int id)
    {
        try
        {
            var notification = await databaseService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound(new { error = "Notification not found" });

            var notificationDto = mapper.Map<NotificationDto>(notification);
            return Ok(notificationDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createNotificationDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await databaseService.GetUserByIdAsync(createNotificationDto.UserId);
            if (user == null)
                return BadRequest(new { error = "User not found" });

            var notification = await databaseService.CreateNotificationAsync(
                createNotificationDto.UserId,
                createNotificationDto.Type,
                createNotificationDto.Message);

            var notificationDto = mapper.Map<NotificationDto>(notification);
            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notificationDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNotification(int id, [FromBody] UpdateNotificationDto updateNotificationDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notification = await databaseService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound(new { error = "Notification not found" });

            notification.Status = updateNotificationDto.Status;
            if (updateNotificationDto.Status == Enums.NotificationStatus.Read)
            {
                notification.ProcessedAt = DateTime.UtcNow;
            }

            await databaseService.UpdateNotificationAsync(notification);
            var notificationDto = mapper.Map<NotificationDto>(notification);

            return Ok(notificationDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    [Route("{id}/mark-read")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            var notification = await databaseService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound(new { error = "Notification not found" });

            await databaseService.MarkNotificationAsReadAsync(id);
            var updatedNotification = await databaseService.GetNotificationByIdAsync(id);
            var notificationDto = mapper.Map<NotificationDto>(updatedNotification);

            return Ok(notificationDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        try
        {
            var notification = await databaseService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound(new { error = "Notification not found" });

            await databaseService.DeleteNotificationAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}