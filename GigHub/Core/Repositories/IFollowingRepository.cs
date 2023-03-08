using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string artistId, string followerId);
        IEnumerable<ApplicationUser> GetArtistsFollowedByUser(string userId);
        void Add(Following following);
        void Remove(Following following);
    }
}