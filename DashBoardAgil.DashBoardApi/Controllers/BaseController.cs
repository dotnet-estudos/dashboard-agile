using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.DashBoardApi.Util;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.Domain.Enums;
using DashBoardAgil.Domain.Interfaces;
using DashBoardAgil.Services.Service;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

        protected RedmineManager _Redmine = null;
        protected IRedmineService _RedmineService = null;
        protected Usuario _user = null;

        public BaseController()
        {
            GlobalConfig.Instance.CarregarConfig();

            //_user = (Usuario)((User) Session["User"]);
           
#if DEBUG
            _Redmine = new Redmine.Net.Api.RedmineManager(
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                           GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User, //Mudar para seu usario para teste
                           CriptografiaAES.Instance.Descriptografar(GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.Pass));

            _RedmineService = new RedmineService(
                          GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.RedmineUrl,
                          GlobalConfig.Instance.ConfiguracaoRedmine.Parametro.User, //Mudar para seu usario para teste
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
        }
        protected SelectList GetSelectTime(string selected = null, bool todos = false)
        {
            try
            {
                var list = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Select(x=>new SelectListItem() { Value= x.Identifier, Text = x.Identifier}).ToList();
                GetSelectedListWithAll(selected, todos, list);
                var select = new SelectList(list.OrderBy(x=>x.Value), "Value",
                          "Text", selected);
                ViewBag.SelectTime = select;
                return select;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<SelectListItem> GetSelectedListWithAll(object selected, bool todos, List<SelectListItem> list)
        {
            if (todos)
            {
                list.Add(new SelectListItem {Value = "0", Text = "Todos", Selected = (selected == null)});
            }

            return list.OrderBy(x => x.Value).ToList();
        }
        protected SelectList GetSelectTipoContagem(TipoContagem? selected = TipoContagem.Hora)
        {
            try
            {
                var tipoContagem = from TipoContagem e in Enum.GetValues(typeof(TipoContagem))
                                   select new
                                   {
                                       Id = e.GetHashCode(),
                                       Nome = e.ToString()
                                   };

                var select = new SelectList(tipoContagem, "Id", "Nome", selected.GetHashCode());
                ViewBag.SelectTipoContagem = select;

                return select;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
        protected SelectList GetSelectSla(string selected = null)
        {
            try
            {
                var strSla = Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "Sla.json"));

                var slas = JsonConvert.DeserializeObject<List<Sla>>(strSla);

                var select = new SelectList(slas, "IdSla", "Identificador", 1);

                ViewBag.SelectSla = new SelectList(select, "IdSla", "Identificador", selected ?? "1");

                return select;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
        protected SelectList GetSelectStatusRedmine(StatusRedmine? selected = null, bool todos = false)
        {
            try
            {                
                var status = new SelectList(GetSelectedListWithAll(selected, todos, Enum.GetValues(typeof(StatusRedmine)).Cast<StatusRedmine>().Select(v => new SelectListItem
                {
                    Text = v.GetDisplayName(),
                    Value = ((int)v).ToString()
                }).ToList()), "Value", "Text", (selected != null ? selected.GetHashCode().ToString() : null));


                ViewBag.SelectStatusRedmine = status;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
        protected SelectList GetSelectPrioridadeRedmine(PrioridadeRedmine? selected = null, bool todos = false)
        {
            try
            {
                var status = new SelectList(GetSelectedListWithAll(selected, todos, Enum.GetValues(typeof(PrioridadeRedmine)).Cast<PrioridadeRedmine>().Select(v => new SelectListItem
                {
                    Text = v.GetDisplayName(),
                    Value = ((int)v).ToString()
                }).ToList()), "Value", "Text", (selected != null ? selected.GetHashCode().ToString() : null));


                ViewBag.SelectPrioridadeRedmine = status;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
        protected SelectList GetSelectTipoChamado(TipoChamadoRedmine? selected = null , bool todos = false)
        {
            try
            {
                var status = new SelectList(GetSelectedListWithAll(selected, todos,Enum.GetValues(typeof(TipoChamadoRedmine)).Cast<TipoChamadoRedmine>().Select(v => new SelectListItem
                {
                    Text = v.GetDisplayName(),
                    Value = ((int)v).ToString()
                }).ToList()), "Value", "Text", (selected != null ? selected.GetHashCode().ToString() : null));
                
                ViewBag.SelectTipoChamado = status;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionUser = (User) Session["User"];
            if (sessionUser != null)
            {
                _user = Global.ReflectObjec(sessionUser, new Usuario());
                ViewBag.AbleToShowConfig = GlobalConfig.Instance.ConfiguracaoRedmine.UsuariosMaster.Contains(_user?.Login ?? "NULL");

            }
            base.OnActionExecuting(filterContext);

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Debugar.WriteLine(String.Format("[Error]:[Controller: {0}][ExceptionType:{1} ][ErrorMessage: {2}][Inner:{3}] ",
                filterContext.Controller?.ToString()??"",
                filterContext.Exception?.ToString()??"",
                filterContext.Exception?.Message ?? "",
                filterContext.Exception?.InnerException?.Message?? ""
                ), "OnException");
            base.OnException(filterContext);
        }
    }
}