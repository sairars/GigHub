using FluentAssertions;
using GigHub.Controllers;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.IntegrationTests.Extensions;
using GigHub.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.IntegrationTests.Controllers
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
        public void Mine_WhenCalled_ShouldReturnUpcomingGigs()
        {
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var genre = _context.Genres.First();
            var gig = new Gig { Artist = user, DateTime = DateTime.Now.AddDays(1), Genre = genre, Venue = "-" };
            _context.Gigs.Add(gig);
            _context.SaveChanges();

            var result = _controller.Mine();

            (result.ViewData.Model as IEnumerable<Gig>).Should().HaveCount(1);
        }

        [Test, Isolated]
        public void Mine_WhenCalled_ShouldUpdateTheGivenGig()
        {
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var genre = _context.Genres.Single(g => g.Id == 1);
            var genres = _context.Genres.ToList();
            var gig = new Gig { Artist = user, DateTime = DateTime.Now.AddDays(1), Genre = genre, Venue = "-" };
            _context.Gigs.Add(gig);
            _context.SaveChanges();

            var viewModel = new GigFormViewModel
            {
                Id = gig.Id,
                Date = DateTime.Today.AddMonths(1).ToString("MMM d yyyy"),
                GenreId = 2,
                Time = "20:00",
                Venue = "Venue"
            };

            var result = _controller.Update(viewModel);

            _context.Entry(gig).Reload();
            gig.DateTime.Should().Be(DateTime.Today.AddMonths(1).AddHours(20));
            gig.GenreId.Should().Be(2);
            gig.Venue.Should().Be("Venue");
        }
    }
}
