using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET Home/Index
        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _context.Gigs
                                        .Include(g => g.Artist)
                                        .Include(g => g.Genre)
                                        .Where(g => g.DateTime > DateTime.Now &&
                                                    !g.IsCancelled);

            if (!string.IsNullOrWhiteSpace(query))
                upcomingGigs = upcomingGigs.Where(ug => ug.Artist.Name.Contains(query) ||
                                                        ug.Genre.Name.Contains(query) ||
                                                        ug.Venue.Contains(query));

            var userId = User.Identity.GetUserId();

            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId &&
                            a.Gig.DateTime > DateTime.Now &&
                            !a.Gig.IsCancelled)
                .ToList()
                .ToLookup(a => a.GigId);

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}