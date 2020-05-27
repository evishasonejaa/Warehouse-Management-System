using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;

namespace WMS.Controllers
{

    public class tbl_clientController : Controller
    {
        private WMSEntities db = new WMSEntities();

        // GET: tbl_client
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)

        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "client_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Contact Person" ? "c_desc" : "Contact Person";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var client = from c in db.tbl_client
                         select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                client = client.Where(s => s.client_name.Contains(searchString)
                                       || s.contact_person.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "client_desc":
                    client = client.OrderByDescending(c => c.client_name);
                    break;
                case "Contact Person":
                    client = client.OrderBy(c => c.contact_person);
                    break;
                case "c_desc":
                    client = client.OrderByDescending(c => c.contact_person);
                    break;
                default:
                    client = client.OrderBy(c => c.client_name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(client.ToPagedList(pageNumber, pageSize));

        }



        // GET: tbl_client/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_client tbl_client = db.tbl_client.Find(id);
            if (tbl_client == null)
            {
                return HttpNotFound();
            }
            return View(tbl_client);
        }

        // GET: tbl_client/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_client/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "client_name,contact_person,phone,email,consumer_email,claim_email,fax,add_1,add_2,city,province,country,zip")] tbl_client tbl_client)
        {
            if (ModelState.IsValid)
            {
                db.tbl_client.Add(tbl_client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_client);
        }

        // GET: tbl_client/Edit/5
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_client tbl_client = db.tbl_client.Find(id);
            if (tbl_client == null)
            {
                return HttpNotFound();
            }
            return View(tbl_client);
        }

        // POST: tbl_client/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "client_name,contact_person,phone,email,consumer_email,claim_email,fax,add_1,add_2,city,province,country,zip")] tbl_client tbl_client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_client);
        }

        // GET: tbl_client/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_client tbl_client = db.tbl_client.Find(id);
            if (tbl_client == null)
            {
                return HttpNotFound();
            }
            return View(tbl_client);
        }

        // POST: tbl_client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tbl_client tbl_client = db.tbl_client.Find(id);
            db.tbl_client.Remove(tbl_client);
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
