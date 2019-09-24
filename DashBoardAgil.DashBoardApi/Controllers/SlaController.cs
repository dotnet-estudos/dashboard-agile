using System.Web.Mvc;

namespace DashBoardAgil.DashBoardApi.Controllers
{
    public class SlaController : BaseController
    {
        // GET: Sla
        public ActionResult Index()
        {
           
            return View();
        }

        // GET: Sla/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sla/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sla/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sla/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sla/Edit/5
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

        // GET: Sla/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sla/Delete/5
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
