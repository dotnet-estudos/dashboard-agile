using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Interfaces;
using DashBoardAgil.Domain.Interfaces.Abstracoes;
using DashBoardAgil.Domain.Enums;
using DashBoardAgil.Services.Extensions;
using DashBoardAgil.Services.Util;
using Newtonsoft.Json;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using DashBoardAgil.Domain.Models;


namespace DashBoardAgil.Services.Service
{
    public class RedmineService : IRedmineService
    {
        private readonly RedmineExtensions _manager;

        public RedmineService(string host, string user, string pass)
        {
            _manager = new RedmineExtensions(host, user, pass);
        }

        public List<Issue> GetObjectsById(string[] ids)
        {
            List<Issue> issues = new List<Issue>();

            foreach (var id in ids)
            {
                try
                {
                    var issue = _manager.GetObject<Issue>(id, null);

                    if (issue != null)
                        issues.Add(issue);
                }
                catch (Exception) { }
            }

            return issues;
        }

        public List<Issue> GetObjectsByProjects(string[] projectIds, NameValueCollection parameters)
        {
            List<Issue> issues = new List<Issue>();

            foreach (var id in projectIds)
            {
                try
                {
                    var projectParam = new NameValueCollection { { RedmineKeys.PROJECT_ID, id } };

                    if (parameters != null) projectParam.Add(parameters);

                    var issue = _manager.GetObjects<Issue>(projectParam);

                    if (issue != null)
                        issues.AddRange(issue);
                }
                catch (Exception) { }
            }

            return issues;
        }

        public IEnumerable<string> FecharChamadosAvaliarProducao(string[] projetos, int prazo)
        {
            List<string> result = new List<string>();
            List<Issue> issues = new List<Issue>();

            var parameters = new NameValueCollection { { RedmineKeys.STATUS_ID, Global.Instance.ConcatenarLista("|", Constantes.FaseProducao) } };
            if (GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.Any(x => x.Ativo))
            {
                parameters.Add(RedmineKeys.SUBPROJECT_ID, String.Format("!{0}", Global.Instance.ConcatenarLista("|", GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.Where(x => x.Ativo).Select(x => x.Id.ToString()).ToList())));
            }

            foreach (var projectId in projetos)
            {
                try
                {
                    var paramProj = new NameValueCollection { { RedmineKeys.PROJECT_ID, projectId } };  //, { RedmineKeys.OFFSET, "0" }, { RedmineKeys.LIMIT, "1000" } };
                    paramProj.Add(parameters);
                    issues.AddRange(_manager.GetObjectsWithJournals(paramProj));
                }
                catch (Exception e)
                {

                    Debugar.WriteLine(String.Format("Erro ao pegar os chamados do time: {0}, Erro: {1}", GlobalConfig.Instance.ConfiguracaoRedmine.GetProjetoById(projectId).ToString(), e.Message));
                }
            }

            #region Atualizar no Redmine

            result = _manager.FecharChamados(issues, prazo, Constantes.FaseProducao.ToArray());
            #endregion

            return result;
        }

        public bool GerarCfdTeams()
        {
            Debugar.WriteLine("Iniciando ... ", "GERADOR CFD");

            var dataIni = new DateTime(DateTime.Now.Year, 5, 1);
            var dataFim = DateTime.Now;
            var lsitTipo = from TipoChamadoRedmine e in Enum.GetValues(typeof(TipoChamadoRedmine))
                           select new { Id = e.GetHashCode(), Nome = e.ToString() };
            var lststr = lsitTipo.Select(x => x.Id.ToString()).ToList();
            String time = null;
            GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.ForEach(team =>
            {
                try
                {
                    time = team.Identifier;

                    if (time != null)
                    {
                        Debugar.WriteLine(string.Format("Iniciando {0} ", time), "GERADOR CFD");
                        var parm = new NameValueCollection
                    {
                            {RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)},
                            {RedmineKeys.STATUS_ID, "*"},
                            {RedmineKeys.PROJECT_ID, String.Format("{0}", time)},
                            {RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", ((DateTime) dataIni).ToString("yyyy-MM-dd"),((DateTime) dataFim).ToString("yyyy-MM-dd"))}
                    };

                        var comulativeFlow = this.GetCumulativeFlowDiagram(parm, CrossCutting.Util.GetWeekList(dataIni, dataFim));
                        Global.Instance.GravarArquivo(ConstantesCC.PATH_CFD_DIAGRAM, JsonConvert.SerializeObject(comulativeFlow), String.Format("{0}.json", time));
                        Debugar.WriteLine(String.Format("{0} gerado CFD", team), "GERADOR CFD");
                    }
                    else
                    {
                        Debugar.WriteLine(String.Format("{0} Não gerado CFD", team), "GERADOR CFD");
                    }
                }
                catch (Exception ex)
                {
                    Debugar.WriteLine(String.Format("Error.. Time: {0} Error: {1} Inner:{2}", time, ex.Message, ex.InnerException.Message), "GERADOR CFD");
                }
            });
            Debugar.WriteLine("Finalizado ... ", "GERADOR CFD");

            return true;
        }

        public bool GerarThroughputReject()
        {
            Debugar.WriteLine("Iniciando ... ", "GerarThroughputReject");

            var dataIni = new DateTime(DateTime.Now.Year, 1, 1);
            var dataFim = DateTime.Now;
            var lsitTipo = from TipoChamadoRedmine e in Enum.GetValues(typeof(TipoChamadoRedmine))
                           select new { Id = e.GetHashCode(), Nome = e.ToString() };
            var lststr = lsitTipo.Select(x => x.Id.ToString()).ToList();
            String time = null;
            GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.ForEach(team =>
            {
                try
                {
                    time = team.Identifier;

                    if (time != null)
                    {
                        Debugar.WriteLine(string.Format("Iniciando {0} ", time), "GerarThroughputReject");
                        var parm = new NameValueCollection
                    {
                            {RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)},
                            {RedmineKeys.STATUS_ID, "closed"},
                            {RedmineKeys.PROJECT_ID, String.Format("{0}", time)},
                            {RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", ((DateTime) dataIni).ToString("yyyy-MM-dd"),((DateTime) dataFim).ToString("yyyy-MM-dd"))}
                    };
                        ///PAREI AQUI
                        var reject = this.GetRejectionRate(parm);

                        var thoupt = new ThroughputControl();

                        thoupt.DataFim = reject.DataFim = dataFim;
                        thoupt.DataIni = reject.DataIni = dataIni;

                        reject.Rejections.ForEach(rejection => thoupt.Throughputs.Add(CrossCutting.Util.ReflectObjec(rejection, new Throughput())));

                        var week = CrossCutting.Util.GetWeekList(dataIni, dataFim);

                        var unionSerieData = thoupt.ByTypeWeek.ToList();

                        reject.ByTypeWeek.ToList().ForEach(x => unionSerieData.Add(x));

                        Global.Instance.GravarArquivo(ConstantesCC.PATH_THROUGHPUT_REJECT_DIAGRAM, JsonConvert.SerializeObject(new { Rejeicao = reject, Throughput = thoupt, Serie = unionSerieData, Categoria = week }), String.Format("{0}.json", time));

                        Debugar.WriteLine(String.Format("{0} gerado ThroughputReject", team), "GerarThroughputReject");
                    }
                    else
                    {
                        Debugar.WriteLine(String.Format("{0} Não gerado", team), "GerarThroughputReject");
                    }
                }
                catch (Exception ex)
                {
                    Debugar.WriteLine(String.Format("Error.. Time: {0} Error: {1} Inner:{2}", time, ex.Message, ex.InnerException.Message), "GerarThroughputReject");
                }
            });
            Debugar.WriteLine("Finalizado ... ", "GerarThroughputReject");

            return true;
        }

        public string GetChamado(string id)
        {
            try
            {
                var parameters = new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.JOURNALS } };
                var issue = _manager.GetObject<Issue>(id, parameters);

                //List<Periodo> lista = new List<Periodo>();

                ////5 dias e 3h
                //lista.Add(new Periodo { DataInicial = Convert.ToDateTime("01/09/2018 11:34:10"), DataFinal = Convert.ToDateTime("06/09/2018 14:38:05") });
                ////2 dias e 6h
                //lista.Add(new Periodo { DataInicial = Convert.ToDateTime("01/09/2018 10:00:00"), DataFinal = Convert.ToDateTime("03/09/2018 16:00:00") });

                return string.Format("#{0}: {1} - {2}: {3}", issue.Id, issue.Subject, issue.Project.Id, issue.Project.Name);
            }
            catch (Exception ex)
            {
                return string.Format("[ERRO] {0}: {1}", id, ex.Message);
            }
        }

        public IEnumerable<string> GetChamadosProjeto(string projeto)
        {
            List<string> result = new List<string>();

            var parameters = new NameValueCollection { { RedmineKeys.PROJECT_ID, projeto } };
            //parameters.Add(RedmineKeys.INCLUDE, RedmineKeys.JOURNALS);

            var issues = _manager.GetObjects<Issue>(parameters);
            issues.ForEach(x => result.Add(string.Format("#{0}: {1}", x.Id, x.Subject)));

            return result;
        }

        public TimeSpan? LeadTime()
        {
            var parameters = new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.JOURNALS } };

            parameters.Add(RedmineKeys.ISSUE_ID, "100150");
            parameters.Add(RedmineKeys.ISSUE_ID, "100182");

            List<Issue> issues = _manager.GetObjects<Issue>(parameters);
            List<Periodo> issuesPeriodo = new List<Periodo>();

            TimeSpan? leadTime = null;

            foreach (var issue in issues)
            {
                DateTime? dataAbertura = issue.Journals
                    .FirstOrDefault(x => x.Details.Any(y => y.Name == "status_id" && y.NewValue == StatusRedmine.Aberto.ToString() || y.NewValue == StatusRedmine.BacklogAberto.ToString()))?
                    .CreatedOn;

                DateTime? dataResolvido = issue.Journals
                    .FirstOrDefault(x => x.Details.Any(y => y.Name == "status_id" && y.NewValue == StatusRedmine.Fechado.ToString()))?
                    .CreatedOn;

                if (dataAbertura != null && dataResolvido != null)
                {
                    issuesPeriodo.Add(new Periodo { DataInicial = dataAbertura.Value, DataFinal = dataResolvido });
                }
            }

            leadTime = issuesPeriodo.TempoMedio();

            return leadTime;
        }

        public List<T> PegarObjeto<T>(string id) where T : class, new()
        {
            return _manager.GetObjects<T>(id);
        }

        public SlaControl GetSla(Projeto time, SlaProjeto slaProjeto, NameValueCollection parametros)
        {
            var retorno = new SlaControl();
            var agora = DateTime.Now;
            var mensagem = "Dados: DateDe:[{0}] DateDe+horaUtil[{1}] DatePara[{2}] Tempo de Pausa: {3}";

            //if (time.Identifier.Equals("time-ze-murai"))
            //{
            //    var b = 1;
            //}


            var chamados = _manager.GetObjectsWithJournals(parametros);


            foreach (var chamado in chamados)
            {
                var item = new Chamado
                {
                    Project = new Project() { Id = chamado.Project.Id, Identifier = chamado.Project.Name },
                    Status = new IdentifiableName() { Id = chamado.Status.Id, Name = chamado.Status.Name },
                    Id = chamado.Id,
                    CreatedOn = chamado.CreatedOn,
                    Subject = chamado.Subject,
                    Description = chamado.Description

                };


                DateTime? dateDe = GetDateByStatus(chamado, slaProjeto.Sla.StatusDe);

                DateTime? datePara = GetDateByStatus(chamado, slaProjeto.Sla.StatusPara);

                //if (retorno.ChamadoSemafaro.Count() == 16)
                //{
                //    var b = 1;
                //}

                if (dateDe != null)
                {
                    double horaPausa = GetTempoPausaChamado(chamado, (DateTime)dateDe, datePara ?? DateTime.Now);

                    var __dateDe = ((DateTime)dateDe);
                    if (datePara == null)
                    {
                        if (((StatusRedmine)chamado.Status.Id) != StatusRedmine.Cancelado)
                        {
                            item.Under = __dateDe.DiferencaDentroDoPrazo(agora, slaProjeto.TipoContagemTempo, slaProjeto.Tempo + horaPausa, slaProjeto.TempoUltil);
                            item.HourTo = Math.Abs(Double.Parse(__dateDe.AddHoraUtil(slaProjeto.Tempo + horaPausa)
                                            .Subtract(agora)
                                            .TotalHours.ToString("##.###")));
                            item.Mensagem = String.Format(mensagem, __dateDe.ToString("dd/MM/yyyy HH:mm:ss"),
                                __dateDe.AddHoraUtil(slaProjeto.Tempo).ToString("dd/MM/yyyy HH:mm:ss"),
                                agora.ToString("dd/MM/yyyy HH:mm:ss"), horaPausa);
                        }
                        else
                        {
                            item.Under = true;
                            item.Mensagem = "Status:  Cancelado";
                        }
                    }
                    else
                    {
                        var __datePara = (DateTime)datePara;
                        item.Under = __dateDe.DiferencaDentroDoPrazo(__datePara, slaProjeto.TipoContagemTempo, slaProjeto.Tempo + horaPausa, slaProjeto.TempoUltil);
                        item.HourTo = Math.Abs(Double.Parse(__datePara.Subtract(__dateDe).TotalHours.ToString("##.###")));
                        item.Mensagem = String.Format(mensagem, __dateDe.ToString("dd/MM/yyyy HH:mm:ss"), __dateDe.AddHoraUtil(slaProjeto.Tempo).ToString("dd/MM/yyyy HH:mm:ss"), __datePara.ToString("dd/MM/yyyy HH:mm:ss"), horaPausa);
                    }

                }
                else
                {
                    item.Mensagem = String.Format(Constantes.Mensagens.NOT_FOUND, "Data para o status " + slaProjeto.Sla.StatusDe.ToString());
                }

                retorno.ChamadoSemafaro.Add(item);
            }

            retorno.Projeto = time;
            retorno.Sla = slaProjeto.Sla;

            return retorno;
        }

        public PrazoControl GetPrazo(NameValueCollection parms, bool efetiva, StatusRedmine status)
        {
            var retorno = new PrazoControl();
            var agora = DateTime.Now;
            var mensagem = "Dados: data[{0}] data2[{1}]";
            var chamados = _manager.GetObjectsWithJournals(parms);



            foreach (var chamado in chamados)
            {
                var item = new Chamado
                {
                    Project = new Project() { Id = chamado.Project.Id, Identifier = chamado.Project.Name },
                    Status = new IdentifiableName() { Id = chamado.Status.Id, Name = chamado.Status.Name },
                    Id = chamado.Id,
                    CreatedOn = chamado.CreatedOn,
                    Subject = chamado.Subject,
                    Description = chamado.Description,
                    FixedVersion = chamado.FixedVersion


                };

                DateTime? dateDe = null;
                DateTime? datePara = null;

                if (chamado.FixedVersion != null)
                {
                    var parmVersion = new NameValueCollection() { { RedmineKeys.PROJECT_ID, chamado.Project.Id.ToString() } };
                    var version = _manager.GetObject<Redmine.Net.Api.Types.Version>(chamado.FixedVersion.Id.ToString(), parmVersion);
                    item.Versao = new Versao()
                    {
                        Id = version.Id,
                        Name = version.Name,
                        DataCriacao = (DateTime)version.CreatedOn,
                        DataPrevista = version.DueDate ?? new DateTime(1900, 1, 1)

                    };
                    dateDe = GetDateByStatus(chamado, status);
                    datePara = version.DueDate;

                    if (datePara != null)
                    {
                        var dateParaC = (DateTime)datePara;
                        var datedeC = dateDe == null ? agora.Date : (DateTime)(dateDe);
                        item.Under = (datedeC <= dateParaC);
                        item.HourTo = dateParaC.Subtract(datedeC).TotalHours;
                        item.Mensagem = dateDe == null
                            ? "Não Encotrada Data para o Status:  " + status.GetDisplayName()
                            : "";

                    }
                    else
                    {
                        item.Mensagem = "Versão Sem data Prevista";
                    }
                }
                else
                {
                    item.Mensagem = "Chamado Sem versão Vinculada";
                }

                if (!efetiva)
                {
                    retorno.ChamadoSemafaro.Add(item);
                }
                else if (dateDe != null)
                {
                    retorno.ChamadoSemafaro.Add(item);
                }
            }

            return retorno;
        }

        private double GetTempoPausaChamado(Issue chamado, DateTime dateRef, DateTime dateLimite)
        {
            double retorno = 0;

            try
            {


                //Pegar Jornals Menor que a data limite e que contenha a tag Aguardando Informação
                var jornals = chamado.Journals.Where(x => x.CreatedOn >= dateRef && x.CreatedOn <= dateLimite && x.Details.Any(y => y.Name == ConstantesCC.Details.Name_Tag
                                                                      && (y.NewValue.Contains(ConstantesCC.Details.Value_AguardandoInformacao) ||
                                                                          y.OldValue.Contains(ConstantesCC.Details.Value_AguardandoInformacao)))
                                                      )
                                              .Select(x => x).ToList();

                if (jornals.Any())
                {
                    //Percorcer os jornal que add A Tag Aguardando Informação
                    jornals.Where(x => x.Details.Any(y =>
                                    y.Name == ConstantesCC.Details.Name_Tag &&
                                    (y.NewValue.Contains(ConstantesCC.Details.Value_AguardandoInformacao) &&
                                     !y.OldValue.Contains(ConstantesCC.Details.Value_AguardandoInformacao))))
                        .OrderBy(x => x.CreatedOn)
                        .ToList()
                        .ForEach((jornal) =>
                        {
                            var firstOrDefault =
                                jornals.FirstOrDefault(x => x.CreatedOn > jornal.CreatedOn &&
                                        x.Details.Any(
                                            y => y.Name == ConstantesCC.Details.Name_Tag &&
                                                (!y.NewValue.Contains(ConstantesCC.Details.Value_AguardandoInformacao) &&
                                                y.OldValue.Contains(ConstantesCC.Details.Value_AguardandoInformacao))));
                            DateTime dateTagOf = firstOrDefault?.CreatedOn ?? DateTime.Now;

                            if (jornal.CreatedOn != null)
                                retorno += ((DateTime)jornal.CreatedOn).CountDifHorasUteis(dateTagOf);

                        });
                }

            }
            catch (Exception ex)
            {
                Debugar.WriteLine(ex.Message);
                throw;
            }

            return retorno;
        }

        public DateTime? GetDateByStatus(Issue chamado, StatusRedmine status, bool oldValue = false)
        {
            return RedmineUtil.GetDateByStatus(chamado, status, oldValue);
        }

        public LeadTimeControl GetCicleTimeTeam(NameValueCollection parms, StatusRedmine statusDe, StatusRedmine statusPara, int primeiroSalto = 5, int quantidadeSaltos = 10, bool leadTime = false)
        {

            var db = new DashBoardAgil.InfraData.Contexto.Contexto(false);

            //var processo = db.Processos
            //                 .Include(x => x.PFS)
            //                 .Include(x => x.PFS.Select(y=>y.Fase))
            //                 .Include(x => x.PFS.Select(y => y.SubFase))
            //                 .FirstOrDefault();

            //processo.PFS.ForEach(x =>
            //{
            //    x.Processo = null;
            //    x.SubFase.PFS = null;
            //    x.Fase.PFS = null;
            //});


            // var conteudo = JsonConvert.SerializeObject(processo);



            //Global.Instance.GravarArquivo(System.AppDomain.CurrentDomain.BaseDirectory, conteudo, "ProcessoBanco.json");

            var chamados = leadTime ? _manager.GetObjects<Issue>(parms) : _manager.GetObjectsWithJournals(parms);
            var leadsTimes = new List<LeadTime>();
            chamados.ForEach(chamado =>
            {
                var dateStatusDe = leadTime ? chamado.CreatedOn : GetDateByStatus(chamado, statusDe);
                var dateStatusPara = leadTime ? chamado.ClosedOn : GetDateByStatus(chamado, statusPara);

                // dateStatusPara = dateStatusPara?? GetDateByStatus(chamado, StatusRedmine.ProducaoAvaliar);

                if (dateStatusDe != null)
                {
                    if (dateStatusPara != null)
                    {
                        var lead = new LeadTime()
                        {
                            IdChamado = chamado.Id,
                            StatusDe = statusDe,
                            StatusPara = statusPara,
                            Tipo = (TipoChamadoRedmine)chamado.Tracker.Id,
                            Prioridade = (PrioridadeRedmine)chamado.Priority.Id,
                            LeadUtil = Math.Round(RedmineUtil.CountDifHorasUteis((DateTime)dateStatusDe, (DateTime)dateStatusPara), 2),
                            LeadBruto = Math.Round(((DateTime)dateStatusPara).Subtract((DateTime)dateStatusDe).TotalHours, 2),
                            //LeadDiaUtil = ((DateTime)dateStatusPara).Diferenca((DateTime)dateStatusDe, TipoContagem.Dia, true),
                            Projeto = chamado.Project.Name,
                            CreatedOn = (DateTime)chamado.CreatedOn,
                            // ClosedOn = (DateTime) chamado.ClosedOn,
                            Versao = CrossCutting.Util.ReflectObjec(chamado.FixedVersion, new IdentificableName()),
                            Status = CrossCutting.Util.ReflectObjec(chamado.Status, new IdentificableName())

                            //Tempo = (int)((DateTime)dateStatusDe).Diferenca((DateTime)dateStatusPara, TipoContagem.Hora, true)  
                        };
                        if (chamado.ClosedOn != null) lead.ClosedOn = (DateTime)chamado.ClosedOn;

                        leadsTimes.Add(lead);
                    }
                }


            });

            var leadsControl = new LeadTimeControl(primeiroSalto, quantidadeSaltos);
            leadsControl.LeadTimes = leadsTimes.ToList();
            leadsControl.UniversoChamados = chamados;
            //var max = LeadsTimes.Max(x=>x.LeadBrutoDays);

            return leadsControl;
        }

        public RejectionControl GetRejectionRate(NameValueCollection parms)
        {
            var chamados = _manager.GetObjectsWithJournals(parms);
            var rejecControl = new RejectionControl();

            rejecControl.Rejections = RedmineUtil.GetRejectionsList(chamados);

            return rejecControl;
        }

        public ThroughputControl GetThroughputRate(NameValueCollection parms)
        {
            var chamados = _manager.GetObjects<Issue>(parms);

            var throughputControl = new ThroughputControl();

            chamados.ForEach(x =>
            throughputControl.Throughputs.Add(new Throughput()
            {
                IdChamado = x.Id,
                ClosedOn = (DateTime)x.ClosedOn,
                CreatedOn = (DateTime)x.CreatedOn,
                Prioridade = (PrioridadeRedmine)x.Priority.Id,
                Projeto = x.Project.Name,
                Tipo = (TipoChamadoRedmine)x.Tracker.Id,

            }));
            return throughputControl;
        }

        public object GetComparativeObject(NameValueCollection parms)
        {
            var chamados = _manager.GetObjectsWithJournals(parms);
            var listFechadoCancelado = new List<int> { StatusRedmine.Cancelado.GetHashCode(), StatusRedmine.Fechado.GetHashCode() };

            var PendenteClienteLista = chamados.Where(Constantes.ChamadoPendenteCliente).Select(x => new { x.Id, x.Status });
            var PendentesInternoLista = chamados.Where(Constantes.ChamadoPendenteInterno).Select(x => new { x.Id, x.Status });
            var Chamados = chamados.Select(x => new { x.Id, x.Status });

            var Rejeitados = chamados.Where(Constantes.ChamadosRejeitados).Where(x => !listFechadoCancelado.Contains(x.Status.Id)).Select(x => new { x.Id, x.Status });
            var FechadosCancelados = chamados.Where(x => listFechadoCancelado.Contains(x.Status.Id));
            var aguardandoInformacao = chamados.Where(Constantes.ChamadosEmAguardandoInformacao).Where(x => !listFechadoCancelado.Contains(x.Status.Id)).Select(x => new { x.Id, x.Status });

            var PendenteClienteListaCompleto = PendenteClienteLista.Concat(aguardandoInformacao).Except(Rejeitados);
            var PendentesInternoListaCompleto = PendentesInternoLista.Concat(Rejeitados).Except(aguardandoInformacao);

            var ret = new
            {
                CriadosCount = Chamados.Count(),
                PendenteInternoCount = PendentesInternoListaCompleto.Count(),
                PendenteClienteCount = PendenteClienteListaCompleto.Count(),
                AguardandoInformacaoCount = aguardandoInformacao.Count(),
                FechadosCanceladosCount = FechadosCancelados.Count(),
                RejeitadosCount = Rejeitados.Count(),
                Criados = Chamados,
                PendenteInterno = PendentesInternoListaCompleto.ToList(), // pendenteInterno - aguardandoInformacao + rejeitados,
                PendenteCliente = PendenteClienteListaCompleto.ToList(),// pendenteCliente - rejeitados + aguardandoInformacao,
                AguardandoInformacao = aguardandoInformacao,
                Rejeitados = Rejeitados,
                FechadosCancelados = FechadosCancelados,

            };

            return ret;
        }

        public object GetCumulativeFlowDiagram(NameValueCollection parms, List<int> weekList)
        {
            var chamados = _manager.GetObjectsWithJournals(parms);

            // Debugar.WriteLine(string.Format("Fim Redmine: {0}", DateTime.Now.ToString("G")));

            var retorno = new List<object>();
            var serie = new List<object>();
            var serieFase = new List<object>();

            // var processo = JsonConvert.DeserializeObject<Processo>(Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "processo.json")));

            var contexto = new DashBoardAgil.InfraData.Contexto.Contexto();

            var processo = contexto.Processos.Include(y => y.PFS).FirstOrDefault(x => x.ProcessoId == 1);

            #region Fluxo para FASE

            var fases = processo.PFS.OrderBy(x => x.Ordem).Select(x => x.Fase).Distinct().ToList();
            fases.ForEach((fase) =>
            {
                //var listFluxo = new List<object>();
                var listSerie = new List<int>();

                var statusFaseRedmine = processo.PFS.Where(x => x.FaseId == fase.FaseId).Select(x => new { Situacao = x.Situacao, SubFase = x.SubFase, StatusRedmine = x.StatusRedmine }).ToList();

                var count = new List<int>();
                weekList.ForEach(semana =>
                  {
                      statusFaseRedmine.ForEach(subFase =>
                      {

                          count.AddRange(chamados.Where(ch => (subFase.Situacao == SituacaoSubFase.Abertura && ((DateTime)ch.CreatedOn).GetWeekOfYear() <= semana) ||
                                                              ch.Journals.Any(y => ((DateTime)y.CreatedOn).GetWeekOfYear() <= semana
                                                              && y.Details.Any(det => det.Property.Equals("attr")
                                                              && det.Name.Equals(RedmineKeys.STATUS_ID)
                                                              && subFase.StatusRedmine.GetHashCode().ToString().Contains(det.NewValue))))
                                              .Select(x => x.Id).ToList());
                          // listFluxo.Add(new { Semana = semana, Quantidade = count });

                      });
                      listSerie.Add(count.Distinct().Count());
                  });

                //retorno.Add(new { Fluxo = flux.Descricao, Dados = listFluxo });
                serieFase.Add(new { name = fase.Descricao, data = listSerie });
            });
            #endregion

            #region Fluxo para Status
            processo.PFS.OrderBy(x => x.Ordem).ToList().ForEach((pfs) =>
                  {
                      //var listFluxo = new List<object>();
                      var listSerie = new List<int>();

                      weekList.ForEach(semana =>
                      {
                          var count = chamados
                                      .Count(ch => (pfs.Situacao == SituacaoSubFase.Abertura && ((DateTime)ch.CreatedOn).GetWeekOfYear() <= semana) ||
                                                   (pfs.Situacao == SituacaoSubFase.Abertura && ch.ClosedOn != null && ((DateTime)ch.ClosedOn).GetWeekOfYear() <= semana) ||
                                                   ch.Journals.Any(y => ((DateTime)y.CreatedOn).GetWeekOfYear() <= semana
                                                             && y.Details.Any(det => det.Property.Equals("attr")
                                                                             && det.Name.Equals(RedmineKeys.STATUS_ID)
                                                                             && pfs.StatusRedmine.GetHashCode().ToString().Equals(det.NewValue))));
                          //listFluxo.Add(new { Semana = semana, Quantidade = count });
                          listSerie.Add(count);
                      });

                      //retorno.Add(new { Fluxo = flux.Descricao, Dados = listFluxo });
                      serie.Add(new { name = pfs.SubFase.Descricao, data = listSerie });

                  });
            #endregion


            return new { Semanas = weekList, Serie = serie, SerieFase = serieFase };
        }

        public ConsumoHorasControl ConsumptionHoursByProject(NameValueCollection parms, string project, string time)
        {
            var retorno = new ConsumoHorasControl();
            var paramProj = new NameValueCollection { { RedmineKeys.ID, "todos" } };
            var projetcosRedmine = _manager.GetObjects<Project>(paramProj);
            var projetoHoraContradata = projetcosRedmine.Where(x => (x.Parent != null || GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Any(y => y.Id.Equals(x.Id.ToString()))) && (x.CustomFields != null && x.CustomFields.Any(y => y.Id == 143 && y.Values != null && y.Values.Any(z => z.Info != ""))));


            if (time != null && !time.Equals(""))
            {

                GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Where(x => time.Equals("0") || x.Identifier.Equals(time)).ToList().ForEach(_time =>
                 {
                     var listProjTime = projetoHoraContradata.Where(proj => (project == null || project.Equals("0") || project.Equals("") || proj.Id.ToString().Equals(project) || proj.Identifier.Equals(project)) && (proj.Id.ToString().Equals(_time.Id.ToString()) || (proj.Parent?.Id.ToString().Equals(_time.Id) ?? false))).Select(
                         pro =>
                             new Projeto()
                             {
                                 Id = pro.Id.ToString(),
                                 Identifier = pro.Identifier,
                                 HorasContratadas = int.Parse(pro.CustomFields.FirstOrDefault(cus => cus.Id == 143)?.Values.FirstOrDefault()?.Info ?? "0")
                             }
                         );
                     retorno.Projetos.AddRange(listProjTime.ToList());

                     var lst = GetListAlocacoes(parms, _time, listProjTime.ToList());
                     lst.ForEach(x => x.Team = new IdentificableName() { Id = int.Parse(_time.Id ?? "0"), Name = _time.Identifier });
                     retorno.HorasAlocadas.AddRange(lst);
                 });
            }

            return retorno;
        }

        private List<ConsumoHoras> GetListAlocacoes(NameValueCollection _parametros, Projeto projeto, List<Projeto> listProjetos = null)
        {
            var lancamentos = new List<TimeEntry>();

            if (listProjetos != null && listProjetos.Any())
            {

                listProjetos.ForEach(pr =>
                {
                    var parm = new NameValueCollection(_parametros) { { RedmineKeys.PROJECT_ID, pr.Id } };
                    lancamentos.AddRange(_manager.GetObjects<TimeEntry>(parm));
                });
            }
            else
            {
                var parm = new NameValueCollection(_parametros) { { RedmineKeys.PROJECT_ID, projeto.Id } };
                lancamentos.AddRange(_manager.GetObjects<TimeEntry>(parm));
            }

            List<ConsumoHoras> ConsumoH = new List<ConsumoHoras>(lancamentos
                .Select(x => new ConsumoHoras()
                {
                    Id = x.Id,
                    Chamado = Global.ReflectObjec(x.Issue, new IdentificableName()),
                    Projeto = Global.ReflectObjec(x.Project, new IdentificableName()),
                    // Team = new IdentificableName() {Id = int.Parse(team.Parent.Id??"0"), Name = team.Parent.Identifier},
                    Usuario = Global.ReflectObjec(x.User, new IdentificableName()),
                    Comentario = x.Comments,
                    Horas = (double)x.Hours,
                    DataAlocacao = (DateTime)x.SpentOn,
                    DataCriacao = (DateTime)x.CreatedOn
                }));

            if (listProjetos != null)
                ConsumoH.AddRange(listProjetos.Where(x => !ConsumoH.Any(y => y.Projeto.Id.ToString().Equals(x.Id))).ToList().Select(item =>
                        new ConsumoHoras()
                        {
                            Id = 0,
                            Chamado = new IdentificableName() { Id = 0, Name = "Sem Chamado" },
                            Projeto = new IdentificableName() { Id = int.Parse(item.Id), Name = item.Identifier },
                            // Team = new IdentificableName() {Id = int.Parse(team.Parent.Id??"0"), Name = team.Parent.Identifier},
                            Usuario = new IdentificableName() { Id = 0, Name = "Sem Usuário" },
                            Comentario = string.Empty,
                            Horas = 0,
                            DataAlocacao = DateTime.Now.Date,
                            DataCriacao = DateTime.Now.Date,
                        }));
            return ConsumoH;
        }

        public DiaDevControl GetDiaDevControl(NameValueCollection parms, Projeto projeto, NameValueCollection parmIssues, DateTime dataIni, DateTime dataFim)
        {

            var db = new DashBoardAgil.InfraData.Contexto.Contexto(false);

            var processo = db.Processos
                             .Include(x => x.PFS)
                             .Include(x => x.PFS.Select(y => y.Fase))
                             .Include(x => x.PFS.Select(y => y.SubFase))
                             .FirstOrDefault();

            processo.PFS.ForEach(x =>
            {
                x.Processo = null;
                x.SubFase.PFS = null;
                x.Fase.PFS = null;
            });

            projeto.Processo = processo;

            var retorno = new DiaDevControl();

            var lancamentos = _manager.GetObjects<TimeEntry>(parms);

            var chas = _manager.GetObjectsWithJournals(parmIssues);
            var chamados = chas.Select(x => Global.ReflectObjec(x, new Chamado())).ToList();


            var listAlocaco = lancamentos.Select(x => Global.ReflectObjec(x, new Alocacao(projeto))).ToList();
            retorno = new DiaDevControl(chamados, dataIni, dataFim) { Lancamentos = listAlocaco, Time = projeto };

            return retorno;
        }

        public EntregasControl GetEntregasPeriodo(NameValueCollection parms, Projeto projeto, DateTime dataIni, DateTime dataFim)
        {

            var db = new DashBoardAgil.InfraData.Contexto.Contexto(false);

            var processo = db.Processos
                             .Include(x => x.PFS)
                             .Include(x => x.PFS.Select(y => y.Fase))
                             .Include(x => x.PFS.Select(y => y.SubFase))
                             .FirstOrDefault();

            processo.PFS.ForEach(x =>
            {
                x.Processo = null;
                x.SubFase.PFS = null;
                x.Fase.PFS = null;
            });

            projeto.Processo = processo;


            var chas = _manager.GetObjectsWithJournals(parms);
            var chamados = chas.Select(x => Global.ReflectObjec(x, new Chamado())).ToList();
            var rejections = RedmineUtil.GetRejectionsList(chas);
            var retorno = new EntregasControl(chamados, rejections, projeto, dataIni, dataFim);

            return retorno;
        }

        public CreatedClosedControl GetCreatedClosed(NameValueCollection parms, DateTime DataIni, DateTime DataFim)
        {

            var parmCreated = new NameValueCollection(parms)
            {
                {RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", DataIni.ToString("yyyy-MM-dd"),DataFim.ToString("yyyy-MM-dd"))},
                {RedmineKeys.STATUS_ID, "*"}
            };
            var parmClosed = new NameValueCollection(parms)
            {
                {RedmineKeys.CLOSED_ON, String.Format("><{0}|{1}", DataIni.ToString("yyyy-MM-dd"),DataFim.ToString("yyyy-MM-dd"))},
                {RedmineKeys.STATUS_ID, "closed"}
            };


            var created = _manager.GetObjects<Issue>(parmCreated);
            var ListCreated = created.Select(x => new CreatedClosed()
            {
                ClosedOn = x.ClosedOn ?? null,
                CreatedOn = (DateTime)x.CreatedOn,
                Flag = Flag.Created,
                IdChamado = x.Id,
                Prioridade = (PrioridadeRedmine)x.Priority.Id,
                Projeto = x.Project.Name,
                Status = new IdentificableName() { Id = x.Status.Id, Name = x.Status.Name },
                Tipo = (TipoChamadoRedmine)x.Tracker.Id,
                Versao = new IdentificableName() { Id = x.FixedVersion?.Id ?? 0, Name = x.FixedVersion?.Name ?? "" }
            }).ToList();
            var closed = _manager.GetObjects<Issue>(parmClosed);
            var ListClosed = closed.Select(x => new CreatedClosed()
            {
                ClosedOn = x.ClosedOn ?? null,
                CreatedOn = (DateTime)x.CreatedOn,
                Flag = Flag.Closed,
                IdChamado = x.Id,
                Prioridade = (PrioridadeRedmine)x.Priority.Id,
                Projeto = x.Project.Name,
                Status = new IdentificableName() { Id = x.Status.Id, Name = x.Status.Name },
                Tipo = (TipoChamadoRedmine)x.Tracker.Id,
                Versao = new IdentificableName() { Id = x.FixedVersion?.Id ?? 0, Name = x.FixedVersion?.Name ?? "" }
            }).ToList();

            var retorno = new CreatedClosedControl(ListCreated, ListClosed);
            return retorno;
        }
    }
}
