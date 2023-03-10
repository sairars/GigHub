using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Tests.Extensions;
using Moq;
using NUnit.Framework;
using System.Data.Entity;

namespace GigHub.Tests.Persistence.Repositories
{
    [TestFixture]
    public class NotificationRepositoryTests
    {
        private NotificationRepository _repository;
        private Mock<DbSet<UserNotification>> _mockUserNotifications;

        [SetUp]
        public void SetUp()
        {
            _mockUserNotifications = new Mock<DbSet<UserNotification>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext
                .SetupGet(c => c.UserNotifications)
                .Returns(_mockUserNotifications.Object);

            _repository = new NotificationRepository(mockContext.Object);
        }

        [Test]
        public void GetNewNotificationsFor_NotificationHasBeenRead_ShouldNotReturnNotifications()
        {
            var notification = new Notification(new Gig(), It.IsAny<NotificationType>());
            var user = new ApplicationUser { Id = "1" };
            var userNotification = new UserNotification(user, notification);
            userNotification.Read();

            _mockUserNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNewNotificationsFor(user.Id);

            notifications.Should().BeEmpty();
        }

        [Test]
        public void GetNewNotificationsFor_NotificationIsForADifferentUser_ShouldNotReturnNotifications()
        {
            var notification = new Notification(new Gig(), It.IsAny<NotificationType>());
            var user = new ApplicationUser { Id = "1" };
            var userNotification = new UserNotification(user, notification);

            _mockUserNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNewNotificationsFor(user.Id + "-");

            notifications.Should().BeEmpty();
        }

        [Test]
        public void GetNewNotificationsFor_NotificationsAreForTheUserAndAreUnRead_ShouldReturnNotifications()
        {
            var notification = new Notification(new Gig(), It.IsAny<NotificationType>());
            var user = new ApplicationUser { Id = "1" };
            var userNotification = new UserNotification(user, notification);

            _mockUserNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNewNotificationsFor(user.Id);

            notifications.Should().Contain(notification);
        }
    }
}
