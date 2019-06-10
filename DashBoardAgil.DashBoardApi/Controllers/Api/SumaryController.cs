using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services;
using DashBoardAgil.Services.Util;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using WebApi.OutputCache.V2;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class SumaryController : BaseApiController
    {
        // GET: api/Sumary
        [CacheOutput(ServerTimeSpan = 1*(60*60))]
        public List<SumaryChamdo> Get(string time = null)
        {
            //var parametros = new NameValueCollection
            //{
            //    {RedmineKeys.PROJECT_ID, "csol1"}, {RedmineKeys.STATUS_ID, Global.Instance.ConcatenarLista("|", Constantes.StatusChamadosAbertos ) }
            //};

            var parametros = new NameValueCollection
            {
                {RedmineKeys.PROJECT_ID, time??"csol1"}, {RedmineKeys.STATUS_ID, "open" }
            };

            var chamados = _Redmine.GetObjects<Issue>(parametros);

            var sumary = new List<SumaryChamdo>();

            sumary.Add(new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fa fa-tasks",
                PaneColor = "primary",
                Fase = "BackLog",
                Quantidade = chamados.Count(Constantes.ChamadoFaseBackLog),
                Mensagem = String.Format("em {0}", "BackLog"),
                UrlRedmine = string.Format("{0}/issues/?project_id={1}&status_id={2}",GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl, time ?? "csol1", Global.Instance.ConcatenarLista("|", Constantes.FaseBackLog))  

            }
            );

            sumary.Add(new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fab fa-dev",
                PaneColor = "green",
                Fase = "Desenvolvimento",
                Quantidade = chamados.Count(Constantes.ChamadoFaseDesenvolvimento),
                Mensagem = String.Format("em {0}", "Desenvolvimento"),
                UrlRedmine = string.Format("{0}/issues/?project_id={1}&status_id={2}", GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl, time ?? "csol1", Global.Instance.ConcatenarLista("|", Constantes.FaseDesnvolvimento))


            }
           );

            sumary.Add(new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fas fa-clipboard-list",
                PaneColor = "yellow",
                Fase = "Homologação",
                Quantidade = chamados.Count(Constantes.ChamadoFaseHomologacao),
                Mensagem = String.Format("em {0}", "Homologação"),
                UrlRedmine = string.Format("{0}/issues/?project_id={1}&status_id={2}", GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl, time ?? "csol1", Global.Instance.ConcatenarLista("|", Constantes.FaseHomologacao))

            }
           );

            sumary.Add(new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fab fa-creative-commons-pd-alt",
                PaneColor = "info",
                Fase = "Produção",
                Quantidade = chamados.Count(Constantes.ChamadoFaseProducao),
                Mensagem = String.Format("em {0}", "Produção"),
                UrlRedmine = string.Format("{0}/issues/?project_id={1}&status_id={2}", GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl, time ?? "csol1", Global.Instance.ConcatenarLista("|", Constantes.FaseProducao))

            }
           );

            return sumary;

        }

        // GET: api/Sumary/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sumary
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Sumary/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sumary/5
        public void Delete(int id)
        {
        }
    }
}
