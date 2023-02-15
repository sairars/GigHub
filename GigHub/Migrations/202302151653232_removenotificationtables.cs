namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removenotificationtables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "GigId", "dbo.Gigs");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "GigId" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropTable("dbo.Notifications");
            DropTable("dbo.UserNotifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        NotificationId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.NotificationId });
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        OriginalVenue = c.String(),
                        OriginalDateTime = c.DateTime(),
                        GigId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserNotifications", "NotificationId");
            CreateIndex("dbo.UserNotifications", "UserId");
            CreateIndex("dbo.Notifications", "GigId");
            AddForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Notifications", "GigId", "dbo.Gigs", "Id", cascadeDelete: true);
        }
    }
}
