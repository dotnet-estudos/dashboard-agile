namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtipochamadoprocessotipo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessoTipo",
                c => new
                    {
                        ProcessoId = c.Int(nullable: false),
                        TipoChamadoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProcessoId, t.TipoChamadoId })
                .ForeignKey("dbo.Processo", t => t.ProcessoId)
                .ForeignKey("dbo.TipoChamado", t => t.TipoChamadoId)
                .Index(t => t.ProcessoId)
                .Index(t => t.TipoChamadoId);
            
            CreateTable(
                "dbo.TipoChamado",
                c => new
                    {
                        TipoChamadoId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.TipoChamadoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProcessoTipo", "TipoChamadoId", "dbo.TipoChamado");
            DropForeignKey("dbo.ProcessoTipo", "ProcessoId", "dbo.Processo");
            DropIndex("dbo.ProcessoTipo", new[] { "TipoChamadoId" });
            DropIndex("dbo.ProcessoTipo", new[] { "ProcessoId" });
            DropTable("dbo.TipoChamado");
            DropTable("dbo.ProcessoTipo");
        }
    }
}
