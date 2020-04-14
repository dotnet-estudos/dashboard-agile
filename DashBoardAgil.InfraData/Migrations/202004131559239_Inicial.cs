namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustonFieldProjeto",
                c => new
                    {
                        CFId = c.Int(nullable: false, identity: true),
                        Campo = c.String(maxLength: 100, unicode: false),
                        Valor = c.String(maxLength: 100, unicode: false),
                        ProjetoId = c.String(maxLength: 100, unicode: false),
                        Projeto_ProjetoId = c.Int(),
                    })
                .PrimaryKey(t => t.CFId)
                .ForeignKey("dbo.Projeto", t => t.Projeto_ProjetoId)
                .Index(t => t.Projeto_ProjetoId);
            
            CreateTable(
                "dbo.Projeto",
                c => new
                    {
                        ProjetoId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        IdExterno = c.String(nullable: false, maxLength: 100, unicode: false),
                        Identifier = c.String(nullable: false, maxLength: 100, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        HorasContratadas = c.Int(nullable: false),
                        QuantidadeDesenvolvedores = c.Int(nullable: false),
                        ProcessoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjetoId)
                .ForeignKey("dbo.Processo", t => t.ProcessoId)
                .Index(t => t.ProcessoId);
            
            CreateTable(
                "dbo.Processo",
                c => new
                    {
                        ProcessoId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.ProcessoId);
            
            CreateTable(
                "dbo.PFS",
                c => new
                    {
                        ProcessoId = c.Int(nullable: false),
                        FaseId = c.Int(nullable: false),
                        SubFaseId = c.Int(nullable: false),
                        StatusRedmine = c.Int(nullable: false),
                        Situacao = c.Int(nullable: false),
                        BackLog = c.Boolean(nullable: false),
                        Cliente = c.Boolean(nullable: false),
                        ValidadoPO = c.Boolean(nullable: false),
                        Desenvolvimento = c.Boolean(nullable: false),
                        Ordem = c.Int(nullable: false),
                        Entregue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProcessoId, t.FaseId, t.SubFaseId })
                .ForeignKey("dbo.Fase", t => t.FaseId)
                .ForeignKey("dbo.Processo", t => t.ProcessoId)
                .ForeignKey("dbo.SubFase", t => t.SubFaseId)
                .Index(t => t.ProcessoId)
                .Index(t => t.FaseId)
                .Index(t => t.SubFaseId);
            
            CreateTable(
                "dbo.Fase",
                c => new
                    {
                        FaseId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.FaseId);
            
            CreateTable(
                "dbo.SubFase",
                c => new
                    {
                        SubFaseId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.SubFaseId);
            
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
                        IdExterno = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.TipoChamadoId);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        Tabela = c.String(maxLength: 100, unicode: false),
                        Acao = c.String(maxLength: 100, unicode: false),
                        NomeUsuario = c.String(maxLength: 100, unicode: false),
                        Chaves = c.String(maxLength: 100, unicode: false),
                        NomeColuna = c.String(maxLength: 100, unicode: false),
                        Propriedade = c.String(maxLength: 100, unicode: false),
                        ValorOriginal = c.String(maxLength: 100, unicode: false),
                        ValorAtual = c.String(maxLength: 100, unicode: false),
                        DataOcorrencia = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projeto", "ProcessoId", "dbo.Processo");
            DropForeignKey("dbo.ProcessoTipo", "TipoChamadoId", "dbo.TipoChamado");
            DropForeignKey("dbo.ProcessoTipo", "ProcessoId", "dbo.Processo");
            DropForeignKey("dbo.PFS", "SubFaseId", "dbo.SubFase");
            DropForeignKey("dbo.PFS", "ProcessoId", "dbo.Processo");
            DropForeignKey("dbo.PFS", "FaseId", "dbo.Fase");
            DropForeignKey("dbo.CustonFieldProjeto", "Projeto_ProjetoId", "dbo.Projeto");
            DropIndex("dbo.ProcessoTipo", new[] { "TipoChamadoId" });
            DropIndex("dbo.ProcessoTipo", new[] { "ProcessoId" });
            DropIndex("dbo.PFS", new[] { "SubFaseId" });
            DropIndex("dbo.PFS", new[] { "FaseId" });
            DropIndex("dbo.PFS", new[] { "ProcessoId" });
            DropIndex("dbo.Projeto", new[] { "ProcessoId" });
            DropIndex("dbo.CustonFieldProjeto", new[] { "Projeto_ProjetoId" });
            DropTable("dbo.Log");
            DropTable("dbo.TipoChamado");
            DropTable("dbo.ProcessoTipo");
            DropTable("dbo.SubFase");
            DropTable("dbo.Fase");
            DropTable("dbo.PFS");
            DropTable("dbo.Processo");
            DropTable("dbo.Projeto");
            DropTable("dbo.CustonFieldProjeto");
        }
    }
}
