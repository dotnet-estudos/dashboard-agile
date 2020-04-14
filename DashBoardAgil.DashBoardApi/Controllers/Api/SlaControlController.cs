using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using WebGrease.Css.Extensions;
using WebApi.OutputCache.V2;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class SlaControlController : BaseApiController
    {
        NameValueCollection _defualtParm = new NameValueCollection() { { RedmineKeys.STATUS_ID, "*" } };

        public List<Issue> Get(string id, DateTime dataIni, DateTime dataFim, string status)
        {
           
            var proj =
               GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.ProjetoId.Equals(id));

            var parametros = new NameValueCollection
            {
                {
                    RedmineKeys.CREATED_ON,
                    String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd"))
                },
                {RedmineKeys.PROJECT_ID, proj.Identifier}
            };

            //verificar os custons fields dos times
            proj.CustonFields.ForEach(custon => {
                parametros.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
            });
            //Fim verificar os custons fields dos times

            var chamados = _Redmine.GetObjects<Issue>(parametros);
            // chamados = chamados.Select(x => _Redmine.GetObject<Issue>(x.ProjetoId.ToString(), new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.JOURNALS } })).ToList();

            return chamados;

        }

        // GET: api/SlaControl/5
        public Object Get(string time, int idSla, int tempo, TipoContagem tipoContagem, DateTime dataIni, DateTime dataFim, bool util)
        {
            try
            {
                var projeto = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.ProjetoId.Equals(time));
                //.ProjetosAtivos().FirstOrDefault(x => x.ProjetoId.Equals(time));


                if (projeto.Identifier.IsNullOrWhiteSpace())
                    return new { status = false, mensagem = "Time é Obrigatório", SlaControl = new SlaControl() };

                var parametros = new NameValueCollection
                        {
                            {RedmineKeys.PROJECT_ID, projeto.Identifier},
                            { RedmineKeys.STATUS_ID, "*"},
                            {RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd")) }
                        };
                //verificar os custons fields dos times
                projeto.CustonFields.ForEach(custon => {
                    parametros.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
                });
                //Fim verificar os custons fields dos times
                var chamados = _Redmine.GetObjects<Issue>(parametros);
                chamados = chamados.Select(x => _Redmine.GetObject<Issue>(x.Id.ToString(), new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.JOURNALS } })).ToList();

                var strSla = Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "Sla.json"));

                var slas = JsonConvert.DeserializeObject<List<Sla>>(strSla);

                var slaProjeto = new SlaProjeto() { IdSla = idSla, Tempo = tempo, TempoUltil = util, TipoContagemTempo = tipoContagem, Sla = slas.FirstOrDefault(x => x.IdSla == idSla) };

                var filtro = new { time, idSla, tempo, tipoContagem, dataIni, dataFim, util };

                var slaControl = _RedmineService.GetSla(projeto, slaProjeto, parametros);

                return new { status = true, mensagem = "sucesso", filtro, SlaControl = slaControl };//"value";
            }
            catch (Exception ex)
            {

                return new { status = false, mensagem = String.Format("{0} {1} {2} {3}", "Erro ao Consultar: ", ex.Message, ex.InnerException?.Message ?? "-", ex.StackTrace), SlaControl = new SlaControl() };
            }


        }

       [CacheOutput(ServerTimeSpan = 60*60)]
        public Object Get(string time, StatusRedmine redmineDe, StatusRedmine redminePara, int tempo, TipoContagem tipoContagem, DateTime dataIni, DateTime dataFim, bool util) //SlaProjeto slaProjeto, DateTime dataIni, DateTime dataFim) //
        {
            try
            {

                if (time.IsNullOrWhiteSpace())
                    return new { status = false, mensagem = "Time é Obrigatório", SlaControl = new SlaControl() };

                var retorno = new List<SlaControl>();

                GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Where(x => x.ProjetoId.Equals(time) || time.Equals("0")).ForEach(
                    (team) =>
                    {
                        // var projeto = GlobalConfig.Instance.ConfiguracaoRedmine.ProjetosAtivos().FirstOrDefault(x => x.Identifier.Equals(team.Identifier));
                        var processo = _RedmineService.GetProcessoTeam(team);
                        var lststr = processo.ProcessoTipo.Select(x => x.TipoChamado.IdExterno).ToList();

                        var slaProjeto = new SlaProjeto() { IdSla = 0, Tempo = tempo, TempoUltil = util, TipoContagemTempo = tipoContagem, Sla = new Sla() { StatusDe = redmineDe, StatusPara = redminePara } };

                        var parametro = new NameValueCollection
                            {
                                { RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)},
                                { RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd")) },
                                { RedmineKeys.STATUS_ID, "*" }
                            };
                        //var parametro = new NameValueCollection(_defaultTipoChamados);
                        //verificar os custons fields dos times
                        team.CustonFields.ForEach(custon => {
                            parametro.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
                        });
                        //Fim verificar os custons fields dos times

                        parametro.Add(RedmineKeys.PROJECT_ID, team.Identifier);
                        
                        var slaControl = _RedmineService.GetSla(team, slaProjeto, parametro);

                        retorno.Add(slaControl);
                    });

                var filtro = new { time, redmineDe, redminePara, dataIni, tipoContagem, tempo, dataFim, util };

                return new { status = true, mensagem = "sucesso", filtro, slaControl = retorno };
            }
            catch (Exception ex)
            {

                return new { status = false, mensagem = String.Format("{0} {1} {2} {3}", "Erro ao Consultar: ", ex.Message, ex.InnerException?.Message ?? "-", ex.StackTrace), SlaControl = new SlaControl() };
            }


        }

        // POST: api/SlaControl
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SlaControl/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SlaControl/5
        public void Delete(int id)
        {
        }
    }
}
