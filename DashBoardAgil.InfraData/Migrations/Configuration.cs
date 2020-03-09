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
            SeedProjetos(context);
            SeedCustonFieldProjetos(context);
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

        private void SeedProjetos(Contexto.Contexto context)
        {
            if (!context.Projetos.Any())
            {
                context.Projetos.AddOrUpdate(
                    new Projeto()
                    {
                        Id = "1",
                        Descricao = "Time Traiket",
                        IdProjeto = "519",
                        Identifier = "time-traiket",
                        Ativo = true,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 4,
                        ProcessoId = 1
                    },
                    new Projeto()
                    {
                        Id = "2",
                        Descricao = "FIPLAN PROJETO - Todo",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 15,
                        ProcessoId = 2,

                    },
                    new Projeto()
                    {
                    Id = "3",
                        Descricao = "FIPLAN SUSTENTAÇÃO - Todo",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 15,
                        ProcessoId = 10,
                        
                    },
                    new Projeto()
                    {
                    Id = "4",
                        Descricao = "FIPLAN PROJETO - SEFAZ",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 9,
                        ProcessoId = 2,
                        
                    },
                    new Projeto()
                    {
                    Id = "5",
                        Descricao = "FIPLAN SUSTENTAÇÃO - SEFAZ",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 9,
                        ProcessoId = 10,
                        
                    },
                    new Projeto()
                    {
                    Id = "6",
                        Descricao = "FIPLAN PROJETO - SEPLAN",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 5,
                        ProcessoId = 2,
                        
                    },
                    new Projeto()
                    {
                    Id = "7",
                        Descricao = "FIPLAN SUSTENTAÇÃO - SEPLAN",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 5,
                        ProcessoId = 10,
                        
                    },
                    new Projeto()
                    {
                    Id = "8",
                        Descricao = "FIPLAN PROJETO - SPF",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 1,
                        ProcessoId = 2,
                        
                    },
                    new Projeto()
                    {
                    Id = "9",
                        Descricao = "FIPLAN SUSTENTAÇÃO - SPF",
                        IdProjeto = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 1,
                        ProcessoId = 10,
                        
                    }
                );
            }
        }

        private void SeedCustonFieldProjetos(Contexto.Contexto context)
        {
            if (!context.CustonFieldProjetos.Any())
            {
                context.CustonFieldProjetos.AddOrUpdate(
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SEFAZ",
                        ProjetoId = "4"
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SEFAZ",
                        ProjetoId = "5"
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SEPLAN",
                        ProjetoId = "6"
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SEPLAN",
                        ProjetoId = "7"
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SPF",
                        ProjetoId = "8"
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SPF",
                        ProjetoId = "9"
                    }
                );
            }
        }
    }
}
