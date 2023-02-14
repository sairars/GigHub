using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.api
{
    [Authorize]
    public class GigsController : ApiController
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


        // Delete /api/Gigs
        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gigInDb = _context.Gigs.SingleOrDefault(g => g.Id == id &&
                                                             g.ArtistId == userId &&
                                                             !g.IsCancelled);

            if (gigInDb == null)
                return NotFound();

            gigInDb.IsCancelled = true;
            _context.SaveChanges();

            return Ok();
        }
    }
}
