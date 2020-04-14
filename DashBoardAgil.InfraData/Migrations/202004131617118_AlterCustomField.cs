namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCustomField : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CustonFieldProjeto", new[] { "Projeto_ProjetoId" });
            DropColumn("dbo.CustonFieldProjeto", "ProjetoId");
            RenameColumn(table: "dbo.CustonFieldProjeto", name: "Projeto_ProjetoId", newName: "ProjetoId");
            AlterColumn("dbo.CustonFieldProjeto", "ProjetoId", c => c.Int(nullable: false));
            AlterColumn("dbo.CustonFieldProjeto", "ProjetoId", c => c.Int(nullable: false));
            CreateIndex("dbo.CustonFieldProjeto", "ProjetoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CustonFieldProjeto", new[] { "ProjetoId" });
            AlterColumn("dbo.CustonFieldProjeto", "ProjetoId", c => c.Int());
            AlterColumn("dbo.CustonFieldProjeto", "ProjetoId", c => c.String(maxLength: 100, unicode: false));
            RenameColumn(table: "dbo.CustonFieldProjeto", name: "ProjetoId", newName: "Projeto_ProjetoId");
            AddColumn("dbo.CustonFieldProjeto", "ProjetoId", c => c.String(maxLength: 100, unicode: false));
            CreateIndex("dbo.CustonFieldProjeto", "Projeto_ProjetoId");
        }
    }
}
