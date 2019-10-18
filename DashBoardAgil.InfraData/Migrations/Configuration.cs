namespace DashBoardAgil.InfraData.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Domain.Models;

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
            SeedTipoChamdo(context);
            context.SaveChangesSeed();
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

        private void SeedTipoChamdo(Contexto.Contexto context)
        {
            if (!context.TipoChamado.Any())
            {
                context.TipoChamado.AddOrUpdate(
                    new TipoChamado()
                    {
                        Descricao = "Evolutiva",
                        IdExterno = "83"
                    },
                    new TipoChamado()
                    {
                        Descricao = "Adaptativa",
                        IdExterno = "86"
                    },
                    new TipoChamado()
                    {
                        Descricao = "Consulta",
                        IdExterno = "87"
                    },
                     new TipoChamado()
                     {
                         Descricao = "Corretiva",
                         IdExterno = "85"
                     },
                     new TipoChamado()
                     {
                         Descricao = "Fiplan: Evolutiva",
                         IdExterno = "96"
                     },
                     new TipoChamado()
                     {
                         Descricao = "Fiplan: Adaptativa",
                         IdExterno = "93"
                     },
                     new TipoChamado()
                     {
                         Descricao = "Fiplan:  Consulta",
                         IdExterno = "100"
                     },
                     new TipoChamado()
                     {
                         Descricao = "Fiplan:  Corretiva",
                         IdExterno = "99"
                     }
                    );
            }
        }
    }
}
