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
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Diagnostics;
using PagedList;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System.Web.Security;

namespace mis.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Mymodel model = new Mymodel();
        // GET: Employees
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(int? page)
        {
            model.EmployeeList = await db.Employee.ToListAsync();
            TempData["TotalEmployee"] = model.EmployeeList.Count();

         

            model.PcList = await db.Pc.ToListAsync();
            TempData["TotalPc"] = model.PcList.Count();
            model.LpList = await db.Lp.ToListAsync();
            TempData["TotalLp"] = model.LpList.Count();

            model.adList = await db.allotment_detail.ToListAsync();

            model.ad = await db.allotment_detail.FirstOrDefaultAsync();
            int pageSize = 2;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            model.employees = db.Employee.ToList().ToPagedList(pageIndex, pageSize);
            model.Pcpaged = db.Pc.ToList().ToPagedList(pageIndex, pageSize);
            model.Lppaged = db.Lp.ToList().ToPagedList(pageIndex, pageSize);
            model.allotPaged = db.allotment_detail.ToList().ToPagedList(pageIndex, pageSize);
            return View(model);
        }

        public void EmployeeExcel()
        {
            var grid = new GridView();
            grid.DataSource = db.Employee.ToList();
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Employee Data.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);
            grid.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString() + "\n");
            Response.End();

        }
        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employee.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,User_id,Name,Address,Mobile_number,Email_address,Aadhaar_number,Pan_number,Birth_date,Job_title,Department,Supervisor,Work_location")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var result = db.Employee.Where(s => s.Email_address == employee.Email_address).ToList();
                if (result.Count == 0)
                {
                    employee.Joining_date = DateTime.Now.ToString();
                    db.Employee.Add(employee);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Email already Exists");
                }

            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employee.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,User_id,Name,Address,Mobile_number,Email_address,Aadhaar_number,Pan_number,Birth_date,Job_title,Department,Supervisor,Work_location")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employee.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employee employee = await db.Employee.FindAsync(id);

            employee.Resignation_date = DateTime.Now.ToString();

            if (employee.Resignation_date == null)
            {
                db.Employee.Add(employee);
            }
           
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult ColumnChart()
        {
            return View();
        }

        public ActionResult Chart()
        {
            return View();
        }

        public ActionResult Employee_joining()
        {
            var stdResult = db.Employee.ToList();
            return Json(stdResult, JsonRequestBehavior.AllowGet);
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
