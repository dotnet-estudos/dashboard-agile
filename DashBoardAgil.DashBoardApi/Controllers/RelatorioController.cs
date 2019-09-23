using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.Domain.Enums;
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

            return View();
        }

        public ActionResult ConsumoHorasProjeto()
        {
            ViewBag.title = "Consumo de Horas Projeto";
            GetSelectTime(null, false);
            ViewBag.SelectStatusRedmineDe = GetSelectStatusRedmine(StatusRedmine.BacklogNovo);
            ViewBag.SelectStatusRedminePara = GetSelectStatusRedmine(StatusRedmine.BacklogAberto);
            GetSelectTipoChamado(null, true);
            GetSelectPrioridadeRedmine(null, true);

            return View();
        }

    }
}
