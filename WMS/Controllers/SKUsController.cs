using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WMS.Controllers
{
    public class SKUsController : Controller
    {
        private WMSEntities db = new WMSEntities();


        // GET: SKUs
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var sKUs = db.SKUs.Include(s => s.tbl_client);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "SKU" : "";
            ViewBag.NameSortParm = sortOrder == "Client" ? "c_desc" : "Client";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var sKU = from s in db.SKUs
                      select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sKU = sKU.Where(s => s.client_name.Contains(searchString)
                                       || s.SKU1.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "SKU":
                    sKU = sKU.OrderByDescending(s => s.SKU1);
                    break;
                case "Client":
                    sKU = sKU.OrderBy(s => s.client_name);
                    break;
                case "c_desc":
                    sKU = sKU.OrderByDescending(s => s.client_name);
                    break;
                default:
                    sKU = sKU.OrderBy(s => s.SKU1);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(sKU.ToPagedList(pageNumber, pageSize));

        }




        // GET: SKUs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKU sKU = db.SKUs.Find(id);
            if (sKU == null)
            {
                return HttpNotFound();
            }
            return View(sKU);
        }

        // GET: SKUs/Create
        public ActionResult Create()
        {
            ViewBag.client_name = new SelectList(db.tbl_client, "client_name", "client_name");
            return View();
        }

        // POST: SKUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "client_name,SKU1,description,length,width,height,rate,supplier,cost,category,notes")] SKU sKU, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);

                    sKU.image_url = Url.Content("~/Images/" + fileName);

                }
                try
                {



                    db.SKUs.Add(sKU);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Trace.TraceError("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.TraceError("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }

                return RedirectToAction("Index");
            }
            ViewBag.client_name = new SelectList(db.tbl_client, "client_name", "contact_person", sKU.client_name);
            return View(sKU);
        }

        // GET: SKUs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKU sKU = db.SKUs.Find(id);
            if (sKU == null)
            {
                return HttpNotFound();
            }
            Session["Image"] = sKU.image_url;
            ViewBag.client_name = new SelectList(db.tbl_client, "client_name", "client_name");
            return View(sKU);
        }

        // POST: SKUs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SKU_id,client_name,SKU1,description,length,width,height,rate,supplier,cost,category,notes")] SKU sKU, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);

                    sKU.image_url = Url.Content("~/Images/" + fileName);
                    db.Entry(sKU).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    sKU.image_url = Session["Image"].ToString();
                    db.Entry(sKU).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.client_name = new SelectList(db.tbl_client, "client_name", "client_name");
            return View(sKU);
        }

        // GET: SKUs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKU sKU = db.SKUs.Find(id);
            if (sKU == null)
            {
                return HttpNotFound();
            }
            return View(sKU);
        }

        // POST: SKUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SKU sKU = db.SKUs.Find(id);
            db.SKUs.Remove(sKU);
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
