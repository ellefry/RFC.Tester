namespace Sap.Conn.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FunctionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProcessRequests");
        }
    }
}
