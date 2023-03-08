using GigHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace GigHub.Persistence.EntityConfigurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(u => u.Artists)
                .WithRequired(f => f.Follower)
                .WillCascadeOnDelete(false);

            HasMany(u => u.Followers)
                .WithRequired(f => f.Artist)
                .WillCascadeOnDelete(false);

            HasMany(u => u.UserNotifications)
                .WithRequired(un => un.User)
                .WillCascadeOnDelete(false);
        }
    }
}