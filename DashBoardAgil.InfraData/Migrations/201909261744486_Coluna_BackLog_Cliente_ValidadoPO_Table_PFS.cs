namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Coluna_BackLog_Cliente_ValidadoPO_Table_PFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PFS", "BackLog", c => c.Boolean(nullable: false));
            AddColumn("dbo.PFS", "Cliente", c => c.Boolean(nullable: false));
            AddColumn("dbo.PFS", "ValidadoPO", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PFS", "ValidadoPO");
            DropColumn("dbo.PFS", "Cliente");
            DropColumn("dbo.PFS", "BackLog");
        }
    }
}
