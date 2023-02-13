using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var followerId = User.Identity.GetUserId();

            if (_context.Followings.Any(f => f.FollowerId == followerId &&
                                              f.ArtistId == followingDto.ArtistId))
                return BadRequest("This following already exists in the system.");

            var following = new Following
            {
                ArtistId = followingDto.ArtistId,
                FollowerId = followerId
            };

            _context.Followings.Add(following);
            _context.SaveChanges();
            return Ok();
        }
    }
}
