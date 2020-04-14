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
           // SeedTipoChamdo(context);
            SeedProjetos(context);
            SeedCustonFieldProjetos(context);
            context.SaveChangesSeed();
        }

        private void SeedProcessos(Contexto.Contexto context)
        {
            if(!context.Processos.Any())
            {
                context.Processos.AddOrUpdate(//p => p.ProcessoId,
                new Processo
                {
                    ProcessoId= 1,
                    Nome = "Sustentação"
                },
                new Processo
                {
                    ProcessoId =2,
                    Nome = "Fiplan Projeto"
                },
                new Processo
                {
                    ProcessoId = 9,
                    Nome = "SAC Digital"
                },
                new Processo
                {
                    ProcessoId = 10,
                    Nome = "Fiplan Sustentação"
                },
                 new Processo
                 {
                     ProcessoId = 12,
                     Nome = "Kanban Projeto - Upstream"
                 },
                  new Processo
                  {
                      ProcessoId = 13,
                      Nome = "Kanban Projeto - DownStream"
                  }
                );

               
            }
        }

        private void SeedFases(Contexto.Contexto context)
        {
            if(!context.Fases.Any())
            {
                context.Fases.AddOrUpdate(//f => f.FaseId,
                new Fase
                {
                    FaseId= 1,
                    Descricao = "Backlog",
                },
                new Fase
                {
                    FaseId=2,
                    Descricao = "Desenvolvimento",
                },
                new Fase
                {
                    FaseId = 3,
                    Descricao = "Pré-Homologação",
                },
                new Fase
                {
                    FaseId = 4,
                    Descricao = "Homologação",
                },
                new Fase
                {
                    FaseId = 5,
                    Descricao = "Produção",
                },
                new Fase
                {
                    FaseId = 6,
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
                        ProjetoId = 1,
                        Descricao = "Time Traiket",
                        IdExterno = "519",
                        Identifier = "time-traiket",
                        Ativo = true,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 4,
                        ProcessoId = 1
                    },
                    new Projeto()
                    {
                        ProjetoId = 2,
                        Descricao = "FIPLAN PROJETO - Todo",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 15,
                        ProcessoId = 2,

                    },
                    new Projeto()
                    {
                    ProjetoId = 3,
                        Descricao = "FIPLAN SUSTENTAÇÃO - Todo",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 15,
                        ProcessoId = 10,
                        
                    },
                    new Projeto()
                    {
                    ProjetoId = 4,
                        Descricao = "FIPLAN PROJETO - SEFAZ",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 9,
                        ProcessoId = 2,
                        
                    },
                    new Projeto()
                    {
                    ProjetoId = 5,
                        Descricao = "FIPLAN SUSTENTAÇÃO - SEFAZ",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 9,
                        ProcessoId = 10,
                        
                    },
                    new Projeto()
                    {
                    ProjetoId = 6,
                        Descricao = "FIPLAN PROJETO - SEPLAN",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 5,
                        ProcessoId = 2,
                        
                    },
                    new Projeto()
                    {
                    ProjetoId = 7,
                        Descricao = "FIPLAN SUSTENTAÇÃO - SEPLAN",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 5,
                        ProcessoId = 10,
                        
                    },
                    new Projeto()
                    {
                    ProjetoId = 8,
                        Descricao = "FIPLAN PROJETO - SPF",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 1,
                        ProcessoId = 2,
                        
                    },
                    new Projeto()
                    {
                        ProjetoId = 9,
                        Descricao = "FIPLAN SUSTENTAÇÃO - SPF",
                        IdExterno = "539",
                        Identifier = "projeto-fiplan",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 1,
                        ProcessoId = 10,
                        
                    },
                    new Projeto()
                    {
                        ProjetoId = 10,
                        Descricao = "Gotham - Upstream",
                        IdExterno = "584",
                        Identifier = "time-espinha-mole",
                        Ativo = false,
                        HorasContratadas = 0,
                        QuantidadeDesenvolvedores = 3,
                        ProcessoId = 12,

                    },
                      new Projeto()
                      {
                          ProjetoId = 11,
                          Descricao = "Gotham - DownStream",
                          IdExterno = "584",
                          Identifier = "time-espinha-mole",
                          Ativo = false,
                          HorasContratadas = 0,
                          QuantidadeDesenvolvedores = 3,
                          ProcessoId = 13,

                      },
                       new Projeto()
                       {
                           ProjetoId = 12,
                           Descricao = "Gotham - DownStream",
                           IdExterno = "584",
                           Identifier = "time-espinha-mole",
                           Ativo = false,
                           HorasContratadas = 0,
                           QuantidadeDesenvolvedores = 3,
                           ProcessoId = 13,

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
                        ProjetoId = 4
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SEFAZ",
                        ProjetoId = 5
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SEPLAN",
                        ProjetoId = 6
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SEPLAN",
                        ProjetoId = 7
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SPF",
                        ProjetoId = 8
                    },
                    new CustonFieldProjeto()
                    {
                        Campo = "cf_252",
                        Valor = "SPF",
                        ProjetoId = 9
                    }
                );
            }
        }
    }
}
