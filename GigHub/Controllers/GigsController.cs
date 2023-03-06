using GigHub.Models;
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
                Genres = _context.Genres.ToList(),
                Heading = "Add A Gig"
            };

            return View("GigForm", viewModel);
        }

        // GET Gigs/Edit/1
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gigInDb = _context.Gigs.SingleOrDefault(g => g.Id == id &&
                                                         g.ArtistId == userId);
            if (gigInDb == null)
                return HttpNotFound();

            var viewModel = new GigFormViewModel
            {
                Id = gigInDb.Id,
                Venue = gigInDb.Venue,
                Date = gigInDb.DateTime.ToString("MMM d yyy"),
                Time = gigInDb.DateTime.ToString("HH:mm"),
                GenreId = gigInDb.GenreId,
                Genres = _context.Genres.ToList(),
                Heading = "Edit A Gig"
            };

            return View("GigForm", viewModel);
        }

        // POST Gigs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
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


        // POST Gigs/Update/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == viewModel.Id &&
                                      g.ArtistId == userId);

            if (gig == null)
                return HttpNotFound();

            gig.Update(viewModel.Venue, viewModel.GetDateTime(), viewModel.GenreId);

            _context.SaveChanges();

            return RedirectToAction("Mine");
        }


        // GET Gigs/Mine
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var myUpcomingGigs = _context.Gigs
                .Where(g => g.ArtistId == userId &&
                            g.DateTime > DateTime.Now &&
                            !g.IsCancelled)
                .Include(g => g.Genre)
                .ToList();

            return View(myUpcomingGigs);

        }


        // GET Gigs/Attending
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigsAttending = _context.Attendances
                                        .Where(a => a.AttendeeId == userId &&
                                                    a.Gig.DateTime > DateTime.Now &&
                                                    !a.Gig.IsCancelled)
                                        .Select(a => a.Gig)
                                        .Include(g => g.Artist)
                                        .Include(g => g.Genre)
                                        .ToList();

            var attendances = _context.Attendances
                                        .Where(a => a.AttendeeId == userId &&
                                                    a.Gig.DateTime > DateTime.Now &&
                                                    !a.Gig.IsCancelled)
                                        .ToList()
                                        .ToLookup(a => a.GigId);

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = gigsAttending,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        // POST Gigs/Search
        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        // Get Gigs/Details/1
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g => g.Artist)
                .SingleOrDefault(g => g.Id == id);

            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel
            {
                Gig = gig,
                IsAttending = _context.Attendances
                    .SingleOrDefault(a => a.GigId == gig.Id && a.AttendeeId == userId) != null,
                IsFollowing = _context.Followings
                    .SingleOrDefault(f => f.ArtistId == gig.ArtistId && f.FollowerId == userId) != null
            };

            return View(viewModel);
        }
    }
}