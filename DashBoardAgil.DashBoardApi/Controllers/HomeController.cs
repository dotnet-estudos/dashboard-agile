using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;

using DashBoardAgil.Domain.Interfaces;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.Services.Service;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

using DashBoardAgil.Services;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            /*
            Global.Instance.CarregarParametros(ConfigurationManager.AppSettings);

            var resultglobal = Global.Instance.ConsultaCpfLista("84118040549");
            */
            if (Session["Usuario"] != null)
            {
                var mensage = Session["Mensagem"];

                if (mensage != null)
                {
                    var men = (Mensagem)mensage;

                    Session["Mensagem"] = null;

                    if (men.Erro)
                    {
                        return View("Index").MensagemErro(men.Mensage);
                    }
                    else
                    {
                        return View("Index").MensagemSucesso(men.Mensage);
                    }

                }


                ViewBag.User = (Usuario)Session["User"];
                //GetSelectTime("time-traiket", false);
                GetSelectTime("1", false);
                return View("Index");
            }


            return View("Login");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }


        [AllowAnonymous]
        /// <summary>
        /// Processa a autenticação no site.
        /// </summary>
        /// <param name="pLogin"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(Login pLogin)
        {
            FormsAuthentication.Initialize();

            var lUsuario = new Login
            {
                Email = pLogin.Email,
                Senha = pLogin.Senha
            };

            var usuario = LocalAutentication(lUsuario); //UsuariosServico.Autenticar(lUsuario);

            if (usuario == null)
                return RedirectToAction("Login", "Home").MensagemErro("Falha na Autenticação, usuário e/ou senha inválido(s).");

            if (!usuario.Ativo)
                return RedirectToAction("Login", "Home").MensagemErro("Usuário Inativo.");

            AutenticarServico(usuario);
            Session["Usuario"] = usuario;


            if (Request.Cookies["progressCookie"] != null)
            {
                Response.Cookies["progressCookie"].Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                var cookie = new HttpCookie("progressCookie");
                cookie.Domain = Request.Url.Host;
                cookie.Path = "/";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Home");
        }

        private Login LocalAutentication(Login lUsuario)
        {
            RedmineManager _RedmineService = null;

            DashBoardAgil.Services.GlobalConfig.Instance.CarregarConfig();

            _RedmineService = new RedmineManager(
                           DashBoardAgil.Services.GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                           lUsuario.Email,
                           lUsuario.Senha);

            //var parm = new NameValueCollection
            //{
            //    {RedmineKeys.USERS, "current"}
            //};

            try
            {
                // Pegar os perfis por projeto do usuário logado
                NameValueCollection parametros = new NameValueCollection();
                parametros.Add("include", "memberships");

                var user = _RedmineService.GetCurrentUser(parametros);

                if (user == null)
                {
                    return null;
                }
                var usuario = Global.ReflectObjec(user, new Usuario());
                #if DEBUG
                //usuario.Perfil = PerfilDash.Cliente;
                #endif
                Session["User"] = usuario;
                ViewBag.User = usuario;

                GlobalConfig.Instance.CarregarConfig();
                var menu = new Domain.Models.Menu();
                Session["ItensMenu"] = menu.MontaMenuPorPerfil(GlobalConfig.Instance.ConfiguracaoRedmine.ItensMenu, usuario.Perfil);

                if (usuario.Status == UserStatus.STATUS_ACTIVE || usuario.Status == UserStatus.STATUS_ANONYMOUS)
                    lUsuario.Ativo = true;
            }
            catch (Exception)
            {

                return null;
            }



            return lUsuario;
        }

        /// <summary>
        /// Processa o Logout do site.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Método privado para Autenticar o Serviço.
        /// </summary>
        /// <param name="usuario"></param>
        private void AutenticarServico(Login usuario)
        {
            var autentic = new FormsAuthenticationTicket(usuario.Email, false, 120);

            var encryptedTicket = FormsAuthentication.Encrypt(autentic);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            if (autentic.IsPersistent) authCookie.Expires = autentic.Expiration;

            Response.Cookies.Add(authCookie);
            FormsAuthentication.SetAuthCookie(usuario.Email, true);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

