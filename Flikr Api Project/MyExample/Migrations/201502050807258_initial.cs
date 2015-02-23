namespace MyExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
         
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        owner = c.String(),
                        secret = c.String(),
                        server = c.String(),
                        farm = c.Int(nullable: false),
                        title = c.String(),
                        ispublic = c.Int(nullable: false),
                        isfriend = c.Int(nullable: false),
                        isfamily = c.Int(nullable: false),
                        keyword = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        ReleaseDate = c.DateTime(nullable: false),
                        Genre = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rating = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.UserProfile");
        }
    }
}
