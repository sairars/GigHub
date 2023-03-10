using GigHub.Core.Repositories;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IGigRepository Gigs { get; }
        IAttendanceRepository Attendances { get; }
        IFollowingRepository Followings { get; }
        IGenreRepository Genres { get; }
        IUserNotificationRepository UserNotifications { get; }
        IApplicationUserRepository Users { get; }
        INotificationRepository Notifications { get; }
        void Complete();
    }
}