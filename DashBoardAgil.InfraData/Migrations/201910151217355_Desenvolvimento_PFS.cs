namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Desenvolvimento_PFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PFS", "Desenvolvimento", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PFS", "Desenvolvimento");
        }
    }
}
