using mis.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace mis.Controllers
{

    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {

            // join two table

            //var report = await db.Lp.Join(db.Pc, x => x.id, y => y.id, (x, y) => new Mymodel
            //{
            //    lp = x,
            //    pc = y,
            //}).ToListAsync();

            //foreach (var reportItem in report)
            //{
            //    Debug.WriteLine(reportItem);
            //}

            Mymodel model = new Mymodel();

            model.adList = await db.allotment_detail.ToListAsync();
            model.ad = await db.allotment_detail.FirstOrDefaultAsync();

            model.EmployeeList = await db.Employee.ToListAsync();

            var allot = db.allotment_detail.ToList();

            model.EmployeeList.RemoveAll(a => allot.Exists(w => w.Employee_name == a.Name));

            ViewData["emptbl"] = new SelectList(model.EmployeeList, "Name", "id");

            var list = new List<string>()
            {
                "Personal Computer",
                "Laptop"
            };

            ViewData["product"] = new SelectList(list);


            var list1 = new List<string>()
            {
                "Table",
                "Mouse",
                "Keyboard"
            };

            ViewData["ass"] = new SelectList(list1);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Allot_product([Bind(Include = "id,Employee_name,Product_alloted,Product_id,Accessories")] allotment_detail ad)
        {
            if (ModelState.IsValid)
            {
                db.allotment_detail.Add(ad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            var allot = db.allotment_detail.ToList();
            List<Employee> emplist = db.Employee.ToList();
            emplist.RemoveAll(a => allot.Exists(w => w.Employee_name == a.Name));
            ViewData["emptbl"] = new SelectList(emplist, "Name", "Name");

            var list = new List<string>()
            {
                "Personal Computer",
                "Laptop"
            };

            ViewData["product"] = new SelectList(list);
            var list1 = new List<string>()
            {
                "Table",
                "Mouse",
                "Keyboard"
            };
            ViewData["ass"] = new SelectList(list1);
            return View("Index");
        }

        public JsonResult product_id(string Product_alloted, allotment_detail ad)
        {
            var allot = db.allotment_detail.ToList();

            if (Product_alloted == "Personal Computer")
            {
                db.Configuration.ProxyCreationEnabled = false;
                var pc = db.Pc.ToList();

                pc.RemoveAll(a => allot.Exists(w => w.Product_id == a.Product_id));

                return Json(pc, JsonRequestBehavior.AllowGet);
            }
            db.Configuration.ProxyCreationEnabled = false;
            var lp = db.Lp.ToList();
            lp.RemoveAll(a => allot.Exists(w => w.Product_id == a.Product_id));
            return Json(lp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            allotment_detail allotment_Detail = await db.allotment_detail.FindAsync(id);
            if (allotment_Detail == null)
            {
                return HttpNotFound();
            }
            return View(allotment_Detail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(allotment_detail ad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ad);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            allotment_detail allotment_detail = await db.allotment_detail.FindAsync(id);
            if (allotment_detail == null)
            {
                return HttpNotFound();
            }
            return View(allotment_detail);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            allotment_detail allotment_detail = await db.allotment_detail.FindAsync(id);

            db.allotment_detail.Remove(allotment_detail);

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}