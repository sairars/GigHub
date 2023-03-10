using FluentAssertions;
using GigHub.Core.Models;
using NUnit.Framework;
using System.Linq;

namespace GigHub.Tests.Domain.Models
{
    [TestFixture]
    public class ApplicationUserTests
    {
        [Test]
        public void Notify_WhenCalled_AddUserNotification()
        {
            var user = new ApplicationUser();
            var notification = Notification
                .GenerateGigCancelledNotification(new Gig());

            user.Notify(notification);

            user.UserNotifications.Count.Should().Be(1);

            var userNotification = user.UserNotifications.First();
            userNotification.User.Should().Be(user);
            userNotification.Notification.Should().Be(notification);
        }
    }
}
