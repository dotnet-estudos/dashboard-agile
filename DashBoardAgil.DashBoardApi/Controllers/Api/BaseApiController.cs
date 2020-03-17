using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Results;
using System.Web.Mvc;
using DashBoardAgil.Aplication;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.DashBoardApi.Util;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Interfaces;
using DashBoardAgil.InfraData.Contexto;
using DashBoardAgil.Services.Service;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{

    public class BaseApiController : ApiController
    {

        protected RedmineManager _Redmine = null;
        protected IRedmineService _RedmineService = null;
        protected NameValueCollection _defaultTipoChamados = null;
        protected NameValueCollection _defaultPrioridade { get; set; }
        protected List<string> _lsitPrioridae { get; set; }
        protected Usuario _user = null;

        public BaseApiController()
        {
            GlobalConfig.Instance.CarregarConfig();

#if DEBUG
            _Redmine = new Redmine.Net.Api.RedmineManager(
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User, //mudar para seu usuario caso queria testar
                           CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));

            _RedmineService = new RedmineService(
                          GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                          GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User, //mudar para seu usuario caso queria testar
                          CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));
#else
            _Redmine = new Redmine.Net.Api.RedmineManager(
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User,
                           CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));

             _RedmineService = new RedmineService(
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User,
                           CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));

#endif

            var lsitTipo =   from TipoChamadoRedmine e in Enum.GetValues(typeof(TipoChamadoRedmine))
            select new
            {
                Id = e.GetHashCode(),
                Nome = e.ToString()
            };
            var lststr = lsitTipo.Select(x => x.Id.ToString()).ToList();
            _defaultTipoChamados = new NameValueCollection { {RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)} };

            var lststrPrt = from PrioridadeRedmine e in Enum.GetValues(typeof(PrioridadeRedmine))
                           select new
                           {
                               Id = e.GetHashCode(),
                               Nome = e.ToString()
                           };
            _lsitPrioridae = lststrPrt.Select(x => x.Id.ToString()).ToList();
            _defaultPrioridade = new NameValueCollection { { RedmineKeys.PRIORITY_ID, Global.Instance.ConcatenarLista("|", _lsitPrioridae) } };

        }

        protected List<string> GetStatusFechamento()
        {
            var db = new Contexto();

            var list = db.PFS.Include(x=>x.SubFase).Where(x => x.Situacao == SituacaoSubFase.Fechamento && x.StatusRedmine != 5).Select(x => x.StatusRedmine.ToString()).ToList();

            return list;

        }

        // GET: api/BaseApi
        protected override ExceptionResult InternalServerError(Exception exception)
        {
            Debugar.WriteLine(String.Format("{0} : {1}", exception.Message,
                exception.InnerException?.Message ?? " "));
            return base.InternalServerError(exception);
        }
        
    }
}
