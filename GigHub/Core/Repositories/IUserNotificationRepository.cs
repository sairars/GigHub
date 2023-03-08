using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IUserNotificationRepository
    {
        IEnumerable<Notification> GetUnReadNotificationsByUser(string userId);
        IEnumerable<UserNotification> GetUnreadUserNotifications(string userId);
    }
}