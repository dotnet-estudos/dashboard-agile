using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.InfraData.Contexto;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    public class TeamsController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Teams
        public ActionResult Index()
        {
            var projetos = db.Projetos.Include(p => p.Processo);
            return View(projetos.ToList());
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projetos.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            ViewBag.ProcessoId = new SelectList(db.Processos, "ProcessoId", "Nome");
            return View();
        }

        // POST: Teams/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjetoId,Descricao,IdExterno,Identifier,Ativo,HorasContratadas,QuantidadeDesenvolvedores,ProcessoId")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                db.Projetos.Add(projeto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProcessoId = new SelectList(db.Processos, "ProcessoId", "Nome", projeto.ProcessoId);
            return View(projeto);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projetos.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProcessoId = new SelectList(db.Processos, "ProcessoId", "Nome", projeto.ProcessoId);
            return View(projeto);
        }

        // POST: Teams/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjetoId,Descricao,IdExterno,Identifier,Ativo,HorasContratadas,QuantidadeDesenvolvedores,ProcessoId")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projeto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProcessoId = new SelectList(db.Processos, "ProcessoId", "Nome", projeto.ProcessoId);
            return View(projeto);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projetos.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projeto projeto = db.Projetos.Find(id);
            db.Projetos.Remove(projeto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
