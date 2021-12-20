namespace BrokerAppTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStocksQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BrokerInfoes", "StocksQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BrokerInfoes", "StocksQuantity");
        }
    }
}
