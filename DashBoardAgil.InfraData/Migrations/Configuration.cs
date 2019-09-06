namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Services.Domain;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexto.Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Contexto.Contexto context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            SeedProcessos(context);
            SeedFases(context);
        }

        private void SeedProcessos(Contexto.Contexto context)
        {
            if(!context.Processos.Any())
            {
                context.Processos.AddOrUpdate(p => p.ProcessoId,
                new Processo
                {
                    Nome = "Sustentação"
                });

                context.SaveChangesSeed();
            }
        }

        private void SeedFases(Contexto.Contexto context)
        {
            if(!context.Fases.Any())
            {
                context.Fases.AddOrUpdate(f => f.FaseId,
                new Fase
                {
                    Descricao = "Backlog",
                },
                new Fase
                {
                    Descricao = "Desenvolvimento",
                },
                new Fase
                {
                    Descricao = "Pré-Homologação",
                },
                new Fase
                {
                    Descricao = "Homologação",
                },
                new Fase
                {
                    Descricao = "Produção",
                },
                new Fase
                {
                    Descricao = "Fechamento",
                });
            }
        }
    }
}
