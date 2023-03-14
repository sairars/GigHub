using FluentAssertions;
using GigHub.Controllers.api;
using GigHub.Core.Models;
using GigHub.IntegrationTests.Extensions;
using GigHub.Persistence;
using GigHub.Tests.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace GigHub.IntegrationTests.Controllers.api
{
    [TestFixture]
    public class GigsControllerTests
    {
        private GigsController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext();
            _controller = new GigsController(new UnitOfWork(_context));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public void Cancel_WhenCalled_ShouldCancelTheGivenGig()
        {
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var genre = _context.Genres.First();
            var gig = new Gig
            {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = genre,
                Venue = "-"
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            _controller.Cancel(gig.Id);

            _context.Entry(gig).Reload();

            gig.IsCancelled.Should().BeTrue();
        }
    }
}
