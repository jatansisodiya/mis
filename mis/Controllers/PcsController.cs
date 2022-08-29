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
    public class PcsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pcs
        public async Task<ActionResult> Index()
        {
            return View(await db.Pc.ToListAsync());
        }


        public void PcCsv()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"Id\",\"Company Name\",\"Model No\",\"Product ID\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Pc Data.csv");
            Response.ContentType = "text/csv";

            var Pc = db.Pc.ToList();

            foreach (var pc in Pc)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                    pc.id,
                    pc.Company_name,
                    pc.Model_no,
                    pc.Product_id
                    ));

            }
            Response.Write(sw.ToString());
            Response.End();
        }

        // GET: Pcs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pc pc = await db.Pc.FindAsync(id);
            if (pc == null)
            {
                return HttpNotFound();
            }
            return View(pc);
        }

        // GET: Pcs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pcs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Company_name,Model_no,Product_id")] Pc pc)
        {
            if (ModelState.IsValid)
            {
                var result = db.Pc.Where(s => s.Product_id == pc.Product_id).ToList();
                if (result.Count == 0)
                {
                    db.Pc.Add(pc);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Employees");
                }
                else
                {
                    ModelState.AddModelError("", "Item already exists");
                }
            }

            return View(pc);
        }

        // GET: Pcs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pc pc = await db.Pc.FindAsync(id);
            if (pc == null)
            {
                return HttpNotFound();
            }
            return View(pc);
        }

        // POST: Pcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Company_name,Model_no,Product_id")] Pc pc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pc).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }
            return View(pc);
        }

        // GET: Pcs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pc pc = await db.Pc.FindAsync(id);
            if (pc == null)
            {
                return HttpNotFound();
            }
            return View(pc);
        }

        // POST: Pcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pc pc = await db.Pc.FindAsync(id);
            db.Pc.Remove(pc);
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
