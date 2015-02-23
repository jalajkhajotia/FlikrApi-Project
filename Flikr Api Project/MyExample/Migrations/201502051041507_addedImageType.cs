namespace MyExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedImageType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "image320", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "image320");
        }
    }
}
