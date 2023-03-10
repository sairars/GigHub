using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Persistence.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGigRepository Gigs { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IUserNotificationRepository UserNotifications { get; }
        public IApplicationUserRepository Users { get; }
        public INotificationRepository Notifications { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(_context);
            Attendances = new AttendanceRepository(context);
            Followings = new FollowingRepository(_context);
            Genres = new GenreRepository(_context);
            UserNotifications = new UserNotificationRepository(_context);
            Users = new ApplicationUserRepository(_context);
            Notifications = new NotificationRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}