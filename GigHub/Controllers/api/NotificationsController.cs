using AutoMapper;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.api
{
    public class NotificationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /api/notifications
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var notifications = _unitOfWork.UserNotifications
                .GetUnReadNotificationsByUser(User.Identity.GetUserId());

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        // POST /api/notifications
        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userNotifications = _unitOfWork.UserNotifications
                .GetUnreadUserNotifications(User.Identity.GetUserId());

            userNotifications.ToList().ForEach(un => un.Read());
            _unitOfWork.Complete();
            return Ok();
        }
    }
}
