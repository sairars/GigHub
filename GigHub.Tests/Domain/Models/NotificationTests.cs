using FluentAssertions;
using GigHub.Core.Models;
using NUnit.Framework;

namespace GigHub.Tests.Domain.Models
{
    [TestFixture]
    public class NotificationTests
    {
        [Test]
        public void GenerateGigCancelledNotification_WhenCalled_ReturnNotificationForGigAndOfTypeCancelled()
        {
            var gig = new Gig();
            var notification = Notification.GenerateGigCancelledNotification(gig);

            notification.Should().BeOfType<Notification>();
            notification.Gig.Should().Be(gig);
            notification.NotificationType.Should().Be(NotificationType.Cancelled);
        }
    }
}
