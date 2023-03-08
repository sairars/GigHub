namespace GigHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenameToFolloweeIdInFollowingsTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Followings", name: "ArtistId", newName: "FolloweeId");
            RenameIndex(table: "dbo.Followings", name: "IX_ArtistId", newName: "IX_FolloweeId");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.Followings", name: "IX_FolloweeId", newName: "IX_ArtistId");
            RenameColumn(table: "dbo.Followings", name: "FolloweeId", newName: "ArtistId");
        }
    }
}
