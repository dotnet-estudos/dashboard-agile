using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services.Util;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class ChamadoController : BaseApiController
    {
        // GET: api/Chamado
        public Issue Get(int id)
        {
            return _Redmine.GetObject<Issue>(id.ToString(), new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.JOURNALS } });
        }

        // GET: api/Chamado/5
        public List<Issue> Get(string projectid)
        {
            
            List<Issue> lista = new List<Issue>();
            var paramProj = new NameValueCollection {{RedmineKeys.PROJECT_ID, projectid},{ RedmineKeys.STATUS_ID, "6|5" }};//, { RedmineKeys.LIMIT, "1000" } };

            try
            {
                lista = _Redmine.GetObjects<Issue>(paramProj);

            }
            catch (Exception ex)
            {

                throw;
            }

            var retorno = lista.Select(x => x.Id).Select(id => _Redmine.GetObject<Issue>(id.ToString(), new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.JOURNALS } })).ToList();

            return retorno;
        }

        /// <summary>
        /// To fetch issues for a date range (uncrypted filter is "><2012-03-01|2012-03-07") :
        ///        GET /issues.xml? created_on =% 3E % 3C2012-03-01|2012-03-07
        ///
        ///To fetch issues created after a certain date(uncrypted filter is ">=2012-03-01") :
        ///GET /issues.xml? created_on =% 3E % 3D2012-03-01
        ///
        ///Or before a certain date(uncrypted filter is "<= 2012-03-07") :
        /// GET /issues.xml? created_on =% 3C%3D2012-03-07
        ///
        ///To fetch issues created after a certain timestamp(uncrypted filter is ">=2014-01-02T08:12:32Z") :
        ///GET /issues.xml? created_on =% 3E % 3D2014-01-02T08:12:32Z
        ///
        ///     To fetch issues updated after a certain timestamp(uncrypted filter is ">=2014-01-02T08:12:32Z") :
        ///GET /issues.xml? updated_on =% 3E % 3D2014-01-02T08:12:32Z
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataIni"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
       
        public List<SumaryChamdo> GetSumaryChamdos(string time= null)
        {
            var parametros = new NameValueCollection
            {
                {RedmineKeys.PROJECT_ID, time??"csol-1"}, {RedmineKeys.STATUS_ID, "O" }
            };

            var chamados = _Redmine.GetObjects<Issue>(parametros);

            var sumary = new List<SumaryChamdo>();

            sumary.Add( new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fa fa-tasks",
                PaneColor = "primary",
                Fase = "BackLog",
                Quantidade = chamados.Count(Constantes.ChamadoFaseBackLog),
                Mensagem = String.Format("Chamado(s) em {0}","BackLog"),
                UrlRedmine = ""

            }
            );

            sumary.Add(new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fab fa-dev",
                PaneColor = "Sucess",
                Fase = "Desenvolvimento",
                Quantidade = chamados.Count(Constantes.ChamadoFaseDesenvolvimento),
                Mensagem = String.Format("Chamado(s) em {0}", "Desenvolvimento")

            }
           );

            sumary.Add(new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fas fa-clipboard-list",
                PaneColor = "yellow",
                Fase = "Homologação",
                Quantidade = chamados.Count(Constantes.ChamadoFaseHomologacao),
                Mensagem = String.Format("Chamado(s) em {0}", "Homologação")

            }
           );

            sumary.Add(new SumaryChamdo
            {
                Ordem = 1,
                FaIcon = "fab fa-creative-commons-pd-alt",
                PaneColor = "info",
                Fase = "Produção",
                Quantidade = chamados.Count(Constantes.ChamadoFaseProducao),
                Mensagem = String.Format("Chamado(s) em {0}", "Produção")

            }
           );

            return sumary;

        }
        // POST: api/Chamado
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Chamado/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Chamado/5
        public void Delete(int id)
        {
        }
    }
}
