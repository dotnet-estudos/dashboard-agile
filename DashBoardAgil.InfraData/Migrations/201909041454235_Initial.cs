namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fase",
                c => new
                    {
                        FaseId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        Cor = c.String(maxLength: 100, unicode: false),
                        Processo_ProcessoId = c.Int(),
                    })
                .PrimaryKey(t => t.FaseId)
                .ForeignKey("dbo.Processo", t => t.Processo_ProcessoId)
                .Index(t => t.Processo_ProcessoId);
            
            CreateTable(
                "dbo.SubFase",
                c => new
                    {
                        SubFaseId = c.Int(nullable: false, identity: true),
                        FaseId = c.Int(nullable: false),
                        Ordem = c.Int(nullable: false),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        StatusRedmine = c.Int(nullable: false),
                        Situacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubFaseId)
                .ForeignKey("dbo.Fase", t => t.FaseId)
                .Index(t => t.FaseId);
            
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
            
            CreateTable(
                "dbo.Processo",
                c => new
                    {
                        ProcessoId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.ProcessoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fase", "Processo_ProcessoId", "dbo.Processo");
            DropForeignKey("dbo.SubFase", "FaseId", "dbo.Fase");
            DropIndex("dbo.SubFase", new[] { "FaseId" });
            DropIndex("dbo.Fase", new[] { "Processo_ProcessoId" });
            DropTable("dbo.Processo");
            DropTable("dbo.Log");
            DropTable("dbo.SubFase");
            DropTable("dbo.Fase");
        }
    }
}
