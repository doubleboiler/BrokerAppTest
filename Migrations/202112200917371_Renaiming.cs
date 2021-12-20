namespace BrokerAppTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renaiming : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operations", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Operations", "Quantuty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operations", "Quantuty", c => c.Int(nullable: false));
            DropColumn("dbo.Operations", "Quantity");
        }
    }
}
