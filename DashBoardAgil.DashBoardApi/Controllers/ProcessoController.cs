using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.InfraData.Contexto;
using DashBoardAgil.Services.Service;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    public class ProcessoController : Controller
    {
        // GET: Processo
        public ActionResult Index()
        {
            return View(new Contexto().Processos.ToList());
        }

        // GET: Processo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Processo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Processo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Processo Processo = new Processo();
                Processo.Nome = collection["Nome"];

                ProcessoService.salvarProcesso(Processo);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Processo/Edit/5
        public ActionResult Edit(int id)
        {
            return View(ProcessoService.carregarPorId(id));
        }

        // POST: Processo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Processo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Processo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
