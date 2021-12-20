namespace BrokerAppTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBrokerDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BrokerInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrgName = c.String(nullable: false, maxLength: 50),
                        Deposit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OperationDate = c.DateTime(nullable: false),
                        Quantuty = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSale = c.Boolean(nullable: false),
                        BrokerInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrokerInfoes", t => t.BrokerInfoId, cascadeDelete: true)
                .Index(t => t.BrokerInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "BrokerInfoId", "dbo.BrokerInfoes");
            DropIndex("dbo.Operations", new[] { "BrokerInfoId" });
            DropTable("dbo.Operations");
            DropTable("dbo.BrokerInfoes");
        }
    }
}
