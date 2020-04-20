using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.CrossCutting.Enums;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class PrazoChamadoController : BaseApiController
    {
        NameValueCollection _nameValueDefault = new NameValueCollection() { { RedmineKeys.INCLUDE, RedmineKeys.VERSION } };
        public Object Get(string time, DateTime dataIni, DateTime dataFim, bool efetiva, StatusRedmine status)
        {
            var filtro = new { time, dataIni, dataFim };
            try
            {
                if (time.IsNullOrWhiteSpace()) throw new Exception("Time é Obrigatório");

                var projeto = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.ProjetoId.ToString().Equals(time));
                //.ProjetosAtivos().FirstOrDefault(x => x.ProjetoId.Equals(time)) ?? null;

                _nameValueDefault.Add(RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd")));

                _nameValueDefault.Add(RedmineKeys.PROJECT_ID, projeto.Identifier);
                //verificar os custons fields dos times
                projeto.CustonFields.ForEach(custon => {
                    _nameValueDefault.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
                });
                //Fim verificar os custons fields dos times
                _nameValueDefault.Add(RedmineKeys.STATUS_ID, "*");
                var retorno = _RedmineService.GetPrazo(_nameValueDefault, efetiva, status);

                return new
                {
                    status = true, mensagem = "sucesso", filtro = filtro, PrazoControl = retorno, };
            }
            catch (Exception ex)
            {
                return new { status = false, mensagem = String.Format("{0} {1} {2} {3}", "Erro ao Consultar: ", ex.Message, ex.InnerException?.Message ?? "-", ex.StackTrace), filtro, PrazoControl = new PrazoControl() };
            }
          
        }
    }
}
