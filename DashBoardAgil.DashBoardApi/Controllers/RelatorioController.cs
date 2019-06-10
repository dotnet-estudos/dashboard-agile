using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services;
using DashBoardAgil.Services.Domain;
using DashBoardAgil.Services.Enums;
using Newtonsoft.Json;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    public class RelatorioController : BaseController
    {
        // GET: Relatorio
        public ActionResult SLA()
        {
            //GetSelectSla();

            ViewBag.SelectStatusRedmineDe = GetSelectStatusRedmine(StatusRedmine.BacklogNovo);
            ViewBag.SelectStatusRedminePara = GetSelectStatusRedmine(StatusRedmine.BacklogAberto);

            GetSelectTipoContagem(TipoContagem.Hora);

            GetSelectTime("time-traiket", true);

            return View();
        }
        public ActionResult DiaDev()
        {
            GetSelectTime("", true);
            //GetSelectStatusRedmine();
            return View();
        }
        public ActionResult PrazoChamado()
        {
            ViewBag.title = "Prazo";
            GetSelectTime("time-traiket");
            GetSelectStatusRedmine(StatusRedmine.ProducaoAvaliar);
            return View();
        }
        public ActionResult Desvios()
        {
            ViewBag.title = "Desvio";
            GetSelectTime(null, true);

            return View();
        }
        public ActionResult CicleTime()
        {
            ViewBag.title = "Clicle Time";
            GetSelectTime(null, true);
            ViewBag.SelectStatusRedmineDe = GetSelectStatusRedmine(StatusRedmine.BacklogNovo);
            ViewBag.SelectStatusRedminePara = GetSelectStatusRedmine(StatusRedmine.BacklogAberto);
            GetSelectTipoChamado(null, true);
            GetSelectPrioridadeRedmine(null, true);

            var processo = new Processo()
            {
                Id = 1,
                Nome = "Sustentação",
                Fluxo = new List<Fase>
                {
                    #region BACKLOG
                    new Fase()
                    {
                        Id = 1,
                        Descricao = "Backlog",
                        RGB = "#1E90FF",
                        IdFase = null,
                        FasePai = null,
                        EStatusRedmine = null,
                        Ordem = 1,
                        OrdemSubFase = null,
                        IdProcesso = 1,
                        StatusAbertura = false,
                        StatusFechamento = false,
                        SubFases = new List<Fase>
                        {

                            new Fase()
                            {
                                Id = 2,
                                Descricao = "Backlog - Novo",
                                IdFase = 1,
                                FasePai = new Fase() {Id = 1, Descricao = "Backlog", Ordem = 1},
                                EStatusRedmine = StatusRedmine.BacklogNovo,
                                Ordem = 1,
                                OrdemSubFase = 1,
                                IdProcesso = 1,
                                StatusAbertura = true,
                                StatusFechamento = false,
                                SubFases = null
                            },
                            new Fase()
                            {
                                Id = 3,
                                Descricao = "Backlog - Aberto",
                                IdFase = 1,
                                FasePai = new Fase() {Id = 1, Descricao = "Backlog", Ordem = 1},
                                EStatusRedmine = StatusRedmine.BacklogAberto,
                                Ordem = 2,
                                OrdemSubFase = 2,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                        }


                    },

                    #endregion

                    #region Desenvolvimento
                    new Fase()
                    {
                        Id = 4,
                        Descricao = "Desenvolvimento",
                        RGB = "#3CB371",
                        IdFase = null,
                        FasePai = null,
                        EStatusRedmine = null,
                        Ordem = 2,
                        OrdemSubFase = null,
                        IdProcesso = 1,
                        StatusAbertura = false,
                        StatusFechamento = false,
                        SubFases = new List<Fase>
                        {

                            new Fase()
                            {
                                Id =5,
                                Descricao = "Desenvolvimento - Andamento",
                                IdFase = 4,
                                FasePai = new Fase() {Id = 2, Descricao = "Desenvolvimento", Ordem = 2},
                                EStatusRedmine = StatusRedmine.DesenvolvimentoAndamento,
                                Ordem = 3,
                                OrdemSubFase = 1,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                            new Fase()
                            {
                                Id = 6,
                                Descricao = "Desenvolvimento - Resolvido",
                                IdFase = 4,
                                FasePai =  new Fase() {Id = 2, Descricao = "Desenvolvimento", Ordem = 2},
                                EStatusRedmine = StatusRedmine.DesenvolvimentoResolvido,
                                Ordem = 4,
                                OrdemSubFase = 2,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                        }


                    },

                    #endregion

                    #region Pré-Homologação
                    new Fase()
                    {
                        Id = 7,
                        Descricao = "Pré-Homologação",
                        RGB = "#FF6347",
                        IdFase = null,
                        FasePai = null,
                        EStatusRedmine = null,
                        Ordem = 3,
                        OrdemSubFase = null,
                        IdProcesso = 1,
                        StatusAbertura = false,
                        StatusFechamento = false,
                        SubFases = new List<Fase>
                        {

                            new Fase()
                            {
                                Id = 8,
                                Descricao = "Pré-Homologação - A Fazer",
                                IdFase = 7,
                                FasePai = new Fase() {Id = 7, Descricao = "Pré-Homologação", Ordem = 3},
                                EStatusRedmine = StatusRedmine.PreHmlAFazer,
                                Ordem = 5,
                                OrdemSubFase = 1,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                            new Fase()
                            {
                                Id = 9,
                                Descricao = "Pré-Homologação - Fazendo",
                                IdFase = 7,
                                FasePai =   new Fase() {Id = 7, Descricao = "Pré-Homologação", Ordem = 3},
                                EStatusRedmine = StatusRedmine.PreHmlFazendo,
                                Ordem = 6,
                                OrdemSubFase = 2,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                             new Fase()
                            {
                                Id = 10,
                                Descricao = "Pré-Homologação - Feito",
                                IdFase = 7,
                                FasePai =  new Fase() {Id = 7, Descricao = "Pré-Homologação", Ordem = 3},
                                EStatusRedmine = StatusRedmine.PreHmlFeito,
                                Ordem = 7,
                                OrdemSubFase = 3,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                        }


                    },

                    #endregion

                    #region Homologação
                    new Fase()
                    {
                        Id = 11,
                        Descricao = "Homologação",
                        RGB = "#D3D3D3",
                        IdFase = null,
                        FasePai = null,
                        EStatusRedmine = null,
                        Ordem = 4,
                        OrdemSubFase = null,
                        IdProcesso = 1,
                        StatusAbertura = false,
                        StatusFechamento = false,
                        SubFases = new List<Fase>
                        {

                            new Fase()
                            {
                                Id = 12,
                                Descricao = "Homologação - Andamento",
                                IdFase = 11,
                                FasePai =   new Fase() {Id = 11, Descricao = "Homologação", Ordem = 4},
                                EStatusRedmine = StatusRedmine.HomologacaoAndamento,
                                Ordem = 8,
                                OrdemSubFase = 1,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                             new Fase()
                            {
                                Id = 13,
                                Descricao = "Homologação - Validado",
                                IdFase = 11,
                                FasePai =  new Fase() {Id = 11, Descricao = "Homologação", Ordem = 4},
                                EStatusRedmine = StatusRedmine.HomologacaoValidado,
                                Ordem = 9,
                                OrdemSubFase = 2,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                        }


                    },

                    #endregion

                     #region Produção
                    new Fase()
                    {
                        Id = 14,
                        Descricao = "Produção",
                        RGB = "#FFEFD5",
                        IdFase = null,
                        FasePai = null,
                        EStatusRedmine = null,
                        Ordem = 5,
                        OrdemSubFase = null,
                        IdProcesso = 1,
                        StatusAbertura = false,
                        StatusFechamento = false,
                        SubFases = new List<Fase>
                        {

                            new Fase()
                            {
                                Id = 15,
                                Descricao = "Produção - Avaliar Produção",
                                IdFase = 14,
                                FasePai =   new Fase() {Id = 14, Descricao = "Produção", Ordem = 5},
                                EStatusRedmine = StatusRedmine.ProducaoAvaliar,
                                Ordem = 10,
                                OrdemSubFase = 1,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = false,
                                SubFases = null
                            },
                        }


                    },

                    #endregion

                    #region fechamento
                    new Fase()
                    {
                        Id = 16,
                        Descricao = "Fechamento",
                        RGB = "#D8BFD8",
                        IdFase = null,
                        FasePai = null,
                        EStatusRedmine = null,
                        Ordem = 6,
                        OrdemSubFase = null,
                        IdProcesso = 1,
                        StatusAbertura = false,
                        StatusFechamento = false,
                        SubFases = new List<Fase>
                        {

                            new Fase()
                            {
                                Id = 17,
                                Descricao = "Fechado",
                                IdFase = 16,
                                FasePai =   new Fase() {Id = 16, Descricao = "Fechamento", Ordem = 6},
                                EStatusRedmine = StatusRedmine.Fechado,
                                Ordem = 11,
                                OrdemSubFase = 1,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = true,
                                SubFases = null
                            },
                            new Fase()
                            {
                                Id = 18,
                                Descricao = "Cancelado",
                                IdFase = 16,
                                FasePai =   new Fase() {Id = 16, Descricao = "Fechamento", Ordem = 6},
                                EStatusRedmine = StatusRedmine.Cancelado,
                                Ordem = 12,
                                OrdemSubFase = 2,
                                IdProcesso = 1,
                                StatusAbertura = false,
                                StatusFechamento = true,
                                SubFases = null
                            },
                        }


                    },

                    #endregion
                }
            };

            Global.Instance.GravarArquivo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Json"), JsonConvert.SerializeObject(processo), "processo.json");

            return View(processo);
        }
        
    }
}
