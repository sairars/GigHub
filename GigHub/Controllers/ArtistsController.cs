using System.Linq;
using GigHub.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    [Authorize]
    public class ArtistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Artists
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();

            var artists = _context.Followings
                                                    .Where(f => f.FollowerId == userId)
                                                    .Select(f => f.Artist)
                                                    .ToList();
            return View(artists);
        }
    }
}