namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProcessoFase", "Processo_ProcessoId", "dbo.Processo");
            DropForeignKey("dbo.ProcessoFase", "Fase_FaseId", "dbo.Fase");
            DropForeignKey("dbo.SubFase", "FaseId", "dbo.Fase");
            DropIndex("dbo.SubFase", new[] { "FaseId" });
            DropIndex("dbo.ProcessoFase", new[] { "Processo_ProcessoId" });
            DropIndex("dbo.ProcessoFase", new[] { "Fase_FaseId" });
            CreateTable(
                "dbo.PFS",
                c => new
                    {
                        ProcessoId = c.Int(nullable: false),
                        FaseId = c.Int(nullable: false),
                        SubFaseId = c.Int(nullable: false),
                        StatusRedmine = c.Int(nullable: false),
                        Situacao = c.Int(nullable: false),
                        Ordem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProcessoId, t.FaseId, t.SubFaseId })
                .ForeignKey("dbo.Fase", t => t.FaseId)
                .ForeignKey("dbo.Processo", t => t.ProcessoId)
                .ForeignKey("dbo.SubFase", t => t.SubFaseId)
                .Index(t => t.ProcessoId)
                .Index(t => t.FaseId)
                .Index(t => t.SubFaseId);
            
            DropColumn("dbo.Fase", "Cor");
            DropColumn("dbo.SubFase", "FaseId");
            DropColumn("dbo.SubFase", "Ordem");
            DropColumn("dbo.SubFase", "StatusRedmine");
            DropColumn("dbo.SubFase", "Situacao");
            DropTable("dbo.ProcessoFase");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProcessoFase",
                c => new
                    {
                        Processo_ProcessoId = c.Int(nullable: false),
                        Fase_FaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Processo_ProcessoId, t.Fase_FaseId });
            
            AddColumn("dbo.SubFase", "Situacao", c => c.Int(nullable: false));
            AddColumn("dbo.SubFase", "StatusRedmine", c => c.Int(nullable: false));
            AddColumn("dbo.SubFase", "Ordem", c => c.Int(nullable: false));
            AddColumn("dbo.SubFase", "FaseId", c => c.Int(nullable: false));
            AddColumn("dbo.Fase", "Cor", c => c.String(maxLength: 100, unicode: false));
            DropForeignKey("dbo.PFS", "SubFaseId", "dbo.SubFase");
            DropForeignKey("dbo.PFS", "ProcessoId", "dbo.Processo");
            DropForeignKey("dbo.PFS", "FaseId", "dbo.Fase");
            DropIndex("dbo.PFS", new[] { "SubFaseId" });
            DropIndex("dbo.PFS", new[] { "FaseId" });
            DropIndex("dbo.PFS", new[] { "ProcessoId" });
            DropTable("dbo.PFS");
            CreateIndex("dbo.ProcessoFase", "Fase_FaseId");
            CreateIndex("dbo.ProcessoFase", "Processo_ProcessoId");
            CreateIndex("dbo.SubFase", "FaseId");
            AddForeignKey("dbo.SubFase", "FaseId", "dbo.Fase", "FaseId");
            AddForeignKey("dbo.ProcessoFase", "Fase_FaseId", "dbo.Fase", "FaseId");
            AddForeignKey("dbo.ProcessoFase", "Processo_ProcessoId", "dbo.Processo", "ProcessoId");
        }
    }
}
