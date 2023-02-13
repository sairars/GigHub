namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFolloweeToArtistInFollowingsTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Followings", name: "FolloweeId", newName: "ArtistId");
            RenameIndex(table: "dbo.Followings", name: "IX_FolloweeId", newName: "IX_ArtistId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Followings", name: "IX_ArtistId", newName: "IX_FolloweeId");
            RenameColumn(table: "dbo.Followings", name: "ArtistId", newName: "FolloweeId");
        }
    }
}
