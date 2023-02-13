﻿using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    [Authorize]
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET Gigs/Create
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.GenreId,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine");
        }


        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var myUpcomingGigs = _context.Gigs
                .Where(g => g.ArtistId == userId &&
                            g.DateTime > DateTime.Now)
                .Include(g => g.Genre)
                .ToList();

            return View(myUpcomingGigs);

        }
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigsAttending = _context.Attendances
                                        .Where(a => a.AttendeeId == userId)
                                        .Select(a => a.Gig)
                                        .Include(g => g.Artist)
                                        .Include(g => g.Genre)
                                        .ToList();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = gigsAttending,
                ShowActions = User.Identity.IsAuthenticated,
                Title = "Gigs I'm Attending",
                Heading = "Gigs I'm Attending"
            };

            return View("Gigs", viewModel);

        }
    }
}