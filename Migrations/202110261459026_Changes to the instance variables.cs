namespace WeatherProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changestotheinstancevariables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventInfoes", "TimeOfEvent", c => c.String());
            AddColumn("dbo.EventInfoes", "City", c => c.String());
            AddColumn("dbo.EventInfoes", "URL", c => c.String());
            AddColumn("dbo.EventInfoes", "IsRemote", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventInfoes", "Latitude", c => c.String());
            AddColumn("dbo.EventInfoes", "Longtitude", c => c.String());
            DropColumn("dbo.EventInfoes", "Description");
            DropColumn("dbo.EventInfoes", "HourOfEvent");
            DropColumn("dbo.EventInfoes", "MinOfEvent");
            DropColumn("dbo.EventInfoes", "SlugForURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventInfoes", "SlugForURL", c => c.String());
            AddColumn("dbo.EventInfoes", "MinOfEvent", c => c.Int(nullable: false));
            AddColumn("dbo.EventInfoes", "HourOfEvent", c => c.Int(nullable: false));
            AddColumn("dbo.EventInfoes", "Description", c => c.String());
            DropColumn("dbo.EventInfoes", "Longtitude");
            DropColumn("dbo.EventInfoes", "Latitude");
            DropColumn("dbo.EventInfoes", "IsRemote");
            DropColumn("dbo.EventInfoes", "URL");
            DropColumn("dbo.EventInfoes", "City");
            DropColumn("dbo.EventInfoes", "TimeOfEvent");
        }
    }
}
