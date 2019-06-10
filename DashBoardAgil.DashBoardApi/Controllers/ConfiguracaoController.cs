using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services;
using DashBoardAgil.Services.Domain;
using Microsoft.Ajax.Utilities;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    public class ConfiguracaoController : BaseController
    {
        // GET: Configuracao
        public ActionResult Edit()
        {
            GlobalConfig.Instance.CarregarConfig();
            return View(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Parametro form)
        {
            GlobalConfig.Instance.CarregarConfig();

            Global.ReflectObjec(form, GlobalConfig.Instance.ConfiguracaoRedmine.Parametro);

            GlobalConfig.Instance.GravarConfig();

            return View(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro);
        }


       
        public ActionResult ProjetosHabilitados()
        {
            GlobalConfig.Instance.CarregarConfig();
            ViewBag.Tipo = ViewBag.Title = "Habilitado";
            return View(GlobalConfig.Instance.ConfiguracaoRedmine.AllowList);
        }

        public ActionResult Log()
        {
            var conteudo = Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", String.Format("Log_{0}.txt", DateTime.Now.Date.ToString("ddMMyyyy"))));
            ViewBag.Title = "Arquivos De Log";
            ViewBag.logData = DateTime.Now.ToString("yyyy-MM-dd");

          
            return View(new Logger() {Log = conteudo}).Mensagem(conteudo.IsNullOrWhiteSpace()?"Arquivo não Encontrado": "");
        }

        public ActionResult ChamadoFechados()
        {
            var conteudo = Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", "Chamados", "Fechados",  String.Format("Log_{0}.txt", DateTime.Now.Date.ToString("ddMMyyyy"))));
            ViewBag.Title = "Arquivos Log De Chamados";
            ViewBag.logData = DateTime.Now.ToString("yyyy-MM-dd");


            return View(new Logger() { Log = conteudo }).Mensagem(conteudo.IsNullOrWhiteSpace() ? "Arquivo não Encontrado para o dia de hoje" : "");
        }

        public ActionResult USer()
        {
            return View(_user);
        }

        public ActionResult ProjetosBloqueados()
        {
            GlobalConfig.Instance.CarregarConfig();
            ViewBag.Tipo = ViewBag.Title = "Bloqueado";
            return View("ProjetosHabilitados", GlobalConfig.Instance.ConfiguracaoRedmine.DenyList);
        }

        public ActionResult UsuariosMaster()
        {
            GlobalConfig.Instance.CarregarConfig();
            ViewBag.usuariosMaster = GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster;
            return View();
        }
    }
}