using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Tests.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Data.Entity;

namespace GigHub.Tests.Persistence.Repositories
{
    [TestFixture]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockGigs;
        private Mock<DbSet<Attendance>> _mockAttendances;
        private Mock<IApplicationDbContext> _mockContext;

        [SetUp]
        public void SetUp()
        {
            _mockGigs = new Mock<DbSet<Gig>>();
            _mockAttendances = new Mock<DbSet<Attendance>>();
            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);
            _mockContext.SetupGet(c => c.Attendances).Returns(_mockAttendances.Object);
            _repository = new GigRepository(_mockContext.Object);
        }

        [Test]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotReturnGig()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(-1),
                ArtistId = "1"
            };

            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId);
            gigs.Should().BeEmpty();
        }

        [Test]
        public void GetUpcomingGigsByArtist_GigIsInTheFutureButCancelled_ShouldNotReturnGig()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };

            gig.Cancel();
            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId);
            gigs.Should().BeEmpty();
        }

        [Test]
        public void GetUpcomingGigsByArtist_GigIsInTheFutureButByDifferentArtist_ShouldNotReturnGig()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };

            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId + "-");
            gigs.Should().BeEmpty();
        }

        [Test]
        public void GetUpcomingGigsByArtist_GigIsInFutureBySameArtistNotCancelled_ShouldReturnAllFutureGigsByArtist()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };

            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId);
            gigs.Should().Contain(gig);
        }

        [Test]
        public void GetGigsUserIsAttending_GigIsInThePast_ShouldNotReturnGigs()
        {
            var attendance = new Attendance
            {
                AttendeeId = "1",
                Gig = new Gig { DateTime = DateTime.Now.AddDays(-1) }
            };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserIsAttending(attendance.AttendeeId);

            gigs.Should().BeEmpty();
        }

        [Test]
        public void GetGigsUserIsAttending_NoGigAttendanceExistForThisUser_ShouldNotReturnGigs()
        {
            var attendance = new Attendance
            {
                AttendeeId = "1",
                Gig = new Gig()
            };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserIsAttending(attendance.AttendeeId + "-");

            gigs.Should().BeEmpty();
        }

        [Test]
        public void GetGigsUserIsAttending_FutureGigAttendanceExistForThisUser_ShouldNotReturnGigs()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1)
            };

            var attendance = new Attendance
            {
                AttendeeId = "1",
                Gig = gig
            };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserIsAttending(attendance.AttendeeId);

            gigs.Should().Contain(gig);
        }
    }
}
