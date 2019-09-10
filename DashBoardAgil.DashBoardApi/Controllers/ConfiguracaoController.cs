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


        
        public ActionResult UsuarioMasterCreate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioMasterCreate(Usuario user)
        {
            GlobalConfig.Instance.CarregarConfig();
            if (!user.Login.IsNullOrWhiteSpace() && !GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster.Any(x => x.Equals(user.Login.Trim())))
            {
                GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster.Add(user.Login.Trim());
                GlobalConfig.Instance.GravarConfig();
            }

            GlobalConfig.Instance.CarregarConfig();
            ViewBag.usuariosMaster = GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster;

            return View("UsuariosMaster").Mensagem("Usuario Adicionado com Sucesso!");
        }

        
        public ActionResult UsuarioMasterDelete(string user)
        {
            var _user = new Usuario() {Login = user};
            return View(_user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioMasterDelete(string user, string delete)
        {
            GlobalConfig.Instance.CarregarConfig();
            if (!user.IsNullOrWhiteSpace() && GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster.Any(x => !x.Equals(user.Trim())))
            {
                var del  = GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster.FirstOrDefault(x=>x.Equals(user.Trim()));
                GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster.Remove(del);
                GlobalConfig.Instance.GravarConfig();
            }

            return RedirectToAction("UsuariosMaster").Mensagem("Usuario Removido com Sucesso!");
        }

        public ActionResult PerfilCliente()
        {
            GlobalConfig.Instance.CarregarConfig();
            ViewBag.perfisCliente = GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente;
            return View();
        }



        public ActionResult PerfilClienteCreate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PerfilClienteCreate(Usuario user)
        {
            GlobalConfig.Instance.CarregarConfig();
            if (!user.Login.IsNullOrWhiteSpace() && !GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente.Any(x => x.Equals(user.Login.Trim())))
            {
                GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente.Add(user.Login.Trim());
                GlobalConfig.Instance.GravarConfig();
            }

            GlobalConfig.Instance.CarregarConfig();
            ViewBag.perfisCliente = GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente;

            return View("PerfilCliente").Mensagem("Perfil Adicionado com Sucesso!");
        }


        public ActionResult PerfilClienteDelete(string user)
        {
            var _user = new Usuario() { Login = user };
            return View(_user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PerfilClienteDelete(string user, string delete)
        {
            GlobalConfig.Instance.CarregarConfig();
            if (!user.IsNullOrWhiteSpace() && GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente.Any(x => !x.Equals(user.Trim())))
            {
                var del = GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente.FirstOrDefault(x => x.Equals(user.Trim()));
                GlobalConfig.Instance.ConfiguracaoRedmine.PerfilCliente.Remove(del);
                GlobalConfig.Instance.GravarConfig();
            }

            return RedirectToAction("PerfilCliente").Mensagem("Perfil Removido com Sucesso!");
        }
    }
}