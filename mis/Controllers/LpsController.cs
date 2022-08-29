using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mis.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;


namespace mis.Controllers
{
    [Authorize]
    public class LpsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Lps
        public async Task<ActionResult> Index()
        {
            return View(await db.Lp.ToListAsync());
        }
        public void lpexcel()
        {
            var grid = new GridView();
            grid.DataSource = db.Lp.ToList();
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Pc Data.xls");
            Response.ContentType = "application/excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);
            grid.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString() + "\n");
            Response.End();
        }
        // GET: Lps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lp lp = await db.Lp.FindAsync(id);
            if (lp == null)
            {
                return HttpNotFound();
            }
            return View(lp);
        }

        // GET: Lps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Company_name,Model_no,Product_id")] Lp lp)
        {
            if (ModelState.IsValid)
            {

                var result = db.Lp.Where(s => s.Product_id == lp.Product_id).ToList();
                if (result.Count == 0)
                {
                    db.Lp.Add(lp);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Employees");
                }
                else
                {
                    ModelState.AddModelError("", "Item already exists");
                }
            }

            return View(lp);
        }

        // GET: Lps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lp lp = await db.Lp.FindAsync(id);
            if (lp == null)
            {
                return HttpNotFound();
            }
            return View(lp);
        }

        // POST: Lps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Company_name,Model_no,Product_id")] Lp lp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lp).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }
            return View(lp);
        }

        // GET: Lps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lp lp = await db.Lp.FindAsync(id);
            if (lp == null)
            {
                return HttpNotFound();
            }
            return View(lp);
        }

        // POST: Lps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lp lp = await db.Lp.FindAsync(id);
            db.Lp.Remove(lp);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Employees");
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
