using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using Microsoft.Ajax.Utilities;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class LogController : BaseApiController
    {
        public object Get(DateTime logData, bool chamado = false)
        {
            var conteudo = "";
            if (chamado)
            {
                conteudo= Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", "Chamados", "Fechados",
                   String.Format("{0}.txt", logData.ToString("ddMMyyyy"))));
            }
            else
            {
                conteudo= Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp",
                   String.Format("Log_{0}.txt", logData.ToString("ddMMyyyy"))));
            }
            return new {Sucesso = !conteudo.IsNullOrWhiteSpace(), Mensagem = conteudo};
        }
    }
}
