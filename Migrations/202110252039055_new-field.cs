namespace WeatherProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfield : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventInfoes",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        Summary = c.String(),
                        Description = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        HourOfEvent = c.Int(nullable: false),
                        MinOfEvent = c.Int(nullable: false),
                        WeatherDescription = c.String(),
                        SlugForURL = c.String(),
                    })
                .PrimaryKey(t => t.EventID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventInfoes");
        }
    }
}
