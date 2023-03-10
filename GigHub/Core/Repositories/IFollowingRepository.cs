using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string artistId, string followerId);
        void Add(Following following);
        void Remove(Following following);
    }
}