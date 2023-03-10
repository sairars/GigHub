using FluentAssertions;
using GigHub.Controllers.api;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Moq;
using NUnit.Framework;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.api
{
    [TestFixture]
    public class AttendancesControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private AttendancesController _controller;
        private Mock<IAttendanceRepository> _mockRepository;
        private readonly int _gigId = 1;
        private readonly string _userId = "1";
        private readonly string _userName = "user1@gighub.com";

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IAttendanceRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork
                .SetupGet(u => u.Attendances)
                .Returns(_mockRepository.Object);
            _controller = new AttendancesController(_mockUnitOfWork.Object);
            _controller.MockCurrentUser(_userId, _userName);
        }

        [Test]
        public void Attend_AttendanceAlreadyExists_ReturnBadRequest()
        {
            var attendance = new Attendance
            {
                Gig = new Gig { Id = _gigId },
                Attendee = new ApplicationUser
                {
                    Id = _userId
                }
            };

            _mockRepository.Setup(r => r.GetAttendance(1, "1")).Returns(attendance);

            var result = _controller.Attend(new AttendanceDto { GigId = 1 });

            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [Test]
        public void Attend_NewAttendance_ReturnOk()
        {
            var result = _controller.Attend(new AttendanceDto { GigId = 1 });
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public void DeleteAttendance_AttendanceDoesNotExist_ReturnNotFound()
        {

            var result = _controller.DeleteAttendance(1);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void DeleteAttendance_AttendanceExists_ReturnOk()
        {
            var attendance = new Attendance
            {
                Attendee = new ApplicationUser { Id = _userId },
                Gig = new Gig { Id = It.IsAny<int>() }
            };
            _mockRepository
                .Setup(r => r.GetAttendance(1, "1")).Returns(attendance);

            var result = _controller.DeleteAttendance(1);
            result.Should().BeOfType<OkNegotiatedContentResult<int>>();
        }
    }
}
