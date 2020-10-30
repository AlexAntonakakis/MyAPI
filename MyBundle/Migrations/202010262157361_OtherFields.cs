namespace MyBundle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OtherFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ExpDate", c => c.DateTime());
            AddColumn("dbo.Products", "AvailDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Products", "Active", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Active");
            DropColumn("dbo.Products", "AvailDate");
            DropColumn("dbo.Products", "ExpDate");
        }
    }
}
