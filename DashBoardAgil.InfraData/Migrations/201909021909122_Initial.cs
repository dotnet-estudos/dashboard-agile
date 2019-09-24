namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        Cor = c.String(),
                        Processo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Processoes", t => t.Processo_Id)
                .Index(t => t.Processo_Id);
            
            CreateTable(
                "dbo.SubFases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFase = c.Int(nullable: false),
                        Ordem = c.Int(nullable: false),
                        Descricao = c.String(),
                        StatusRedmine = c.Int(nullable: false),
                        Situacao = c.Int(nullable: false),
                        Fase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fases", t => t.Fase_Id)
                .Index(t => t.Fase_Id);
            
            CreateTable(
                "dbo.Processoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fases", "Processo_Id", "dbo.Processoes");
            DropForeignKey("dbo.SubFases", "Fase_Id", "dbo.Fases");
            DropIndex("dbo.SubFases", new[] { "Fase_Id" });
            DropIndex("dbo.Fases", new[] { "Processo_Id" });
            DropTable("dbo.Processoes");
            DropTable("dbo.SubFases");
            DropTable("dbo.Fases");
        }
    }
}
