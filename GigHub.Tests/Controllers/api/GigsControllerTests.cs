using FluentAssertions;
using GigHub.Controllers.api;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using Moq;
using NUnit.Framework;
using System.Web.Http.Results;
using GigHub.Tests.Extensions;

namespace GigHub.Tests.Controllers.api
{
    [TestFixture]
    public class GigsControllerTests
    {
        private GigsController _controller;
        private Mock<IGigRepository> _mockRepository;
        private readonly string _userId = "1";
        private readonly string _userName = "user1@gighub.com";

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IGigRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Gigs).Returns(_mockRepository.Object);

            _controller = new GigsController(mockUnitOfWork.Object);
            _controller.MockCurrentUser(_userId, _userName);
        }

        [Test]
        public void Cancel_NoGigWithGivenIdExists_ReturnNotFound()
        {
            var result = _controller.Cancel(1);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void Cancel_GigIsCancelled_ReturnNotFound()
        {
            var gig = new Gig();
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _controller.Cancel(1);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void Cancel_UserTryingToCancelAnotherUsersGig_ReturnUnAuthorized()
        {
            var gig = new Gig { ArtistId = _userId + "-" };

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _controller.Cancel(1);
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public void Cancel_ValidRequest_ReturnOk()
        {
            var gig = new Gig { ArtistId = _userId };

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}
