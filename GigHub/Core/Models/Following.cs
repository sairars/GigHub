namespace GigHub.Core.Models
{
    public class Following
    {
        public string FollowerId { get; set; }
        public string ArtistId { get; set; }
        public ApplicationUser Follower { get; set; }
        public ApplicationUser Artist { get; set; }
    }
}