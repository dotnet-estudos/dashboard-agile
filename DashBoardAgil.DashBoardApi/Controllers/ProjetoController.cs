using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services;
using DashBoardAgil.Services.Domain;
using WebGrease.Css.Extensions;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    public class ProjetoController : BaseController
    {
        // GET: Projeto
        public ActionResult Index()
        {
            return View();
        }

        // GET: Projeto/Details/5
        public ActionResult Details(string id, string tipo)
        {
            GlobalConfig.Instance.CarregarConfig();
            return View(GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Concat(GlobalConfig.Instance.ConfiguracaoRedmine.DenyList).ToList().FirstOrDefault(x => x.Id == id));
        }

        // GET: Projeto/Create
        public ActionResult Create(string tipo)
        {
            ViewBag.Tipo = ViewBag.Title = tipo;
            return View();
        }

        // POST: Projeto/Create
        [HttpPost]
        public ActionResult Create(Projeto collection, string Tipo)
        {
            try
            {
                GlobalConfig.Instance.CarregarConfig();
                if (Tipo.Equals("Habilitado"))
                {
                    GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Add(collection);
                }
                else
                {   
                    GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.Add(collection);
                }

                GlobalConfig.Instance.GravarConfig();
                
                return RedirectToAction(String.Format("Projetos{0}s", Tipo), "Configuracao").MensagemSucesso("Salvo Com Sucesso!");
            }
            catch(Exception ex)
            {
                Debugar.WriteLine(string.Format("Error [Create projetc]: Exceptiion: {0}, inner {1}", ex.Message, ex.InnerException?.Message??""));
                return View().MensagemErro("Erro na tentativa de execução"); ;
            }
        }

        // GET: Projeto/Edit/5
        public ActionResult Edit(string id, string tipo)
        {
            GlobalConfig.Instance.CarregarConfig();
            ViewBag.Tipo = ViewBag.Title = tipo;
            return View(GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Concat(GlobalConfig.Instance.ConfiguracaoRedmine.DenyList).ToList().FirstOrDefault(x => x.Id == id));
        }

        // POST: Projeto/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Projeto collection, string Tipo)
        {
            try
            {
                GlobalConfig.Instance.CarregarConfig();
                if (Tipo.Equals("Habilitado"))
                {
                    var edit = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Id == id);
                    if (edit != null)
                    {
                        GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Remove(edit);
                        GlobalConfig.Instance.GravarConfig();

                        edit.Identifier = collection.Identifier;
                        edit.Ativo = collection.Ativo;
                        GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Add(edit);
                    }
                    else
                    {
                        Debugar.WriteLine(String.Format("[Error:] edit {0} não encontrado;", id??"NULL"));
                    }
                }
                else
                {
                    var edit = GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.FirstOrDefault(x => x.Id == id);
                    if (edit != null)
                    {
                        GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.Remove(edit);
                    GlobalConfig.Instance.GravarConfig();
                    edit.Identifier = collection.Identifier;
                    edit.Ativo = collection.Ativo;
                    GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.Add(edit);
                    }
                    else
                    {
                        Debugar.WriteLine(String.Format("[Error:] edit {0} não encontrado;", id ?? "NULL"));
                    }
                }

                GlobalConfig.Instance.GravarConfig();
                //var result = Global.ReflectObjec(collection, edit);

                return RedirectToAction(String.Format("Projetos{0}s", Tipo), "Configuracao").MensagemSucesso("Salvo Com Sucesso!");
            }
            catch
            {
                return View().MensagemErro("Erro na tentativa de execução"); ;
            }
        }

        // GET: Projeto/Delete/5
        public ActionResult Delete(int id, string Tipo)
        {
            ViewBag.Tipo = ViewBag.Title = Tipo;
            return View();
        }

        // POST: Projeto/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Projeto collection, string Tipo)
        {
            try
            {
                GlobalConfig.Instance.CarregarConfig();
                if (Tipo.Equals("Habilitado"))
                {
                    var edit = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Id == id);
                    GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Remove(edit);
                }
                else
                {
                    var edit = GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.FirstOrDefault(x => x.Id == id);
                    GlobalConfig.Instance.ConfiguracaoRedmine.DenyList.Remove(edit);
                }

                GlobalConfig.Instance.GravarConfig();
                return RedirectToAction(String.Format("Projetos{0}s", Tipo), "Configuracao").MensagemSucesso("Excluído Com Sucesso!");
            }
            catch
            {
                return View().MensagemErro("Erro na tentativa de execução");
            }
        }
    }
}
