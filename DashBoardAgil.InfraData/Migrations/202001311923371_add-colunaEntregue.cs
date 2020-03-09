namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolunaEntregue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PFS", "Entregue", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PFS", "Entregue");
        }
    }
}
