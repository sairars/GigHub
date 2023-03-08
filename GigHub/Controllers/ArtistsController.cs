using GigHub.Core;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    [Authorize]
    public class ArtistsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArtistsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET Artists/Following
        public ActionResult Following()
        {
            var artists = _unitOfWork.Followings.GetArtistsFollowedByUser(User.Identity.GetUserId());
            return View(artists);
        }
    }
}